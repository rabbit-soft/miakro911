﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using rabnet.RNC;

namespace rabdump
{
    
    class Options//:Object
    {
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum Rubool { Да, Нет };
        private static Options _oops = null;
        //const String Opt = "Настройки";
        const String Z7_NAME = @"7z\7za.exe";
        const String RD_OPTIONS = "rabdumpOptions";
        const String MYSQL_EXE = @"\bin\mysql.exe";
        const String MYSQL_DUMP = @"\bin\mysqldump.exe";
        //const String ArKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        //const String ArValue = "rabdump";        
        //public String MySqlExePath = "";
        //public String MySqlDumpPath = "";

        private String _myPath = "";//@"C:\Program Files\MySQL\MySQL Server 5.1"; 
        private String _p7 = "";
        private String _servUrl;
        private bool _sas = false;
        private RabnetConfig _rnc;
        //private readonly List<DataSource> _databases;
        //private readonly List<ArchiveJob> _jobs ;    

        private Options() { }

        /// <summary>
        /// Синглтон опций
        /// </summary>
        /// <returns></returns>
        public static Options Inst
        {
            get
            {
                if (_oops == null)
                {
                    _oops = new Options();
                    _oops.Load();
                }
                return _oops;
            }
        }

        public String MySqlPath
        {
            get { return _myPath;}
            set
            {
                if (Directory.Exists(value))
                {
                    _myPath = value;
                }
            }
        }
        public String MySqlExePath 
        {
            get {
                return _myPath + MYSQL_EXE;
            }
        }

        public String MySqlDumpPath { get { return _myPath + MYSQL_DUMP; } }

        public string ServerUrl { get { return _servUrl; } set { _servUrl = value; } }

        public String Path7Z
        {
            get 
            {
                if (_p7==null || _p7 == "")
                {
                    string search = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                    DirectoryInfo di = Directory.GetParent(search);
                    search = Path.Combine(di.FullName, Z7_NAME);
                    if (File.Exists(search))
                        return search;
                }
                return _p7; 
            }
            set 
            { 
                if(File.Exists(value))
                    _p7 = value; 
            }
        }

        public List<DataSource> DataSource { get { return _rnc.DataSources; } }

        public List<ArchiveJob> Jobs { get { return _rnc.ArchiveJobs; } }

        public bool StartAtStart { get { return _sas; } set { _sas = value; } }

        /// <summary>
        /// Сохраняет ВСЕ настройки программы
        /// </summary>
        public void Save()
        {
            _rnc.SaveDataSources();
            _rnc.SaveArchiveJobs();
            _rnc.SaveOption(RabnetConfig.RNCOption.MysqlPath, _myPath);
            _rnc.SaveOption(RabnetConfig.RNCOption.zip7path, _p7);
            _rnc.SaveOption(RabnetConfig.RNCOption.serverUrl, _servUrl);
            _rnc.SaveOption(RabnetConfig.RNCOption.RabDump_StartupAtStart, _sas.ToString());
        }

        /// <summary>
        /// Загружает все настройки программы
        /// </summary>
        public void Load()
        {
            _rnc = new RabnetConfig();
            _rnc.LoadDataSources();
            _rnc.LoadArchiveJobs();
            MySqlPath = _rnc.GetOption(RabnetConfig.RNCOption.MysqlPath);
            _p7 = _rnc.GetOption(RabnetConfig.RNCOption.zip7path);
            _servUrl = _rnc.GetOption(RabnetConfig.RNCOption.serverUrl);
            bool.TryParse(_rnc.GetOption(RabnetConfig.RNCOption.RabDump_StartupAtStart),out _sas);
        }

        internal RabnetConfig GetRabnetConfig()
        {
            return _rnc;
        }
    }
}
