#if !DEMO
using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using gamlib;
using rabnet.forms;
using rabnet.RNC;
using System.Text;

namespace rabnet
{
    public static class ExcelMaker
    {
        // todo сделать эти параметры в настройках
        const string EXPORT_FOLDER = "export";
        const string SEPARATOR = ";";
        const string EXTENTION = ".csv";
        const string TARG_ENCODING = "windows-1251";

        private static XmlNode[] _xmls;
        private static string _repName;


        public static void MakeExcelFromXML(XmlNode[] xmls, String repName, string[] headers)///Для плагинов сделать excel тоже наверное нужны делегаты или еще что
        {
            _repName = repName;
            _xmls = xmls;
            string path = getExportFolderPath();

            if (path == "") return;
            path = Helper.DuplicateName(Path.Combine(path, filename()));

            WaitForm wf = new WaitForm();
            StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.GetEncoding(TARG_ENCODING));

            try
            {
                wf.Flush(); wf.MaxValue = 100; wf.Show(); wf.Style = ProgressBarStyle.Blocks;
                wf.MaxValue = xmls[0].FirstChild.ChildNodes.Count;

                string row = "";                

                for (int h = 0; h < headers.Length; h++)
                {
                    row += String.Format("\"{0}\"{1}", headers[h], SEPARATOR);
                }
                sw.WriteLine(row.TrimEnd(','));
                
                foreach (XmlNode nd in xmls[0].FirstChild.ChildNodes)
                {
                    row = "";
                    foreach (XmlNode nd2 in nd.ChildNodes)
                    {
                        row += String.Format("\"{0}\"{1}", nd2.InnerText, SEPARATOR);                        
                    }

                    sw.WriteLine(row.TrimEnd(SEPARATOR.ToCharArray()));
                    wf.Inc();
                }

                sw.Flush();
                sw.Close();

                wf.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка");
                wf.Visible = false;
            }
        }

        /// <summary>
        /// Экспорт в ListView
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="name"></param>
        public static void MakeExcelFromLV(ListView lv, string name)
        {            
            int refreshRate = lv.Items.Count / 100;

            WaitForm wf = new WaitForm();           

            try
            {
                string path = getExportFolderPath();
                if (path == "") return;

                path = Helper.DuplicateName(Path.Combine(path,name + " " + DateTime.Now.ToShortDateString() + EXTENTION));                

                wf.Flush(); wf.MaxValue = 100; wf.Show(); wf.Style = ProgressBarStyle.Blocks;

                StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.GetEncoding(TARG_ENCODING));
                
                int cols = lv.Columns.Count;//для обеспечения быстроты заполнения

                /// Заполнение названиями когонок    
                string row = "";
                for (int h = 0; h < lv.Columns.Count; h++)
                {
                    row += String.Format("\"{0}\"{1}", lv.Columns[h].Text, SEPARATOR);
                }
                sw.WriteLine(row.TrimEnd(SEPARATOR.ToCharArray()));
                    
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    row = "";
                    for (int j = 0; j < cols; j++)
                    {
                        row += String.Format("\"{0}\"{1}", lv.Items[i].SubItems[j].Text, j != cols - 1 ? SEPARATOR : "");
                    }
                    sw.WriteLine(row.TrimEnd(SEPARATOR.ToCharArray()));
                    if (refreshRate != 0 && (i % refreshRate == 0 || i == lv.Items.Count))
                    {
                        Application.DoEvents();
                        if(!wf.isFull)
                            wf.Inc();
                    }
                }
                sw.Flush();
                sw.Close();

                wf.Hide();
            }
            catch (Exception ex)
            {
                wf.Hide();
                MessageBox.Show(ex.Message, "Произошла ошибка");
            }
        }

        private static string getExportFolderPath()
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
                    path = Helper.PathCombine(path,RabnetConfig.MY_DOCUMENTS_APP_FOLDER,EXPORT_FOLDER);
                    Engine.opt().setOption(Options.OPT_ID.XLS_FOLDER, path);
                }                
            }
            if (path != "" && !Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
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
                filename += ")" + EXTENTION;
            }
            else
            {
                filename = _repName + " " + DateTime.Now.ToShortDateString() + EXTENTION;
            }
            return filename;
        }

        /// <summary>
        /// Заполняет заголовки колонок.
        /// (по хорошему их нужно передавать при формировании ХМЛ)
        /// </summary>
        /// <param name="repType">Тип отчета</param>
        /// <param name="xlWorkSheet"></param>
        //private static void drawHeader(string[] headers, ref Excel.Worksheet xlWS)
        //{
        //    xlWS.get_Range("A1", "Z1").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
        //    xlWS.get_Range("A1", "Z1").Font.Bold = true;
        //    int col = 1;
        //    int row = 1;
        //    foreach (string s in headers)
        //        xlWS.Cells[row, col++] = s;
        //}

        //private static string[,] doMegaMatr(XmlNode roteNode)
        //{
        //    try
        //    {
        //        List<string> horisontal = new List<string>();
        //        horisontal.Add("");
        //        List<string> vertical = new List<string>();
        //        vertical.Add("");
        //        foreach (XmlNode nd in roteNode.FirstChild.ChildNodes)
        //        {
        //            if (!horisontal.Contains(nd.SelectSingleNode("dt").InnerText))
        //                horisontal.Add(nd.SelectSingleNode("dt").InnerText);
        //            if (!vertical.Contains(nd.SelectSingleNode("name").InnerText))
        //                vertical.Add(nd.SelectSingleNode("name").InnerText);
        //        }
        //        horisontal.Sort();
        //        vertical.Sort();
        //        String[,] megaMatrix = new String[vertical.Count,horisontal.Count];
        //        for (int i = 0; i < vertical.Count; i++)
        //            megaMatrix[i, 0] = vertical[i];
        //        for (int i = 0; i < horisontal.Count; i++)               
        //            megaMatrix[0, i] = horisontal[i];
        //        foreach (XmlNode nd in roteNode.FirstChild.ChildNodes)
        //        {
        //            int rowIndex = vertical.IndexOf(nd.SelectSingleNode("name").InnerText);
        //            int colIndex = horisontal.IndexOf(nd.SelectSingleNode("dt").InnerText);             
        //            megaMatrix[rowIndex, colIndex] = nd.SelectSingleNode("state").InnerText;
        //        }
        //        return megaMatrix;
        //    }
        //    catch {return new string[0,0];}
        //}
    }
    

}
#endif
