using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace pEngine
{
    public enum optType { UserID,DefaultUser,ColumnWidth, SpliterPosition }

    public interface IOptions
    {
        int GetIntOption(optType opt);
        string GetStringOption(optType opt);
        bool GetBoolOption(optType opt);

        void SetOption(optType opt,string value);
        void SetOption(optType opt, int value);
        void SetOption(optType opt, bool value);
        /// <summary>
        /// Получает размер колонок для treeView
        /// </summary>
        /// <param name="panel">Панель отображения данных (наследник BasePanel)</param>
        /// <param name="treeviewindex">Индекс treeView</param>
        /// <param name="comaseparateWidths">размеры колонок разделенные через запятую</param>
        void SetTVColumnsWidth(string panelname, string treeviewname, string comaseparateWidths);
        string GetTVColumnsWidth(string panelname, string treeviewname);

        //string GetDefServer();
        //string GetNextServer();
        //void AddServer(string server);
    }


    partial class Options:IOptions
    {
        private const string REG_PATH = @"Software\9-Bits\grdUpdate";

        private RegistryKey _userReg = Registry.CurrentUser.CreateSubKey(REG_PATH);
        private RegistryKey _loclReg = Registry.LocalMachine.CreateSubKey(REG_PATH);
        private User _curUser = null;

        public Options(User user)
        {
            BindUser(user);
        }
        public Options() { }

        public void BindUser(User user)
        {
            _curUser = user;
            _userReg = _userReg.CreateSubKey(_curUser.Name);
            this.SetOption(optType.DefaultUser, user.Name);
            loadUserOptions();
        }

        private void loadUserOptions()
        {
            serversLoad();
        }
      
        private string getDefOptVal(optType opt)
        {
            switch (opt)
            {
                default: return "";
            }
        }
      
        #region IOptions Members

        public int GetIntOption(optType opt)
        {
            //string val = GetStringOption(opt);
            //if (Helper.isInteger(val))
            //    return int.Parse(val);
            //else return 0;
            try
            {
                return (int)_userReg.GetValue(opt.ToString(), 0);
            }
            catch
            {
                return 0;
            }
        }
        public bool GetBoolOption(optType opt)
        {
            string val = GetStringOption(opt);
            return (val != "0" || val.ToLower() == "true");
        }
        public string GetStringOption(optType opt)
        {
            if (local(opt))
                return (string)_loclReg.GetValue(opt.ToString(), getDefOptVal(opt));
            else if(_curUser!=null)
                return (string)_userReg.GetValue(opt.ToString(), getDefOptVal(opt));

            throw new Exception("Пользователь не установлен");
        }

        public void SetOption(optType opt, string value) { setOptions(opt,value,RegistryValueKind.String);}
        public void SetOption(optType opt, int value) { setOptions(opt, value, RegistryValueKind.DWord); }
        public void SetOption(optType opt, bool value) { setOptions(opt,value?"1":"0",RegistryValueKind.String); }

        public void SetTVColumnsWidth(string panelname, string treeviewname, string comaseparateWidths)
        {
            RegistryKey rk = _userReg.CreateSubKey("Columns");
            //TODO замена названия классов на рандомные слова
            string key = panelname + "_" + treeviewname;
            rk.SetValue(key, comaseparateWidths, RegistryValueKind.String);
        }

        public string GetTVColumnsWidth(string panelname, string treeviewname)
        {
            RegistryKey rk = _userReg.CreateSubKey("Columns");
            //TODO замена названия классов на рандомные слова
            string key = panelname + "_" + treeviewname;
            return (string)rk.GetValue(key,"");
        }

#endregion 

        private void setOptions(optType opt, object o, RegistryValueKind rvk)
        {
            if (local(opt))
                _loclReg.SetValue(opt.ToString(), o, rvk);
            else if (_curUser != null) 
                _userReg.SetValue(opt.ToString(), o, rvk);//todo проверить установку опций

        }

        private bool local(optType opt)
        {
            return opt == optType.DefaultUser /*&& another optTypes*/;
        }


    }
}
