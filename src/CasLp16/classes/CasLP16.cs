#if DEBUG
#define NOCATCH
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using gamlib;

namespace CAS
{
    public sealed partial class CasLP16
    {
        /// <summary>
        /// Сохранить все товары
        /// </summary>
        public void SavePLUs()
        {
            foreach (PLU plu in _pluList)
            {
                if (plu.Delete)
                    deletePLU(plu);
                else savePLU(plu);
            }
        }

        /// <summary>
        /// Сохранить все сообщения
        /// </summary>
        public void SaveMSGs()
        {
            foreach (MSG msg in _msgList)
            {
                if (msg.Delete)
                    deleteMSG(msg);
                else saveMSG(msg);
            }
        }
    }

    //Реализация самого класса
    public sealed partial class CasLP16
    {
        private static CasLP16 _instance = null;
        /// <summary>
        /// Должен быть создан лишь один класс CasLP16
        /// </summary>
        public static CasLP16 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CasLP16();
                return _instance;
            }
        }

        private int _id = 1;
        private string _host = "192.168.0.5";
        private int _port = 8111;
        private int _loadProgress = -1;
        private bool _loading = false;
        private Thread _timer = new Thread(timerStart);
        private NetworkStream _nwStream;
        private TcpClient _tcpClient;
        //private byte[] _bytesForRead;
        private readonly PLUList _pluList = new PLUList();
        private readonly MSGList _msgList = new MSGList();

        public int ScaleNumber
        {
            get { return _id; }
            set
            {
                if (value < 0 || value > 99) return;
                _id = value;
            }
        }
        public string Host
        {
            get { return _host; }
            set { SetScaleAddress(value, _port); }
        }
        public int Port
        {
            get { return _port; }
            set { SetScaleAddress(_host, value); }
        }
        public int LoadProgress
        {
            get { return _loadProgress; }
        }
        /// <summary>
        /// Идет ли загрузка данных из весов.
        /// </summary>
        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; }
        }
        public int PLUCount
        {
            get { return _pluList.Count; }
        }
        public int MSGCount
        {
            get { return _msgList.Count; }
        }
        /// <summary>
        /// Получить текущее состояние весов
        /// </summary>
        public State GetState()
        {
            byte[] cmd = new byte[1] { Info.Commands.STATE_GET };
            byte[] state = new byte[Info.Sizes.STATE_LENGTH];
            execCommand(cmd, ref state, 1);
            return new State(state);
        }

        public FactoryConfig GetFactoryConfig()
        {
            byte[] cmd = new byte[1] { Info.Commands.FACTORY_CONF_GET };
            byte[] state = new byte[Info.Sizes.FACTORY_CONF_LENGTH];
            execCommand(cmd, ref state, 1);
            return new FactoryConfig(state);
        }

        public Summary GetSummary()
        {
            byte[] cmd = new byte[1] { Info.Commands.SUMMARY_GET };
            byte[] state = new byte[Info.Sizes.SUMMARY_LENGTH];
            execCommand(cmd, ref state, 1);
            return new Summary(state);
        }
        /// <summary>
        /// Стирание сумарного итога продаж по всем товарам
        /// </summary>
        public void ClearSumarys()
        {
            byte[] cmd = new byte[1] { Info.Commands.SUMMARY_CLEAR };
            execCommand(cmd);
        }

        //public PLUList PluList{get { return _pluList; }}
        //public MSGList MsgList{get { return _msgList; }}
        public bool Connected
        {
            get 
            {
                if (_tcpClient == null ||_tcpClient.Client ==null)
                    return false;
                else return _tcpClient.Connected;
            }
        }
        public CasLP16() { }

        ~CasLP16()
        {
            this.Disconnect();
        }

        public int[] getIDsOfPLUs()
        {
            return _pluList.getIds();
        }

        public int[] getIDsOfMSGs()
        {
            return _msgList.getIds();
        }



        /// <summary>
        /// Установить адрес весов в локальной сети.
        /// </summary>
        /// <param name="host">Адрес</param>
        /// <param name="port">Порт</param>
        /// <returns>ReturnCode</returns>
        public int SetScaleAddress(string host, int port)
        {
#if !NOCATCH
            try
            {
#endif
            IPAddress.Parse(host);
            _host = host;
            _port = port;
            return ReturnCode.SUCCESS;
#if !NOCATCH
            }
            catch(Exception)
            {
                return ReturnCode.ERROR;
            }
#endif
        }

        
        //private State getScaleState()
        //{
        //    byte[] cmd = new byte[1]{Info.Commands.STATE_GET};
        //    byte[] state = new byte[Info.Sizes.STATE_LENGTH];
        //    execCommand(cmd, ref state, 1);
        //    return new State(state);
        //}

        //private FactoryConfig getFactoryConf()
        //{
        //    byte[] cmd = new byte[1] { Info.Commands.FACTORY_CONF_GET };
        //    byte[] state = new byte[Info.Sizes.FACTORY_CONF_LENGTH];
        //    execCommand(cmd, ref state, 1);
        //    return new FactoryConfig(state);
        //}

        //private Summary getSummary()
        //{
        //    byte[] cmd = new byte[1] { Info.Commands.SUMMARY_GET };
        //    byte[] state = new byte[Info.Sizes.SUMMARY_LENGTH];
        //    execCommand(cmd, ref state, 1);
        //    return new Summary(state);
        //}

        /// <summary>
        /// Нужен для соблюдения интерсала между посылом команд
        /// </summary>
        private void runTimer()
        {
            _timer = new Thread(timerStart);
            _timer.IsBackground = true;
            _timer.Start();
        }

        private static void timerStart()
        {
            Thread.Sleep(220);//положено 200
        }

        private void connectWaiter(IAsyncResult res)
        {
            TcpClient clnt = (TcpClient)res.AsyncState;
            if (clnt.Client != null)
            {
                try
                {
                    clnt.EndConnect(res);
                    _nwStream = clnt.GetStream();
                    _nwStream.ReadTimeout = 10000;
                }
                catch(SocketException)
                {
                    this.Disconnect();
                }
            }
        }

        private int beginSession()
        {
#if !NOCATCH
            try
            {
#endif
                while (_timer.IsAlive)
                    Thread.Sleep(10);
                if (_tcpClient == null || !_tcpClient.Connected) return ReturnCode.CONNECTION_NOT_SET;
                _nwStream.WriteByte((byte)_id);
                byte[] ans = new byte[2];
                try
                {
                    _nwStream.Read(ans, 0, ans.Length);
                }
                catch (IOException)
                {
                    return ReturnCode.READ_TIMEOUT;
                }
                runTimer();
                if (ans[1] == Info.Answers.ERROR)
                    return ReturnCode.SCALE_ERROR;
                else return ReturnCode.SUCCESS;
#if !NOCATCH
            }
            catch (IOException)
            {
                return ReturnCode.CONNECTION_FAIL;
            }
#endif
        }

        /// <summary>
        /// Выполняет команду, на которую весы не возвращают данные
        /// </summary>
        private int execCommand(byte[] bts)
        {
            byte[] doodle = new byte[1];
            return execCommand(bts, ref doodle, 0);
        }
        private int execCommand(byte[] cmd, ref byte[] answer, int offset)
        {
            return execCommand(cmd,ref answer,offset,true);
        }
        /// <summary>
        /// Выполняет команду весов
        /// </summary>
        /// <param name="bts">Массив байтов соманды.</param>
        /// <param name="answer">Информация, полученнаяф от весов</param>
        /// <param name="offset">Смещение. С какого байта начинаются данные.</param>
        /// <param name="byByte">Summary,FactoryConfig,State  не принимают данные если читать побайтно.</param>
        /// <returns>Код состояния</returns>
        private int execCommand(byte[] cmd, ref byte[] answer, int offset,bool byByte)
        {
#if !NOCATCH
            try
            {
#endif
                int result = beginSession();
                if (result != ReturnCode.SUCCESS) return result;
                _nwStream.Write(cmd, 0, cmd.Length);
                byte[] ans = new byte[answer.Length + offset];
                try
                {
                    if (byByte)
                    {
                        int a = 0;
                        int i = 0;
                        while (i < ans.Length && a != -1)//альтернатива Read, т.к он не читал полностью месседж
                        {
                            a = _nwStream.ReadByte();
                            if (a != -1)
                                ans[i] = (byte)a;
                            if (i == 0 && a == Info.Answers.ERROR) break;
                            i++;
                        }
                    }
                    else
                    {
                        _nwStream.Read(ans, 0, ans.Length);
                    }
                }
                catch (IOException)
                {
                    return ReturnCode.READ_TIMEOUT;
                }
                //_nwStream.Read(ans, 0,401);
                //System.Diagnostics.Debug.WriteLine("Scale ans:"+ans[0],"execComand");
                if (ans[0] == Info.Answers.ERROR)
                    return ReturnCode.SCALE_ERROR;
                else //if (ans[0] == Info.Ansvers.SUCCESS)
                {
                    Array.Copy(ans, offset, answer, 0, answer.Length);
                    return ReturnCode.SUCCESS;
                }
#if !NOCATCH
            }
            catch (IOException)
            {
                return ReturnCode.CONNECTION_FAIL;
            }
#endif
        }

        private int getPLUbyID(int id, out PLU plu)
        {
            byte[] bid = BitConverter.GetBytes(id);
            byte[] bplu = new byte[Info.Sizes.PLU_LENGTH];
            byte[] cmd = new byte[5];
            cmd[0] = Info.Commands.PLU_GET;
            Array.Copy(bid, 0, cmd, 1, 4);
            int result = execCommand(cmd, ref bplu, 1);
            if (result != ReturnCode.SUCCESS)
            {
                plu = null;
                return result;
            }
            else
            {
                plu = new PLU(bplu);
                return result;
            }
        }

        private int getMSGbyID(int id, out MSG msg)
        {
            msg = null;
            if (id < 1 || id > 1000) return ReturnCode.BAD_PARAMS;
            byte[] bid = BitConverter.GetBytes(id);
            byte[] bmsg = new byte[Info.Sizes.MSG_LENGTH];
            byte[] cmd = new byte[3];
            cmd[0] = Info.Commands.MSG_GET;
            Array.Copy(bid, 0, cmd, 1, 2);
            int result = execCommand(cmd, ref bmsg, 1);          
            if (result != ReturnCode.SUCCESS)                           
                return result;           
            else
            {
                if(!BitHelper.ArrayIsEmpty(bmsg))
                    msg = new MSG(id,bmsg);
                return result;
            }
        }

        private int savePLU(PLU plu)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.PLU_SET];
            cmd[0] = Info.Commands.PLU_SET;
            Array.Copy(plu.Bytes, 0, cmd, 1, plu.Bytes.Length);
            return execCommand(cmd);
        }

        private int saveMSG(MSG msg)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.MSG_SET];
            cmd[0] = Info.Commands.MSG_SET;
            Array.Copy(BitConverter.GetBytes(msg.ID), 0, cmd, 1, 2);
            Array.Copy(msg.Bytes, 0, cmd, 3, 400);
            return execCommand(cmd);
        }

        private int deletePLU(PLU plu)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.PLU_CLEAR];
            cmd[0] = Info.Commands.PLU_CLEAR;
            Array.Copy(BitConverter.GetBytes(plu.ID), 0, cmd, 1, 4);
            return execCommand(cmd);
        }

        private int deleteMSG(MSG msg)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.MSG_CLEAR];
            cmd[0] = Info.Commands.MSG_CLEAR;
            Array.Copy(BitConverter.GetBytes(msg.ID), 0, cmd, 1, 2);
            return execCommand(cmd);
        }

        /// <summary>
        /// Устанавливает соединение с весами
        /// </summary>
        /// <returns>Код ошибки</returns>
        public int Connect()
        {
            try
            {
                IPEndPoint pnt = new IPEndPoint(IPAddress.Parse(_host), _port);
                _tcpClient = new TcpClient();
                //_tcpClient.BeginConnect(pnt.Address, pnt.Port, new AsyncCallback(connectWaiter), _tcpClient);
                _tcpClient.ReceiveTimeout = 6000;
                _tcpClient.Connect(pnt);
                _nwStream = _tcpClient.GetStream();
                return ReturnCode.SUCCESS;
            }
            catch (SocketException)
            {
                return ReturnCode.CONNECTION_FAIL;
            }
            catch (Exception)
            {
                return ReturnCode.ERROR;
            }

        }
        public int Connect(string host, int port)
        {
            SetScaleAddress(host, port);
            return Connect();
        }

        public int Disconnect()
        {
#if !NOCATCH
            try
            {
#endif
            if (_nwStream != null)
            {
                _nwStream.Close();
                _nwStream.Dispose();
            }
            if (_tcpClient != null)
            {
                _tcpClient.Close();
            }
            return ReturnCode.SUCCESS;
#if !NOCATCH
            }
            catch (Exception)
            {
                return ReturnCode.ERROR;
            }
#endif
        }

        public int LoadPLUs() { return LoadPLUs(0, Info.Sizes.PLU_MAX_INDEX); }
        public int LoadPLUs(int from, int until)
        {
            if (from >= until) return ReturnCode.BAD_PARAMS;
            int[]ids = new int[until-from+1];
            for (int i = from; i <= until; i++)
                ids[i - from] = i;
            return LoadPLUs(ids);
            /*_pluList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            _loadProgress = 0;
            _loading = true;
            for (int i = from; i <= until; i++)
            {
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                PLU plu;
                getPLUbyID(i, out plu);
                if (plu != null)
                    _pluList.Add(plu);
                _loadProgress = (until - from) / 100 * (i - from);
#if !NOCATCH
                }
                catch(Exception ex) 
                {
 
                }
#endif
            }
            _loading = false;
            _loadProgress = -1;
            return ReturnCode.SUCCESS;*/
        }
        public int LoadPLUs(params int[] IDs)
        {
            _loading = true;
            _pluList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS)
            {
                _loading = false;
                return result;
            }
            //_loadProgress = 0;
            
            for (int i = 0; i < IDs.Length; i++)
            {
                if (IDs[i] < 0 || IDs[i] > Info.Sizes.PLU_MAX_INDEX) continue;
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                PLU plu;
                getPLUbyID(IDs[i], out plu);
                if (plu != null)
                    _pluList.Add(plu);
                //_loadProgress = ;
#if !NOCATCH
                }
                catch(Exception) 
                {
                }
#endif
            }
            _loading = false;
            //_loadProgress = -1;
            return ReturnCode.SUCCESS;
        }

        /// <summary>
        /// Проверяет есть ли в списке PLU c данным ID.
        /// Если нет, то пытается загрузить с весов.
        /// Если в весах есть - добавлет в список, если нет - возвращает  null
        /// </summary>
        public PLU GetPLUbyID(int id)
        {
            foreach (PLU plu in _pluList)
            {
                if (plu.ID == id)
                    return plu;
            }
            PLU newplu;
            getPLUbyID(id, out newplu);
            if (newplu != null)
                _pluList.Add(newplu);
            return newplu;
        }

        public int CleadPLUSummary(int pluid)
        {
            if (!_pluList.ContainsID(pluid)) return ReturnCode.PLU_IS_NOT_EXISTS;
            byte[] bts = new byte[5];
            bts[0] = Info.Commands.PLU_SUMMARY_CLEAR;
            Array.Copy(BitConverter.GetBytes(pluid), 0, bts, 1, 4);
            return execCommand(bts);
        }

        /// <summary>
        /// Сохраняет в памяти весов LPU с заданным ID
        /// При этом обнуляет общие итоги по продажам!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SavePLUbyID(int id)
        {
            PLU save = _pluList.GetPLU(id);
            if (save == null) return ReturnCode.WRONG_PLU_ID;
            return savePLU(save);
        }

        /// <summary>
        /// Добавляет новый това
        /// </summary>
        /// <param name="newplu">ID нового товара</param>
        /// <returns>ReturnCode</returns>
        public int AddPLU(PLU newplu)
        {
            if (GetPLUbyID(newplu.ID) != null) return ReturnCode.PLU_ID_ALREADY_EXISTS;
            _pluList.Add(newplu);
            return savePLU(newplu);
        }

        public int DeletePLUbyID(int id)
        {
            PLU plu = GetPLUbyID(id);
            if ( plu == null) return ReturnCode.PLU_IS_NOT_EXISTS;

            _pluList.Remove(plu);
            return deletePLU(plu);         
        }

        public int LoadMSGs() { return LoadMSGs(1, Info.Sizes.MSG_MAX_INDEX); }
        public int LoadMSGs(int from, int until)
        {
            if (from >= until) return ReturnCode.BAD_PARAMS;
            int[] ids = new int[until - from + 1];
            for (int i = from; i <= until; i++)
                ids[i - from] = i;
            return LoadMSGs(ids);
            /*_msgList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            _loadProgress = 0;
            _loading = true;
            for (int i = from; i <= until; i++)
            {
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                MSG msg;
                getMSGbyID(i, out msg);
                if (msg != null && msg.Text !="")
                    _msgList.Add(msg);
                _loadProgress = (until - from) / 100 * (i - from);
#if !NOCATCH
                }
                catch(Exception ex) 
                {
 
                }
#endif
            }
            _loading = false;
            _loadProgress = -1;
            return ReturnCode.SUCCESS;*/
        }

        public int LoadMSGs(params int[] IDs)
        {
            _msgList.Clear();
            _loading = true;
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            //_loadProgress = 0;         
            for (int i = 0; i < IDs.Length; i++)
            {
                if (IDs[i] < 0 || IDs[i] > Info.Sizes.MSG_MAX_INDEX) continue;
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                MSG msg;
                getMSGbyID(IDs[i], out msg);
                if (msg != null)
                    _msgList.Add(msg);
                //_loadProgress = ;
#if !NOCATCH
                }
                catch(Exception) 
                {
 
                }
#endif
            }
            _loading = false;
            //_loadProgress = -1;
            return ReturnCode.SUCCESS;
        }

        public MSG GetMSGbyID(int id)
        {
            if (id < 1 || id > 1000) return null;
            foreach (MSG msg in _msgList)
            {
                if (msg.ID == id)
                    return msg;
            }
            MSG newmsg;
            getMSGbyID(id, out newmsg);
            if (newmsg != null)
                _msgList.Add(newmsg);
            return newmsg;
        }

        public int SaveMSGbyID(int id)
        {
            MSG save = _msgList.GetMSG(id);
            if (save == null) return ReturnCode.WRONG_PLU_ID;
            return saveMSG(save);
        }

        public int AddMSG(MSG newmsg)
        {
            if (GetMSGbyID(newmsg.ID) != null) return ReturnCode.MSG_ID_ALREADY_EXIST;
            _msgList.Add(newmsg);
            return saveMSG(newmsg);
        }
        [Obsolete("Виснет.Данные не отдает")]
        public int DeleteMSGbyID(int id)
        {
            return ReturnCode.NOT_SUPPORTED;// НА весах пишется "POC",вводишь 2символа, пишется лого.При этом данные не отдает
            //MSG msg = GetMSGbyID(id);
            //if (msg == null) return ReturnCode.MSG_IS_NOT_EXISTS;
            //_msgList.Remove(msg);
            //return deleteMSG(msg);
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    }

}

