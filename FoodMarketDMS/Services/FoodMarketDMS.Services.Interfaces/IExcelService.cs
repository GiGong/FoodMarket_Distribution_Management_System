namespace FoodMarketDMS.Services.Interfaces
{
    public interface IExcelService
    {
        public const string EXCEL_FILE_EXT = "Excel Files|*.xls;*.xlsx";

        string[][] GetExcelData(string path, int numOfColumn);
    }
}
