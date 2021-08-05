using FoodMarketDMS.Business.Interfaces;
using FoodMarketDMS.Business.Models;
using FoodMarketDMS.Services.Interfaces;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FoodMarketDMS.Services
{
    public class StateWrapperService : IStateWrapperService
    {
        private const string USER_SHEET = "이용자 목록";
        private const string SERVICE_SHEET = "서비스 목록";
        private const string OFFER_SHEET = "제공 목록";

        public List<UserClass> Users { get; set; } = new List<UserClass>();
        public List<ServiceClass> Services { get; set; } = new List<ServiceClass>();
        public List<OfferClass> Offers { get; set; } = new List<OfferClass>();


        public async void SaveState()
        {
            string[][][] data = new string[3][][];
            data[0] = new string[Users.Count][];
            data[1] = new string[Services.Count][];
            data[2] = new string[Offers.Count][];

            //for (int i = 0, l = Users.Count; i < l; i++)
            //{
            //    data[0][i] = Users[i].ToStringArray();
            //}
            //for (int i = 0, l = Services.Count; i < l; i++)
            //{
            //    data[1][i] = Services[i].ToStringArray();
            //}
            //for (int i = 0, l = Offers.Count; i < l; i++)
            //{
            //    data[2][i] = Offers[i].ToStringArray();
            //}

            SetStringArray(ref data[0], Users.Cast<IStringArraySerializable>().ToArray());
            SetStringArray(ref data[1], Services.Cast<IStringArraySerializable>().ToArray());
            SetStringArray(ref data[2], Offers.Cast<IStringArraySerializable>().ToArray());

            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(data);
            await File.WriteAllBytesAsync(IStateWrapperService.STATE_PATH, jsonUtf8Bytes);
        }

        private void SetStringArray(ref string[][] data, IStringArraySerializable[] originData)
        {
            data = new string[originData.Length][];
            for (int i = 0, l = originData.Length; i < l; i++)
            {
                data[i] = originData[i].ToStringArray();
            }
        }

        public void LoadState()
        {
            byte[] jsonUtf8Bytes = ReadDataAsync().Result;
            if (jsonUtf8Bytes is null)
            {
                return;
            }

            var utf8Reader = new Utf8JsonReader(jsonUtf8Bytes);
            var jsonData = JsonSerializer.Deserialize<string[][][]>(ref utf8Reader);

            Users = new List<UserClass>(jsonData[0].GetLength(0));
            Services = new List<ServiceClass>(jsonData[1].GetLength(0));
            Offers = new List<OfferClass>(jsonData[2].GetLength(0));

            Task taskUser = new Task(() => LoadUsers(jsonData[0]));
            Task taskService = new Task(() => LoadServices(jsonData[1]));
            Task taskOffer = new Task(() => LoadOffers(jsonData[2]));
            taskUser.Start();
            taskService.Start();

            taskUser.Wait();

            taskOffer.Start();
            taskService.Wait();
            taskOffer.Wait();
        }

        private async Task<byte[]> ReadDataAsync()
        {
            byte[] data;
            try
            {
                data = await File.ReadAllBytesAsync(IStateWrapperService.STATE_PATH);
            }
            catch (FileNotFoundException e)
            {

                MessageBox.Show(e.Message);

                return null;
            }

            return data;
        }

        private void LoadUsers(string[][] data)
        {
            foreach (string[] user in data)
            {
                Users.Add(new UserClass(user));
            }
            Users.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        private void LoadServices(string[][] data)
        {
            foreach (string[] service in data)
            {
                Services.Add(new ServiceClass(service));
            }
            Services.Sort((x, y) => x.Date.CompareTo(y.Date));
        }

        private void LoadOffers(string[][] data)
        {
            foreach (string[] offer in data)
            {
                Offers.Add(new OfferClass(offer, Users.Find(user => user.Id == OfferClass.GetUserIdFromStringData(offer))));
            }
            Offers.Sort((x, y) => x.Date.CompareTo(y.Date));
        }

        public void ExportToExcel(string path, IExcelService excelService)
        {
            var userData = Users.Select(user => user.ExportStringArray()).ToList();
            userData.Insert(0, UserClass.PropertyNames);

            var serviceData = Services.Select(service => service.ExportStringArray()).ToList();
            serviceData.Insert(0, ServiceClass.PropertyNames);


            var offerData = Offers.Select(offer => offer.ExportStringArray()).ToList();
            offerData.Insert(0, OfferClass.PropertyNames);

            string[][][] excelData = new string[3][][] { userData.ToArray(), serviceData.ToArray(), offerData.ToArray() };
            excelService.SaveToExcel(excelData, path, new string[] { USER_SHEET, SERVICE_SHEET, OFFER_SHEET });
        }
    }
}
