using FoodMarketDMS.Business.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace FoodMarketDMS.Services.Interfaces
{
    public interface IStateWrapperService
    {
        public static readonly string STATE_PATH = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state.json")}";

        List<UserClass> Users { get; set; }
        List<ServiceClass> Services { get; set; }
        List<OfferClass> Offers { get; set; }

        void SaveState();
        void LoadState();

        void ExportToExcel(string path, IExcelService excelService);
    }
}
