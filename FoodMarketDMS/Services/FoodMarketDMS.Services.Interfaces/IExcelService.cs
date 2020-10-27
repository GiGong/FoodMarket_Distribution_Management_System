namespace FoodMarketDMS.Services.Interfaces
{
    public interface IExcelService
    {
        public const string EXCEL_FILE_EXT = "Excel Files|*.xls;*.xlsx";

        string[][] GetExcelData(string path, int numOfColumn);
        void SaveToExcel(string[][] data, string path);
        void SaveToExcel(string[][] data, string path, int indexOfWorksheet);
    }
}
