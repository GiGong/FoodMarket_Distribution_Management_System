using System.ComponentModel;

namespace FoodMarketDMS.Core.Extensions
{
    public static class BackgroundWorkerExtension
    {
        public static void RunAsync(this BackgroundWorker backgroundWorker, DoWorkEventHandler eventHandler)
        {
            backgroundWorker.DoWork += eventHandler;
            backgroundWorker.RunWorkerAsync();
        }
    }
}
