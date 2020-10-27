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
        public List<UserClass> Users { get; set; } = new List<UserClass>();
        public List<ServiceClass> Services { get; set; } = new List<ServiceClass>();
        public List<OfferClass> Offers { get; set; } = new List<OfferClass>();


        public async void SaveState()
        {
//#if DEBUG
//            Services.Add(new ServiceClass(new DateTime(2020, 10, 1), new List<string> { "토닉워터", "캔커피1", "치즈과자5", "거울", "치약", "냉동닭", "빵", "김", "마스크팩" }));
//#endif
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
            taskOffer.Start();

            taskUser.Wait();
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
#if DEBUG
                MessageBox.Show(e.Message);
#endif
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
        }

        private void LoadServices(string[][] data)
        {
            foreach (string[] service in data)
            {
                Services.Add(new ServiceClass(service));
            }
        }

        private void LoadOffers(string[][] data)
        {
            foreach (string[] offer in data)
            {
                Offers.Add(new OfferClass(offer));
            }
        }

        public void ExportToExcel(string path, IExcelService excelService)
        {
            //excelService.SaveToExcel(Users, path, 1);
            //excelService.SaveToExcel(Services, path, 2);
            //excelService.SaveToExcel(Offers, path, 3);
        }
    }
}
