using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.IO;
using X_Classes;


namespace rabdump
{
    class SocketServer
    {
        private readonly TcpListener _tcpListener;
        private readonly Thread _listenThread;

        public SocketServer()
        {
            _tcpListener = new TcpListener(IPAddress.Any, 3000);
            _listenThread = new Thread(ListenForClients);
            _listenThread.Start();
        }

        public void Close()
        {
            _tcpListener.Stop();
            _listenThread.Abort();
        }
        
        private void ListenForClients()
        {
            try
            {
                _tcpListener.Start();

                while (true)
                {
                    //blocks until a client has connected to the server
                    try
                    {
                        TcpClient client = _tcpListener.AcceptTcpClient();

                        //create a thread to handle communication 
                        //with connected client
                        Thread clientThread = new Thread(HandleClientComm);
                        clientThread.IsBackground = true;
                        clientThread.Start(client);
                    }
                    catch //(Exception e)
                    {
                        //                    MessageBox.Show(e.Message + e.StackTrace);
                    }
                }
            }
            catch
            {
            }
        }
        
        private static void HandleClientComm(object client)
        {

            RabUpdateInfo nfo=RabUpdater.ReadUpdateInfo(Path.GetDirectoryName(Application.ExecutablePath)+"\\updates\\updates.xml");
            
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] message = new byte[4096];

            string cmd = "";
            string resp = "";

            bool loggedin = false;

            try
            {

                while (true)
                {
                    int bytesRead = 0;

                    try
                    {
                        //blocks until a client sends a message
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        //a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        break;
                    }

                    //message has successfully been received
                    //                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead) + " -> " + System.AppDomain.GetCurrentThreadId().ToString());
                    cmd += encoder.GetString(message, 0, bytesRead);
                    cmd = cmd.Replace("\r", "");
                    if (cmd[cmd.Length - 1] == (char)10)
                    {
                        cmd = cmd.Replace("\n", "");
                        //                        System.Diagnostics.Debug.WriteLine(cmd + " -> " + System.AppDomain.GetCurrentThreadId().ToString());

                        switch (cmd)
                        {
                            case "hello":
                                resp = "Ok# Hello there!!" + Environment.NewLine;
                                clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                loggedin = true;
                                break;
                            case "updateInfo":
                                if (loggedin)
                                {
                                    //FileVersionInfo myFI = FileVersionInfo.GetVersionInfo(Path.GetDirectoryName(Application.ExecutablePath)+"\\updates\\setup.exe");
                                    resp = "Ok# Version:" + Application.ProductVersion.ToString() + Environment.NewLine;
                                    clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                }
                                break;
                            case "updateFile":
                                if (loggedin)
                                {
                                    //FileVersionInfo myFI = FileVersionInfo.GetVersionInfo(Path.GetDirectoryName(Application.ExecutablePath)+"\\updates\\setup.exe");

                                    if (X_Tools.XTools.VerifyMD5(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + nfo.FileName, nfo.FileMD5))
                                    {

                                        try
                                        {
                                            FileStream loc = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + nfo.FileName, FileMode.Open, FileAccess.Read);

                                            resp = "Ok# " + nfo.FileName + "/" + loc.Length + "/" + nfo.FileMD5 + Environment.NewLine;
                                            clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);


                                            const int length = 2048;
                                            Byte[] buffer = new Byte[length];
                                            int bR = loc.Read(buffer, 0, length);

                                            // write the required bytes

                                            while (bR > 0)
                                            {
                                                clientStream.Write(buffer, 0, bR);
                                                //                                            bR = 0;
                                                bR = loc.Read(buffer, 0, length);
                                                //resp = "--------------------------->"+loc.Position.ToString();
                                                //clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                            }
                                            loc.Close();


                                        }
                                        catch
                                        {
                                            resp = "Err#0001 File operation failed"+Environment.NewLine;
                                            try
                                            {
                                                clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                            }
                                            catch
                                            {
                                            }

                                        }
                                    }
                                    else
                                    {
                                        resp = "Err#0002 Update file has bad MD5"+Environment.NewLine;
                                        try
                                        {
                                            clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                        }
                                        catch
                                        {
                                        }
                                        break;
                                    }

                                }
                                break;
                            default:
                                resp = "Err#0010 Unknown command \"" + cmd + "\"" + Environment.NewLine;
                                clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                //                                cmd = cmd + Environment.NewLine;
                                //                                clientStream.Write(encoder.GetBytes(cmd), 0, cmd.Length);
                                break;
                        }

                        if (!loggedin)
                        {
                            break;
                        }
                        cmd = "";
                    }


                }
            }
            catch
            {
            }

            tcpClient.Close();
        }
    }
}
