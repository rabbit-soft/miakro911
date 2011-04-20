using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;

namespace rabnet
{
    public class TestReport:IReportInterface
    {
        const string _fileName = "testplgn";

        public string UniqueName
        {
            get { return "TestReport"; }
        }

        public string MenuText
        {
            get { return "Тестовый плагин"; }
        }

        public void MakeReport()
        {
            PeriodForm dlg = new PeriodForm(MenuText);
            Filters f = new Filters();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                f["dttp"] = dlg.PeriodChar;
                f["dtval"] = dlg.DateValue;
                (new ReportViewForm(MenuText, FileName, new XmlDocument[]
                {
                   Engine.db().makeReport(getSQL(f)),
                   dlg.getXml()
                }
                )).ShowDialog();
            }
        }

        public string FileName
        {
            get 
            {
                checkFile();
                return _fileName; 
            }
        }

        private void checkFile()
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\reports\\" + _fileName + ".rdl";
                if (!File.Exists(path))
                {
                    /*string[] s = Assembly.GetExecutingAssembly().GetManifestResourceNames();*/
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TestPlugIn.test.rdl");
                    FileStream fileStream = new FileStream(path, FileMode.CreateNew);
                    for (int i = 0; i < stream.Length; i++)
                        fileStream.WriteByte((byte)stream.ReadByte());
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("|Ex| "+ex.Message);
            }
        }

        private string getSQL(Filters f)
        {
            string period = "";
            if (f.safeValue("dttp") == "d")
            {
                DateTime dt = DateTime.Parse(f.safeValue("dtval"));
                period = String.Format("DATE(d_date)='{0:yyyy-MM-dd}'", dt);
            }
            else if (f.safeValue("dttp") == "y")
            {
                period = String.Format("YEAR(d_date)={0}", f.safeValue("dtval"));
            }
            else
            {
                DateTime dt = DateTime.Parse(f.safeValue("dtval"));
                period = String.Format("MONTH(d_date)={0:MM} AND YEAR(d_date)={0:yyyy}", dt);
            }
            string s = String.Format(@"
    (SELECT SUM(r_group) grp,
    d_reason,
    (SELECT d_name FROM deadreasons WHERE d_reason=d_id) reason 
FROM dead WHERE {0} GROUP BY d_reason)
UNION 
(SELECT SUM(r_group),0,'Итого' FROM dead WHERE {0});", period);
            return s;
        }

    }
}
