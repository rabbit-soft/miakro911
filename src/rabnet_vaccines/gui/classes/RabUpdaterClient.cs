#if !DEMO 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using log4net;
using X_Tools;

namespace rabnet
{
    class RabUpdaterClient
    {
        public static readonly ILog _log = LogManager.GetLogger(typeof(RabUpdaterClient));

        private static RabUpdaterClient _updater;
        private string _ip;
        private TcpClient _tcpClient;
        private Thread GetUpdateT;

        public delegate void progressUpCB(int p);
        public delegate void progressDoneCB(bool res);

        public progressUpCB progressUp;
//        public progressDoneCB progressDone;

        private int ThrRes = -1;

        private string UpFilePath = "";

        public const int UpErrStillWorking              = -1;
        public const int UpErrFinishedOK                = 0;
        public const int UpErrFinishedNoHandshake       = 10;
        public const int UpErrFinishedNoGetFile         = 11;
        public const int UpErrFinishedFileTransferFail  = 12;
        public const int UpErrFinishedSocketFail        = 5;
        public const int UpErrBadMD5OnServer            = 15;
        public const int UpErrBadMD5Local               = 16;

        public static RabUpdaterClient Get()
        {
            if (_updater == null)
            {
                _updater=new RabUpdaterClient();
            }
            return _updater;
        }

        public void SetIP(string ip)
        {
            _ip = ip;
        }

        private string ReadLine(Stream stream)
        {
            int bytesRead;
            byte[] message = new byte[1];

            string cmd = "";

            ASCIIEncoding encoder = new ASCIIEncoding();

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = stream.Read(message, 0, 1);
                }
                catch
                {
                    //a socket error has occured
                    return "";
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    return "";
                }

                //message has successfully been received
                //                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead) + " -> " + System.AppDomain.GetCurrentThreadId().ToString());
                cmd += encoder.GetString(message, 0, bytesRead);
                cmd = cmd.Replace("\r", "");
                if (cmd.Length >= 1)
                {
                    if (cmd[cmd.Length - 1] == (char)10)
                    {
                        cmd = cmd.Replace("\n", "");
                        return cmd;
                    }
                }
            }

        }

        private bool ReadFile(Stream stream,long size, string name)
        {
            int bytesRead;
            long bytesReadTotal=0;
            byte[] message = new byte[4096];

            ASCIIEncoding encoder = new ASCIIEncoding();

            FileStream fl;

            try
            {
                fl = new FileStream(name, FileMode.Create);
            }
            catch
            {
                return false;
            }

            while (true)
            {
                bytesRead = 0;
                SetProgr((double)bytesReadTotal / (double)size * 100);

                //System.Diagnostics.Debug.WriteLine(.ToString());
                stream.ReadTimeout = 15000;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = stream.Read(message, 0, 4096);
                }
                catch
                {
                    fl.Close();
                    //a socket error has occured
                    return false;
                }

                if (bytesRead == 0)
                {
                    fl.Close();
                    //the client has disconnected from the server
                    return false;
                }

                bytesReadTotal += bytesRead;
                if (bytesReadTotal >= 6000000)
                {
                    Debug.WriteLine("Successfully connected to server\r\n");
                }

                try
                {
                    fl.Write(message, 0, bytesRead);
                }
                catch
                {
                    fl.Close();
                    return false;
                }

                if (bytesReadTotal == size)
                {
                    fl.Close();
                    return true;
                }


                //message has successfully been received
                //                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead) + " -> " + System.AppDomain.GetCurrentThreadId().ToString());
//                cmd += encoder.GetString(message, 0, bytesRead);
//                cmd = cmd.Replace("\n", "");
//                if (cmd.Length >= 1)
//                {
//                    if (cmd[cmd.Length - 1] == (char)13)
//                    {
//                        cmd = cmd.Replace("\r", "");
//                        return tre;
//                    }
//                }
            }

        }

        public bool CheckUpdate()
        {
            // Create a new instance of a TCP client
            Stream strRemote;
            ASCIIEncoding encoder = new ASCIIEncoding();
            _tcpClient = new TcpClient();
            try
            {
                // Connect the TCP client to the specified IP and port
                _tcpClient.Connect(_ip, 3000);
                System.Diagnostics.Debug.WriteLine("Successfully connected to server\r\n");

                strRemote = _tcpClient.GetStream();

                string msg="hello"+Environment.NewLine;
                string res="";
                System.Diagnostics.Debug.WriteLine(msg);

                strRemote.Write(encoder.GetBytes(msg), 0, encoder.GetByteCount(msg));


                res=ReadLine(strRemote);

                Debug.WriteLine(res + "\r\n");
                if (res.Substring(0, 3) == "Ok#")
                {
                    Debug.WriteLine("Successfully handshake\r\n");
                    msg = "updateInfo" + Environment.NewLine;

                    Debug.WriteLine(msg);
                    strRemote.Write(encoder.GetBytes(msg), 0, encoder.GetByteCount(msg));
                    res = ReadLine(strRemote);
                    Debug.WriteLine(res+"\r\n");
                    if (res.Substring(0, 3) == "Ok#")
                    {
                        if (res.Substring(4, 8) == "Version:")
                        {
                            Debug.WriteLine(res + "---<\r\n");
                            Version ver = new Version(res.Substring(12));
                            Debug.WriteLine(ver.ToString() + "---<\r\n");

                            Version myVer = new Version(Application.ProductVersion);

                            if (myVer < ver)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exMessage)
            {
                // Display any possible error
                Debug.WriteLine(exMessage.Message);
                _log.Warn("Failed to get update info: " + exMessage.Message);
            }
            
            _tcpClient.Close();

            return false;

        }

        private void SetProgr(double progr)
        {
            if (progressUp != null)
            {
                 progressUp((int)Math.Round(progr));
            }
        }

        private void GetUpdateThr()
        {

            SetProgr(0);

            Stream strRemote;
            ASCIIEncoding encoder = new ASCIIEncoding();
            _tcpClient = new TcpClient();
            _tcpClient.ReceiveTimeout = 5000;
            _tcpClient.SendTimeout = 5000;
            try
            {
                // Connect the TCP client to the specified IP and port
                _tcpClient.Connect(_ip, 3000);
                Debug.WriteLine("Successfully connected to server\r\n");

                strRemote = _tcpClient.GetStream();

                string msg = "hello" + Environment.NewLine;
                string res = "";

                strRemote.Write(encoder.GetBytes(msg), 0, encoder.GetByteCount(msg));


                res = ReadLine(strRemote);

                Debug.WriteLine(res + "\r\n");
                if (res.Substring(0, 3) == "Ok#")
                {
                    Debug.WriteLine("Successfully handshake\r\n");
                    msg = "updateFile" + Environment.NewLine;

                    strRemote.Write(encoder.GetBytes(msg), 0, encoder.GetByteCount(msg));
                    res = ReadLine(strRemote);
                    Debug.WriteLine(res + "\r\n");
                    if (res.Substring(0, 3) == "Ok#")
                    {

                        string fi = res.Substring(4);

                        string[] fie = fi.Split('/');
                        Debug.WriteLine(fie[0] + "---<\r\n");
                        Debug.WriteLine(fie[1] + "---<\r\n");
                        Debug.WriteLine(fie[2] + "---<\r\n");

                        long sz = Convert.ToInt32(fie[1]);
                        string uMD5 = fie[2];


                        string dir = Path.GetDirectoryName(Application.ExecutablePath) + "\\upd\\";

                        Directory.CreateDirectory(dir);

                        UpFilePath = dir + fie[0];

                        if (ReadFile(strRemote, sz, UpFilePath))
                        {
                            if (XTools.VerifyMD5(UpFilePath, uMD5))
                            {
                                ThrRes = UpErrFinishedOK;
                            }
                            else
                            {
                                ThrRes = UpErrBadMD5Local;
                            }
                        }
                        else
                        {
                            ThrRes = UpErrFinishedFileTransferFail;
                        }

                        return;

                        //                        if (res.Substring(4, 8) == "Version:")
                        //                        {
                        //                            System.Diagnostics.Debug.WriteLine(res + "---<\r\n");
                        //                            Version ver = new Version(res.Substring(12));
                        //                            System.Diagnostics.Debug.WriteLine(ver.ToString() + "---<\r\n");
                        //
                        //                            Version myVer = new Version(Application.ProductVersion);
                        //
                        //                            if (myVer < ver)
                        //                            {
                        //                                return true;
                        //                            }
                        //                            else
                        //                            {
                        //                                return false;
                        //                            }
                        //}
                    }
                    else
                    {
                        if (res.Substring(0, 4) == "Err#")
                        {
                            uint errN = Convert.ToUInt32(res.Substring(4, 4));
                            if (errN == 2)
                            {
                                ThrRes = UpErrBadMD5OnServer;
                                return;
                            }
                        }
                        ThrRes = UpErrFinishedNoGetFile;
                        return;
                    }
                }
                else
                {
                    ThrRes = UpErrFinishedNoHandshake;
                    return;
                }




            }
            catch (Exception exMessage)
            {
                // Display any possible error
                System.Diagnostics.Debug.WriteLine(exMessage.Message);
                _log.Error("Failed to get update file: "+exMessage.Message);
            }

            ThrRes = UpErrFinishedSocketFail;

            _tcpClient.Close();
        }

        public void GetUpdate()
        {
            ThrRes = UpErrStillWorking;
            this.GetUpdateT = new Thread(new ThreadStart(GetUpdateThr));
            GetUpdateT.Start();
        }

        public int GetUpRes()
        {
            return ThrRes;
        }

        public string GetUpFilePath()
        {
            return UpFilePath;
        }

    }
}

#endif