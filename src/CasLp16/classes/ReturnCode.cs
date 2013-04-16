using System;
using System.Collections.Generic;
using System.Text;

namespace CAS
{
    /// <summary>
    /// Коды ответа Данного класса
    /// </summary>
    public static class ReturnCode
    {
        public static string getDescription(int code)
        {
            switch (code)
            {
                case SUCCESS: return "Операция прошла успешно";
                case ERROR: return "Произошла ошибка";
                case SCALE_ERROR: return "Весы сообщили об ошибке";
                case BAD_HOST: return "Неверно указан адрес весов в сети";
                case CONNECTION_NOT_SET: return "Соединение с весами не установлено.";
                case WRONG_PLU_ID: return "Неверный номер записи о товаре";
                case PLU_ID_ALREADY_EXISTS: return "Запись о товаре с таким номером уже существует";

                case CONNECTION_FAIL: return "Разрыв соединения.";
                default: return "";
            }
        }

        public const byte SUCCESS = 0;
        public const byte ERROR = 1;
        public const byte SCALE_ERROR = 2;
        public const byte BAD_HOST = 3;
        public const byte CONNECTION_NOT_SET = 4;
        public const byte WRONG_PLU_ID = 5;
        public const byte PLU_ID_ALREADY_EXISTS = 6;
        public const byte BAD_PARAMS = 7;
        public const byte WRONG_MSG_ID = 8;
        public const byte MSG_ID_ALREADY_EXIST = 9;
        public const byte PLU_IS_NOT_EXISTS = 10;
        public const byte MSG_IS_NOT_EXISTS = 11;
        public const byte CONNECTION_FAIL = 12;
        public const byte NOT_SUPPORTED = 13;
        public const byte READ_TIMEOUT = 14;
    }
}
