using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace rabnet
{
    //class ReportPlugInClass
    //{
#if !DEMO
        //private static volatile ReportPlugInClass instance;

        //public List<ReportBase> Plugins = new List<ReportBase>();

        
        /*public static ReportPlugInClass Instance
        {
            get
            {
                if (instance == null)
                    instance = new ReportPlugInClass();
                return instance;
            }
        }*/


#endif
    //}

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
