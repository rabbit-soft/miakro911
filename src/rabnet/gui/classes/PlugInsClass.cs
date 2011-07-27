using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace rabnet
{
    class ReportPlugInClass
    {
#if !DEMO
        private static volatile ReportPlugInClass instance;

        public List<IReportInterface> Plugins = new List<IReportInterface>();

        private ReportPlugInClass() 
        {
            foreach (string Filename in Directory.GetFiles(Path.Combine(Application.StartupPath, "reports"), "*.dll"))
            {
                try
                {
                    Assembly Asm = Assembly.LoadFile(Filename);//загружаем Сборку
                    foreach (Type AsmType in Asm.GetTypes())//Проверяем все имеющиеся типы данных (классы)
                    {
                        if (AsmType.GetInterface("IReportInterface") != null)//Если интерфейс у типа
                        {
                            IReportInterface Plugin = (IReportInterface)Activator.CreateInstance(AsmType);
                            Plugins.Add(Plugin);
                        }
                    }
                }
                catch(BadImageFormatException)
                {
                    continue;
                }
            }
       }

        public static ReportPlugInClass Instance
        {
            get
            {
                if (instance == null)
                    instance = new ReportPlugInClass();
                return instance;
            }
        }

        public IReportInterface getPluginByUName(string uname)
        {
            foreach(IReportInterface p in Plugins)
            {
                if (p.UniqueName == uname)
                    return p;
            }
            return null;
        }
#endif
    }

    /*tatic class AsmLoader
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public static bool LoadAssembly(string asmname)
        {
            if (!asmname.EndsWith(".dll"))
                asmname += ".dll"; 
            foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(asm.ManifestModule.Name == asmname)                
                    return true;            
            }
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),asmname);
                if(!File.Exists(path))return false;
                Assembly.LoadFile(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }*/
}
