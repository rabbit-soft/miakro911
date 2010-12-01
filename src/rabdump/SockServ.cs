using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using X_Classes;


namespace rabdump
{
    class SocketServer
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public SocketServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        public void Close()
        {
            tcpListener.Stop();
            listenThread.Abort();
        }
        
        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();

                while (true)
                {
                    //blocks until a client has connected to the server
                    try
                    {
                        TcpClient client = this.tcpListener.AcceptTcpClient();

                        //create a thread to handle communication 
                        //with connected client
                        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
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
        
        private void HandleClientComm(object client)
        {

            RabUpdateInfo nfo=RabUpdater.ReadUpdateInfo(Path.GetDirectoryName(Application.ExecutablePath)+"\\updates\\updates.xml");
            
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] message = new byte[4096];
            int bytesRead;

            string cmd = "";
            string resp = "";

            bool loggedin = false;

            try
            {

                while (true)
                {
                    bytesRead = 0;

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




                                    try
                                    {
                                        FileStream loc = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + nfo.file_name, FileMode.Open, FileAccess.Read);

                                        resp = "Ok# " + nfo.file_name + "/" + loc.Length + Environment.NewLine;
                                        clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);


                                        int Length = 2048;
                                        Byte[] buffer = new Byte[Length];
                                        int bR = loc.Read(buffer, 0, Length);

                                        // write the required bytes

                                        while (bR > 0)
                                        {
                                            clientStream.Write(buffer, 0, bR);
                                            //                                            bR = 0;
                                            bR = loc.Read(buffer, 0, Length);
                                            //resp = "--------------------------->"+loc.Position.ToString();
                                            //clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                        }
                                        loc.Close();


                                    }
                                    catch
                                    {
                                        resp = "Err# File operation failed";
                                        try
                                        {
                                            clientStream.Write(encoder.GetBytes(resp), 0, resp.Length);
                                        }
                                        catch
                                        {
                                        }

                                    }


                                }
                                break;
                            default:
                                resp = "Err# Unknown command \"" + cmd + "\"" + Environment.NewLine;
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
