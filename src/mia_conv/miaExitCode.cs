using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>
    /// Класс содержащий список возможных ExitCode, Которые возвращает mia_conv.
    /// </summary>
    public static class miaExitCode
    {
        public const int OK = 0;
        public const int ERROR = 1;
        public const int FILE_NOT_EXISTS = 2;
        public const int NOT_ENOUGH_ARGS = 3;
        public const int EXPECTED_ONE_USER = 4;
        public const int USER_MUST_HAVE_PASSWORD = 5;
        public const int DB_ALREADY_EXISTS = 6;
        public const int DB_NOT_EXISTS = 7;
        public const int DB_ACCESS_DENIED = 8;
        public const int ABORTED_BY_USER = 9;

        public static string GetText(int code)
        {
            return GetText(code, new string[0]);
        }

        public static string GetText(int code,string[] args)
        {
            switch (code)
            {
                case OK: return "Операция прошла успешно";
                case FILE_NOT_EXISTS: return "Ну удается найти mia-файл";
                case NOT_ENOUGH_ARGS: return "Не достаточно входных параметров";
                case EXPECTED_ONE_USER: return "Необходим один пользователь";
                case USER_MUST_HAVE_PASSWORD: return "У каждого пользователя";
                case DB_ALREADY_EXISTS: return "База данных уже существует";
                case DB_NOT_EXISTS: return "Базы данных не существует";
                case DB_ACCESS_DENIED: return "Не верный пользователь или пароль";
                case ABORTED_BY_USER: return "Отменено пользователем";
                default: return "";
            }
        }
    }

