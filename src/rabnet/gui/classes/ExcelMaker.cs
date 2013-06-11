#if !DEMO
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using gamlib;
using rabnet.forms;
using rabnet.RNC;

namespace rabnet
{
    public static class ExcelMaker
    {
        /// <summary>
        /// Если отчет должен быть заполнен данными иначе нежели стандартный отчет. 
        /// Данный делегат используется в MakeExcelFromXML для передачи функции не стандартной обработки.
        /// Нужно для матриц.
        /// </summary>
        public delegate void DataFillCallBack(XmlNode[] xmls, ref Excel.Worksheet xlWorkSheet);

        private static XmlNode[] _xmls;
        private static string _repName;

        public static void MakeExcelFromXML(XmlNode[] xmls, String repName, string[] headers) { MakeExcelFromXML(xmls, repName, headers, null); }
        public static void MakeExcelFromXML(XmlNode[] xmls, String repName, string[] headers, DataFillCallBack dataFill)///Для плагинов сделать excel тоже наверное нужны делегаты или еще что
        {
            _repName = repName;
            _xmls = xmls;
            string path = getXlsFolderPath();
            if (path == "") return;
            path = Helper.DuplicateName(path+ "\\" + filename());

            object misValue = Type.Missing;
            Excel.Application xlApp = new Excel.ApplicationClass();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            WaitForm wf = new WaitForm();
            try
            {
                wf.Flush(); wf.MaxValue = 100; wf.Show(); wf.Style = ProgressBarStyle.Blocks;
                int row, col;
                if (dataFill != null)
                {
                    dataFill(xmls, ref xlWorkSheet);
                }
                else
                {
                    drawHeader(headers, ref xlWorkSheet);
                    row = 2;
                    wf.MaxValue = xmls[0].FirstChild.ChildNodes.Count;
                    foreach (XmlNode nd in xmls[0].FirstChild.ChildNodes)
                    {
                        col = 1;
                        foreach (XmlNode nd2 in nd.ChildNodes)
                        {
                            xlWorkSheet.Cells[row, col] = nd2.InnerText;
                            col++;
                        }
                        row++;
                        wf.Inc();
                    }
                }

                wf.Hide();

                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка");
                wf.Visible = false;
            }
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            killExcelProcess();
        }

        public static void MakeExcelFromLV(ListView lv, string name)
        {
            WaitForm wf = new WaitForm();
            object misValue = Type.Missing;
            Excel.Application xlApp = new Excel.ApplicationClass();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            try
            {
                string path = getXlsFolderPath();
                if (path == "") return;
                path = Helper.DuplicateName(path + "\\" + name + " " + DateTime.Now.ToShortDateString() + ".xls");
                
                wf.Flush(); 
                wf.MaxValue = 100; 
                wf.Show(); 
                wf.Style = ProgressBarStyle.Blocks;

                int rate = lv.Items.Count / 100;
                int cols = lv.Columns.Count;//для обеспечения быстроты заполнения

                for (int h = 0; h < lv.Columns.Count; h++)//Заполнение названиями когонок
                {
                    xlWorkSheet.Cells[1, h + 1] = lv.Columns[h].Text;
                }
                ((Excel.Range)xlWorkSheet.Rows[1, Type.Missing]).Font.Bold = true;
                    
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        xlWorkSheet.Cells[i + 2, j + 1] = lv.Items[i].SubItems[j].Text;
                    }
                    if (rate != 0 && (i % rate == 0 || i == lv.Items.Count))
                    {
                        Application.DoEvents();
                        if(!wf.isFull)
                            wf.Inc();
                    }
                }
                wf.Hide();
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
            }
            catch (Exception ex)
            {
                wf.Hide();
                MessageBox.Show(ex.Message, "Произошла ошибка");
            }
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            killExcelProcess();
        }

        private static string getXlsFolderPath()
        {
            String path = "";
            if (Engine.opt().getIntOption(Options.OPT_ID.XLS_ASK) == 1)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.SelectedPath;                
            }
            else
            {
                path = Engine.opt().getOption(Options.OPT_ID.XLS_FOLDER);
                if (!Directory.Exists(path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    path = Helper.PathCombine(path,RabnetConfig.MY_DOCUMENTS_APP_FOLDER,"excel");
                    Engine.opt().setOption(Options.OPT_ID.XLS_FOLDER, path);
                }                
            }
            if (path != "" && !Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        private static string[,] doMegaMAtr(XmlNode roteNode)
        {
            try
            {
                List<string> horisontal = new List<string>();
                horisontal.Add("");
                List<string> vertical = new List<string>();
                vertical.Add("");
                foreach (XmlNode nd in roteNode.FirstChild.ChildNodes)
                {
                    if (!horisontal.Contains(nd.SelectSingleNode("dt").InnerText))
                        horisontal.Add(nd.SelectSingleNode("dt").InnerText);
                    if (!vertical.Contains(nd.SelectSingleNode("name").InnerText))
                        vertical.Add(nd.SelectSingleNode("name").InnerText);
                }
                horisontal.Sort();
                vertical.Sort();
                String[,] megaMatrix = new String[vertical.Count,horisontal.Count];
                for (int i = 0; i < vertical.Count; i++)
                    megaMatrix[i, 0] = vertical[i];
                for (int i = 0; i < horisontal.Count; i++)               
                    megaMatrix[0, i] = horisontal[i];
                foreach (XmlNode nd in roteNode.FirstChild.ChildNodes)
                {
                    int rowIndex = vertical.IndexOf(nd.SelectSingleNode("name").InnerText);
                    int colIndex = horisontal.IndexOf(nd.SelectSingleNode("dt").InnerText);             
                    megaMatrix[rowIndex, colIndex] = nd.SelectSingleNode("state").InnerText;
                }
                return megaMatrix;
            }
            catch {return new string[0,0];}
        }

        /// <summary>
        /// Убивает процесс excel, т.к. она сам че-то не убивается
        /// </summary>
        private static void killExcelProcess()
        {
            
            Hashtable myHashtable = new Hashtable();
            Process[] AllProcesses = Process.GetProcessesByName("excel");
            int iCount = 0;
            foreach (Process ExcelProcess in AllProcesses)
            {
                myHashtable.Add(ExcelProcess.Id, iCount);
                iCount = iCount + 1;
            }

            AllProcesses = Process.GetProcessesByName("excel");
            // check to kill the right process
            foreach (Process ExcelProcess in AllProcesses)
            {
                if (myHashtable.ContainsKey(ExcelProcess.Id))
                    ExcelProcess.Kill();
            }
            AllProcesses = null;
        }

        private static string filename()
        {
            string filename = "";
            if (_xmls.Length > 1)
            {
                filename += _repName + " (";
                foreach (XmlNode nd in _xmls[1].FirstChild.ChildNodes)
                {
                    foreach (XmlNode nd2 in nd.ChildNodes)
                        filename += nd2.InnerText + " ";
                }
                filename += ").xls";
            }
            else
            {
                filename = _repName + " " + DateTime.Now.ToShortDateString() + ".xls";
            }
            return filename;
        }

        /// <summary>
        /// Заполняет заголовки колонок.
        /// (по хорошему их нужно передавать при формировании ХМЛ)
        /// </summary>
        /// <param name="repType">Тип отчета</param>
        /// <param name="xlWorkSheet"></param>
        private static void drawHeader(string[] headers, ref Excel.Worksheet xlWS)
        {
            xlWS.get_Range("A1", "Z1").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            xlWS.get_Range("A1", "Z1").Font.Bold = true;
            int col = 1;
            int row = 1;
            foreach (string s in headers)
                xlWS.Cells[row, col++] = s;
        }
    }
    

}
#endif
