using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace rabnet
{
    public abstract class ReportBase
    {
        const string FOLDER = "reports\\";

        private string _name = "";
        private string _menuText = "";

        public ReportBase(string nm,string mt)
        {
            _name = nm;
            _menuText = mt;
        }

        /// <summary>
        /// Уникальное имя плагина
        /// </summary>
        public string UniqueName { get { checkFile(); return _name; } }
        /// <summary>
        /// Текст в контекстном меню главной формы
        /// </summary>
        public string MenuText { get { checkFile(); return _menuText; } }
        public string FileName
        {
            get
            {
                checkFile();
                return _name;
            }
        }

        public abstract void MakeReport();

        protected abstract Stream getAssembly();

        private void checkFile()
        {
            try
            {             
                string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),FOLDER) + _name + ".rdl";
                if (!File.Exists(path))
                {
                    Stream stream = getAssembly();
                    if (stream !=null && stream.Length != 0)
                    {
                        FileStream fileStream = new FileStream(path, FileMode.CreateNew);
                        for (int i = 0; i < stream.Length; i++)
                            fileStream.WriteByte((byte)stream.ReadByte());
                        fileStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }
        }

        #region static_members

        public static List<ReportBase> Plugins = new List<ReportBase>();

        public static int CheckPlugins() 
        {
            foreach (string Filename in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "reports"), "*.dll"))
            {                
                try
                {
                    Assembly Asm = Assembly.LoadFile(Filename);//загружаем Сборку
                    foreach (Type AsmType in Asm.GetTypes())//Проверяем все имеющиеся типы данных (классы)
                    {
                        if (AsmType.BaseType==typeof(ReportBase))
                        {
                            ReportBase Plugin = (ReportBase)Activator.CreateInstance(AsmType);
                            if(!alreadyLoaded(Plugin))
                                Plugins.Add(Plugin);
                        }
                    }
                }
                catch { continue; }
            }
            return Plugins.Count;
        }

        public static ReportBase GetPluginByName(string name)
        {
            foreach (ReportBase p in Plugins)
            {
                if (p.UniqueName == name)
                    return p;
            }
            return null;
        }

        private static bool alreadyLoaded(ReportBase rb)
        {
            foreach (ReportBase pl in Plugins)
                if (pl.UniqueName == rb.UniqueName)
                    return true;
            return false;
        }

        #endregion static_members

    }
}
