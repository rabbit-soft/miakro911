﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace pEngine
{
    partial class Options
    {
        private const string DEF_SERVER = "localhost";
        private const string SERVERS = "servers";
        //private const string DEF_SERV = "defserv";

        private List<sServer> _servers = new List<sServer>();
        private int _defserv = -1;

        public string ServersGetDefault()
        {
            string result = "";
            if (_servers.Count == 0)
                result = DEF_SERVER;
            else
            {
                if (_defserv != -1)
                    result = _servers[_defserv].Address;
                else
                {
                    result = GetStringOption(optType.DefaultServer);
                    if (result == "")
                    {
                        _defserv = 0;
                        return _servers[_defserv].Address;
                    }
                    else
                    {
                        foreach (sServer cand in _servers)
                            if (cand.Name == result)
                            {
                                _defserv = _servers.IndexOf(cand);
                                result = _servers[_defserv].Address;
                                break;
                            }
                    }
                }
            }
            return  prettyServer(result);
        }

        public void ServersAdd(sServer serv,bool def)
        {
            protectSName(serv);
            _servers.Add(serv);
            if (def)
                _defserv = _servers.Count - 1;
        }
        public void ServersAdd(sServer serv)
        {
            ServersAdd(serv, false);
        }

        public string ServersGetNext()
        {
            if (_servers.Count == 0) return DEF_SERVER;

            _defserv = (_defserv + 1) % _servers.Count;
            SetOption(optType.DefaultServer, _servers[_defserv].Name);
            return prettyServer(_servers[_defserv].Address);
        }

        public int ServersCount
        {
            get{return _servers.Count;}
        }

        /// <summary>
        /// Cтирает старые серверы и записывает в реестр новые
        /// </summary>
        /// <param name="list"></param>
        public void ServersSave(sServer[] list)
        {
            try { _globReg.DeleteSubKey(SERVERS); }
            catch { }; //tсли не существует такого
            RegistryKey rk = _globReg.CreateSubKey(SERVERS);
            string def = _servers[_defserv].Address;
            _servers.Clear();
            foreach (sServer s in list)
            {
                _servers.Add(s);
                rk.SetValue(s.Name, s.Address, RegistryValueKind.String);
                if (def == s.Address)
                    _defserv = _servers.Count - 1;
            }
            if (_defserv == -1 || _defserv >= _servers.Count)
                _defserv = 0;
            SetOption(optType.DefaultServer, _servers[_defserv].Name);
        }

        /// <summary>
        /// Записывает в реестр серверы из памяти
        /// </summary>
        public void ServersSave()
        {
            try { _globReg.DeleteSubKey(SERVERS); }
            catch { };
            RegistryKey rk = _globReg.CreateSubKey(SERVERS);
            foreach (sServer s in _servers)
            {
                rk.SetValue(s.Name, s.Address, RegistryValueKind.String);
            }
        }

        public string[] ServersGetNames()
        {
            string[] result = new string[_servers.Count];
            int i =0;
            foreach (sServer s in _servers)
                result[i++] = s.Name;
            return result;
        }

        private void serversLoad()
        {
            string def = (string)_userReg.GetValue(optType.DefaultServer.ToString(), "");

            RegistryKey rk = _globReg.CreateSubKey(SERVERS);
            foreach (string nm in rk.GetValueNames())
            {
                _servers.Add(new sServer(nm, (string)rk.GetValue(nm, "")));
                if (nm == def)
                    _defserv = _servers.Count - 1;
            }
        }

        private string prettyServer(string result)
        {
            if (!result.StartsWith("http://")) result = "http://" + result;
            result = result.Replace('\\','/');
            if (!result.EndsWith("/")) result += "/";
            return result;
        }

        private void protectSName(sServer serv)
        {
            foreach (sServer cand in _servers)
                if (serv.Name == cand.Name)
                {
                    if (cand.Name.EndsWith(")") && cand.Name.Contains("("))
                    {
                        string num = cand.Name.Substring(cand.Name.LastIndexOf("("), cand.Name.Length - 2);
                        if (Helper.isInteger(num))
                        {
                            serv.Name = serv.Name.Remove(cand.Name.LastIndexOf("("))+"("+(int.Parse(num)+1).ToString()+")";
                        }
                        else 
                            serv.Name += "(1)";
                    }
                    else
                        serv.Name += "(1)";
                    break;
                }
            
        }

        /*private void saveDefServ()
        {
            if (_defserv != -1)
                _userReg.SetValue(DEF_SERV, _servers[_defserv].Name, RegistryValueKind.String);
        }*/
    }
}
