using System;
using System.Reflection;
using System.Text;

namespace pEngine
{
    public class pException : Exception
    {
#region errors

        [pException("Операция прошла успешно")]
        public const int OK = 0;
        [pException("Ошибка")]
        public const int Error = 1;
        [pException("Не корректный файл пользователя")]
        public const int InvalidUserFile = 2;
        [pException("Не найдено адреса ни одного сервера. Задайте свой")]
        public const int AdressesNotFound = 3;
        [pException("Не верный пароль")]
        public const int InvalidUserPassword = 4;
        [pException("Не удалось подключиться ни к одному из серверов")]
        public const int FailToConnect = 5;
        [pException("Ошибка расшифровки сообщения от сервера")]
        public const int DecryptException = 6;
        [pException("Ошибка зашифровки вопроса")]
        public const int EncryptException = 7;

        [pException("На сервере все прошло хорошо")]
        public const int ServerOK = 50;
        [pException("Ошибка на сервере")]
        public const int ServerError = 51;
        [pException("Вызываемый метод не найден на сервере")]
        public const int ServerMethodNotFound = 52;
        [pException("Необходимо сменить пароль")]
        public const int NeedChangeUserPass = 53;
        [pException("Не верный пользователь")]
        public const int IncorrectUser = 54;
        [pException("Не корректный параметр")]
        public const int IncorrectParam = 55;
        [pException("Сервер не смог расшифровать нашу посылку")]
        public const int ServerCannotDecrypt = 56;
        [pException("Сервер не смог зашифровать посылку для нас")]
        public const int ServerCannotEncrypt = 57;

#endregion errors

        private int _code = 0;
        private string _msg = "";
        private Exception _srcExc;
        public int Code { get { return _code; } }
        public override string Message  { get { return _msg; } } 
        public Exception SourceException { get { return _srcExc; } }

        public pException(int code, string message)
        {
            _msg = message;
            _code = code;
        }

        public pException(int code)
        {
            _code = code;
            _msg = getCodeMessage(code);
        }
        public pException(int Code, Exception Inner):this(Code)
        {
            _srcExc = Inner;
        }

        private string getCodeMessage(int code)
        {
            try
            {
                Type tp = this.GetType();
                FieldInfo[] flds = tp.GetFields();
                object[] atrs = null;
                pExceptionAttribute patr = null;
                foreach (FieldInfo fi in flds)
                {
                    if ((int)fi.GetValue(this) != code) continue;
                    atrs = fi.GetCustomAttributes(false);
                    foreach (object atr in atrs)
                    {
                        if (atr is pExceptionAttribute)
                        {
                            patr = atr as pExceptionAttribute;//TODO при более 2х аттрибутов выпадет
                            return patr.Message;
                        }
                    }
                    break;
                }
            }
            catch { }
            return "";
        }



    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    class pExceptionAttribute : Attribute
    {
        private string _msg;
        public string Message { get { return _msg; } set { _msg = value; } }

        public pExceptionAttribute(string Message)
        {
            _msg = Message;
        }
        
    }
}
