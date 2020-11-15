using FoodMarketDMS.Core.Controls;
using FoodMarketDMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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


        #region Load, Get Excel Data

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

            worker.RunWorkerAsync(new ExcelLoadParameter { Path = path, NumOfColumn = numOfColumn });
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

            var param = (ExcelLoadParameter)e.Argument;
            string path = param.Path;
            int numOfColumn = param.NumOfColumn;

            var worker = sender as BackgroundWorker;
            List<string[]> list = new List<string[]>();

            try
            {
                worker.ReportProgress(0, "Opening Excel...");

                excelApp = new Excel.Application();

                // 엑셀 파일 열기
                wb = excelApp.Workbooks.Open(path, ReadOnly: true);

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

                wb.Close();
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

        #endregion Load, Get Excel Data


        public void SaveToExcel(string[][] data, string path)
        {
            throw new NotImplementedException();
        }

        public void SaveToExcel(string[][] data, string path, int indexOfWorksheet, string nameOfWorksheet = null)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += SaveExcelData;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            _doneEvent = new AutoResetEvent(false);
            _progressWindow = new ProgressWindow();

            worker.RunWorkerAsync(new ExcelSaveParameter { Path = path, IndexOfWorksheet = indexOfWorksheet, NameOfWorksheet = nameOfWorksheet, Data = data });
            _progressWindow.ShowDialog();

            _doneEvent.WaitOne();
            KillExcel();
        }

        private void SaveExcelData(object sender, DoWorkEventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            var param = (ExcelSaveParameter)e.Argument;
            string path = param.Path;
            int indexOfWorksheet = param.IndexOfWorksheet;
            string nameOfWorksheet = param.NameOfWorksheet;
            var originData = param.Data;

            if (originData.Length == 0)
            {
                _doneEvent.Set();
                return;
            }

            var worker = sender as BackgroundWorker;

            try
            {
                worker.ReportProgress(0, "Opening Excel...");

                excelApp = new Excel.Application();

                // 엑셀 파일 열기
                wb = excelApp.Workbooks.Add(Missing.Value);

                worker.ReportProgress(3);


                ws = (wb.Worksheets.Count < indexOfWorksheet ? wb.Worksheets.Add() : wb.Worksheets[indexOfWorksheet]) as Excel.Worksheet;


                if (!string.IsNullOrWhiteSpace(nameOfWorksheet))
                {
                    ws.Name = nameOfWorksheet;
                }
                worker.ReportProgress(5, "Saving Data...");

                int row = originData.GetLength(0);
                int column = originData[0].Length;

                object[,] data = new object[row, column];

                for (int r = 0; r < row; r++)
                {
                    for (int c = 0; c < column; c++)
                    {
                        data[r, c] = originData[r][c];
                    }

                    (sender as BackgroundWorker).ReportProgress((int)((double)r / row * 100));
                }

                //Excel.Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[row, column]];
                //rng.Value = data;

                Excel.Range rng = ws.get_Range("A1", Missing.Value);
                rng = rng.get_Resize(row, column);
                rng.set_Value(Missing.Value, data);

                wb.SaveAs(path);
                wb.Close();
                excelApp.Quit();
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

        public void SaveToExcel(string[][][] data, string path, string[] namesOfWorksheet = null)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += SaveExcelSheets;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            _doneEvent = new AutoResetEvent(false);
            _progressWindow = new ProgressWindow();

            worker.RunWorkerAsync(new ExcelMultiSaveParameter { Path = path, NamesOfWorksheet = namesOfWorksheet.Reverse().ToArray(), Data = data.Reverse().ToArray() });
            _progressWindow.ShowDialog();

            _doneEvent.WaitOne();
            KillExcel();
        }

        private void SaveExcelSheets(object sender, DoWorkEventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            var param = (ExcelMultiSaveParameter)e.Argument;
            string path = param.Path;
            string[] namesOfWorksheet = param.NamesOfWorksheet;
            string[][][] originData = param.Data;

            if (originData.Length == 0)
            {
                _doneEvent.Set();
                return;
            }

            var worker = sender as BackgroundWorker;

            try
            {
                worker.ReportProgress(0, "Opening Excel...");

                excelApp = new Excel.Application();

                // 엑셀 파일 열기
                wb = File.Exists(path) ? excelApp.Workbooks.Open(path, ReadOnly: false, Editable: true) : excelApp.Workbooks.Add(Missing.Value);

                worker.ReportProgress(1);

                #region Worksheet
                for (int i = 0, numOfWorksheet = 1, l = originData.Length; i < l; i++, numOfWorksheet++)
                {
                    try
                    {
                        ws = wb.Worksheets[namesOfWorksheet[i]];
                    }
                    catch (Exception)
                    {
                        ws = (wb.Worksheets.Count < numOfWorksheet ? wb.Worksheets.Add() : wb.Worksheets[numOfWorksheet]) as Excel.Worksheet;
                    }

                    string name = namesOfWorksheet[i];
                    if (!string.IsNullOrWhiteSpace(name) && ws.Name != name)
                    {
                        ws.Name = name;
                    }
                    worker.ReportProgress(1, $"Saving Data... {numOfWorksheet} / {l}");

                    int row = originData[i].Length;
                    int column = originData[i][0].Length;

                    object[,] data = new object[row, column];

                    for (int r = 0; r < row; r++)
                    {
                        for (int c = 0; c < column; c++)
                        {
                            data[r, c] = originData[i][r][c];
                        }

                        (sender as BackgroundWorker).ReportProgress((int)((double)r / row * 100));
                    }

                    Excel.Range rng = ws.get_Range("A1", Missing.Value);
                    rng = rng.get_Resize(row, column);
                    rng.set_Value(Missing.Value, data);
                    rng.Columns.AutoFit();
                }
                #endregion Worksheet

                wb.SaveAs(path);
                wb.Close();
                excelApp.Quit();
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

        #region Parameters

        private struct ExcelLoadParameter
        {
            public string Path { get; set; }
            public int NumOfColumn { get; set; }
        }

        private struct ExcelSaveParameter
        {
            public string Path { get; set; }
            public int IndexOfWorksheet { get; set; }
            public string NameOfWorksheet { get; set; }
            public string[][] Data { get; set; }
        }

        private struct ExcelMultiSaveParameter
        {
            public string Path { get; set; }
            public string[] NamesOfWorksheet { get; set; }
            public string[][][] Data { get; set; }
        }

        #endregion Parameters
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
