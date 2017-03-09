#if DEBUG
#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace updater
{
    public delegate void EndUpdateEventHandler();
    public delegate void ProgressEventHandler(string Name, int PreVer, int ver);
    public delegate void ErrorEventHandler(string Name, Exception exc);

    class UpdateThread
    {
        public ErrorEventHandler Error;
        public ProgressEventHandler Progress;
        public EndUpdateEventHandler EndUpdate;

        private List<UpRow> _uprows;
        private int _curver;
        private Dictionary<int, string> _scripts;

        public UpdateThread(List<UpRow> uprows, int curver, Dictionary<int, string> scripts)
        {
            _uprows = uprows;
            _curver = curver;
            _scripts = scripts;
        }

        internal void StartUpdate()
        {
            Thread t = new Thread(update);
            t.Start();
        }

        private void update()
        {
            foreach (UpRow ur in _uprows) {
                MySqlConnection _sql = _sql = new MySqlConnection(ur.ConnString + ";Allow User Variables=True");
                _sql.Open();
#if !NOCATCH
                try {
#endif
                    while (ur.PreVer < _curver) {
                        ur.PreVer++;
                        foreach (int k in _scripts.Keys)
                            if (k == ur.PreVer) {
                                if (Progress != null)
                                    Progress(ur.Name, ur.PreVer - 1, ur.PreVer);

                                MySqlCommand c = new MySqlCommand("", _sql);
                                c.CommandTimeout = 1200;
                                String[] cmds = _scripts[k].Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
                                c.CommandText = cmds[0];
                                c.ExecuteNonQuery();
                                if (cmds.Length > 1) {
                                    MySqlScript sc = new MySqlScript(_sql, cmds[1]);
                                    sc.Delimiter = "|";
                                    sc.Execute();
                                }
                            }
                    }
#if !NOCATCH
                } catch (Exception ex) {
                    if (Error != null)
                        Error(ur.Name, ex);
                } finally {
                    _sql.Close();
                }
#endif
            }
            if (EndUpdate != null)
                EndUpdate();

        }
    }

    class UpRow
    {
        public int PreVer = 0;
        public readonly string Name = "";
        public readonly string ConnString = "";

        public UpRow(int pv, string name, string cs)
        {
            PreVer = pv;
            Name = name;
            ConnString = cs;
        }
    }
}
