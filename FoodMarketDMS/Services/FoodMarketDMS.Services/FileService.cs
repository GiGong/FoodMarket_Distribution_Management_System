using FoodMarketDMS.Services.Interfaces;
using Microsoft.Win32;

namespace FoodMarketDMS.Services
{
    public class FileService : IFileService
    {
        public int Index { get; set; } = 0;

        public string OpenFilePath(string filter, string title)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = title,
                Filter = filter
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public string SaveFilePath(string filter, string title)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = title,
                Filter = filter
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}
