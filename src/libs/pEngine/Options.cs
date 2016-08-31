using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace pEngine
{
    public enum optType 
    { 
        DefaultUser,
        DefaultServer,
        ColumnWidth, 
        SpliterPosition,
        WindowState,
        /// <summary>
        /// x;y
        /// </summary>
        WindowPosition,
        /// <summary>
        /// width;height
        /// </summary>
        WindowSize
    }

    public partial class Options
    {
        private string REG_PATH
        {
            get{ return @"Software\9-Bits\"+AppDomain.CurrentDomain.FriendlyName;}
        }

        private RegistryKey _userReg;
        private RegistryKey _globReg;// = Registry.CurrentUser.CreateSubKey(REG_PATH);
        private User _curUser = null;

        public Options(User user):this()
        {
            BindUser(user);
        }
        public Options() 
        {
            _globReg = Registry.CurrentUser.CreateSubKey(REG_PATH);
        }

        internal void BindUser(User user)
        {
            _curUser = user; 
            _userReg = Registry.CurrentUser.CreateSubKey(REG_PATH + "\\users").CreateSubKey(_curUser.Name);
            //this.SetOption(optType.DefaultUser, user.Name);
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
                case optType.WindowState:
                    return "2";
                case optType.WindowPosition:
                case optType.WindowSize:
                    return "0;0";
                default: return "";
            }
        }

        public int GetIntOption(optType opt)
        {
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
            if (isGlob(opt))
                return (string)_globReg.GetValue(opt.ToString(), getDefOptVal(opt));
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

        public void SetFormOpt(optType type, string form, string param)
        {
            RegistryKey rk = _userReg.CreateSubKey("Windows");
            rk.SetValue(form+"_" + type.ToString(), param);
        }

        public string GetFormOpt(optType type, string form)
        {
            RegistryKey rk = _userReg.CreateSubKey("Windows");
            return (string)rk.GetValue(form + "_" + type.ToString(), getDefOptVal(type));
        }

        private void setOptions(optType opt, object val, RegistryValueKind rvk)
        {
            if (isGlob(opt))
                _globReg.SetValue(opt.ToString(), val, rvk);
            else if (_curUser != null)
                _userReg.SetValue(opt.ToString(), val, rvk);//todo проверить установку опций           
        }

        private bool isGlob(optType opt)
        {
            switch (opt)
            {
                case optType.DefaultUser: return true;
                default: return false;
            }
        }


    }
}
