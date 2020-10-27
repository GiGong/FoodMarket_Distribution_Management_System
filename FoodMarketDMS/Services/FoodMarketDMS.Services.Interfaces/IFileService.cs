namespace FoodMarketDMS.Services.Interfaces
{
    public interface IFileService
    {
        public int Index { get; set; }

        /// <summary>
        /// Get file path to open, using OpenFileDialog
        /// </summary>
        /// <param name="filter">File extention filter</param>
        /// <param name="title">File Dialog Title</param>
        /// <returns></returns>
        string OpenFilePath(string filter, string title);

        /// <summary>
        /// Get file path for saving, using SaveFileDialog
        /// </summary>
        /// <param name="filter">File extention filter</param>
        /// <param name="title">File Dialog Title</param>
        /// <returns></returns>
        string SaveFilePath(string filter, string title);
    }
}
