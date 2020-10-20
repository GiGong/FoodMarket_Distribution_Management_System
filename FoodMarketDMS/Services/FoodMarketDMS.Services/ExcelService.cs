using FoodMarketDMS.Core.Controls;
using FoodMarketDMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using Excel = Microsoft.Office.Interop.Excel;

namespace FoodMarketDMS.Services
{
    public class ExcelService : IExcelService
    {
        private static string[][] _result;
        private static ProgressWindow _progressWindow;
        private static AutoResetEvent _doneEvent;

        public string[][] GetExcelData(string path, int numOfColumn)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += ReadExcelData;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            _result = null;
            _doneEvent = new AutoResetEvent(false);
            _progressWindow = new ProgressWindow();

            worker.RunWorkerAsync(new ExcelParameter() { Path = path, NumOfColumn = numOfColumn });
            _progressWindow.ShowDialog();

            _doneEvent.WaitOne();
            KillExcel();

            return _result;
        }

        private static void ReadExcelData(object sender, DoWorkEventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            var param = (ExcelParameter)e.Argument;
            string path = param.Path;
            int numOfColumn = param.NumOfColumn;

            var worker = sender as BackgroundWorker;
            List<string[]> list = new List<string[]>();

            try
            {
                worker.ReportProgress(0, "Opening Excel...");

                excelApp = new Excel.Application();

                // 엑셀 파일 열기
                wb = excelApp.Workbooks.Open(path);

                worker.ReportProgress(3);

                // 첫번째 Worksheet
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                int row = ws.UsedRange.EntireRow.Count;

                // 현재 Worksheet에서 사용된 Range 전체를 선택
                Excel.Range rng = ws.UsedRange;

                //Excel.Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[row, numOfColumn]];

                // Range 데이타를 배열 (One-based array)로
                object[,] data = rng.Value;

                worker.ReportProgress(5, "Loading Data...");

                for (int r = 1; r <= data.GetLength(0); r++)
                {
                    int length = Math.Min(data.GetLength(1), numOfColumn);
                    string[] arr = new string[length];

                    for (int c = 1; c <= length; c++)
                    {
                        if (data[r, c] == null)
                        {
                            continue;
                        }
                        else if (data[r, c] is string)
                        {
                            arr[c - 1] = data[r, c] as string;
                        }
                        else
                        {
                            arr[c - 1] = data[r, c].ToString();
                        }
                    }

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(arr[i]) == false)
                        {
                            list.Add(arr);
                            break;
                        }
                    }

                    worker.ReportProgress((int)((double)r / row * 100));

                }

                wb.Close(true);
                excelApp.Quit();

                _result = list.ToArray();
            }
            catch (Exception ex)
            {
                if (wb != null)
                {
                    wb.Close(SaveChanges: false);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                }
                throw ex;
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
                _doneEvent.Set();
            }
        }

        static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private static void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                _progressWindow.State = e.UserState.ToString();
            }
            _progressWindow.Progress = e.ProgressPercentage;
        }

        private static void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _progressWindow.Close();
        }

        private struct ExcelParameter
        {
            public string Path { get; set; }
            public int NumOfColumn { get; set; }
        }


        /// <summary>
        /// Kill all child excel processes
        /// </summary>
        private void KillExcel()
        {
            Process[] allProcesses = Process.GetProcessesByName("excel");
            int currPsId = Process.GetCurrentProcess().Id;

            // check to kill the child excel process
            foreach (Process ExcelProcess in allProcesses)
            {
                if (ExcelProcess.Parent().Id == currPsId)
                    ExcelProcess.Kill();
            }
        }

    }

    public static class ProcessExtensions
    {
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
    }
}
