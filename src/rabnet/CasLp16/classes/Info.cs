using System;
using System.Collections.Generic;
using System.Text;

namespace CAS
{
    /// <summary>
    /// Информация о размерах, адресах значений.
    /// </summary>
    static class Info
    {
        public static class Sizes
        {
            public static class PLU
            {
                public const byte ID_ADDRESS = 0x00;
                public const byte ID_LENGTH = 4;

                public const byte CODE_ADDRESS = 0x04;
                public const byte CODE_LENGTH = 6;

                public const byte PRODUCT_NAME_1_ADDRESS = 0x0A;
                public const byte PRODUCT_NAME_2_ADDRESS = 0x26;
                public const byte PRODUCT_NAME_LENGTH = 28;

                public const byte PRICE_ADDRESS = 0x42;
                public const byte PRICE_LENGTH = 4;

                public const byte LIVETIME_ADDRESS = 0x46;
                public const byte LIVETIME_LENGTH = 3;

                public const byte TARA_WEIGHT_ADDRESS = 0x49;
                public const byte TARA_WEIGHT_LENGHT = 2;

                public const byte GROUP_CODE_ADDRESS = 0x4B;
                public const byte GROUP_CODE_LENGTH = 6;

                public const byte MESSAGE_ID_ADDRESS = 0x51;
                public const byte MESSAGE_ID_LENGTH = 2;

                public const byte LAST_CLEAR_ADDRESS = 0x53;
                public const byte LAST_CLEAR_LENGTH = 6;

                public const byte TOTAL_SUMM_ADDRESS = 0x59;
                public const byte TOTAL_SUMM_LENGTH = 4;

                public const byte TOTAL_WEIGHT_ADDRESS = 0x5D;
                public const byte TOTAL_WEIGHT_LENGTH = 4;

                public const byte TOTAL_SELL_ADDRESS = 0x61;
                public const byte TOTAL_SELL_LENGTH = 3;
            }
            public static class Summary
            {
                public const byte ROLL_ADDRESS = 0x00;
                public const byte ROLL_LENGTH = 4;

                public const byte STICKER_ADDRESS = 0x04;
                public const byte STICKER_LENGTH = 4;

                public const byte SUMM_ADDRESS = 0x08;
                public const byte SUMM_LENGTH = 4;

                public const byte SELL_ADDRESS = 0x0C;
                public const byte SELL_LENGTH = 3;

                public const byte WEIGHT_ADDRESS = 0x0F;
                public const byte WEIGHT_LENGTH = 4;

                public const byte ALL_PLU_SUMM_ADDRESS = 0x13;
                public const byte ALL_PLU_SUMM_LENGTH = 4;

                public const byte ALL_PLU_SELL_ADDRESS = 0x17;
                public const byte ALL_PLU_SELL_LENGTH = 3;

                public const byte ALL_PLU_WEIGHT_ADDRESS = 0x1A;
                public const byte ALL_PLU_WEIGHT_LENGTH = 4;

                public const byte LAST_CLEAR_ADDRESS = 0x1E;
                public const byte LAST_CLEAR_LENGTH = 6;

                public const byte FREE_PLU_ADDRESS = 0x24;
                public const byte FREE_PLU_LENGTH = 2;

                public const byte FREE_MSG_ADDRESS = 0x26;
                public const byte FREE_MSG_LENGTH = 2;
            }
            public static class State
            {
                public const byte STATE_BYTE_ADDRESS = 0x00;
                public const byte STATE_BYTE_LENGHT = 1;

                public const byte ABSOLUTE_WEIGHT_ADDRESS = 0x01;
                public const byte ABSOLUTE_WEIGHT_LENGHT = 2;

                public const byte PRICE_ADDRESS = 0x02;
                public const byte PRICE_LENGHT = 4;

                public const byte VALUE_ADDRESS = 0x07;
                public const byte VALUE_LENGHT = 4;

                public const byte CHECKED_PLU_ADDRESS = 0x0B;
                public const byte CHECKED_PLU_LENGHT = 4;
            }
            public class FactoryConfig
            {
                public const byte WEIGHT_LIMIT_ADDRESS = 0x00;
                public const byte WEIGHT_LIMIT_LENGHT = 2;

                public const byte DOT_PLACE_ADDRESS = 0x02;
                public const byte DOT_PLACE_LENGHT = 3;

                public const byte DRW_MODE_ADDRESS = 0x05;
                public const byte DRW_MODE_LENGHT = 1;

                public const byte SHIT1_ADDRESS = 0x06;
                public const byte SHIT1_LENGHT = 1;

                public const byte SHIT2_ADDRESS = 0x07;
                public const byte SHIT2_LENGHT = 1;

                public const byte WEIGHT_FOR_PRICE_ADDRESS = 0x08;
                public const byte WEIGHT_FOR_PRICE_LENGHT = 2;

                public const byte ROUND_VALUE_ADDRESS = 0x0A;
                public const byte ROUND_VALUE_LENGHT = 1;

                public const byte TARA_LIMIT_ADDRESS = 0x0B;
                public const byte TARA_LIMIT_LENGHT = 2;
            }
            public class UserPreferences
            {
                public const byte DEPARTMENT_NUMBER_ADDRESS = 0x00;
                public const byte DEPARTMENT_NUMBER_LENGHT = 3;

                public const byte STICKER_FORMAT_ADDRESS = 0x03;
                public const byte STICKER_FORMAT_LENGHT = 1;

                public const byte BARCODE_FORMAT_ADDRESS = 0x04;
                public const byte BARCODE_FORMAT_LENGHT = 1;

                public const byte PRINT_OFFSET_ADDRESS = 0x05;
                public const byte PRINT_OFFSET_LENGHT = 1;

                public const byte PRINT_FEATURES_ADDRESS = 0x06;
                public const byte PRINT_FEATURES_LENGHT = 1;

                public const byte CHANGE_WEIGHT_ADDRESS = 0x07;
                public const byte CHANGE_WEIGHT_LENGHT = 2;
            }
            public static class Commands
            {
                public const int PLU_SET = 1 + 83;
                public const int MSG_SET = 1 + 2 + 400;
                public const int PLU_CLEAR = 1 + 4;
                public const int MSG_CLEAR = 1 + 2;
            }
            /// <summary>
            /// Максимальный номер ID сообщения. ID=0 допустим 
            /// </summary>
            public const int PLU_MAX_INDEX = 4000;
            /// <summary>
            /// Размер в байтах одной записи PLU = 100
            /// </summary>
            public const int PLU_LENGTH = 100;
            /// <summary>
            /// Максимально допустимое количество символов ASCII в одном сообщении.
            /// </summary>
            public const int MSG_LENGTH = 400;
            /// <summary>
            /// Максимальный номер ID сообщения
            /// </summary>
            public const int MSG_MAX_INDEX = 1000;
            /// <summary>
            /// Максимальное количество строчек в сообщении = 8
            /// </summary>
            public const int MSG_MAX_STRINGS_COUNT = 8;
            /// <summary>
            /// Размер одной строки в поле сообщения = 50
            /// </summary>
            public const int MSG_MAX_STRING_LENGTH = 50;
            public const int FACTORY_CONF_LENGTH = 13;
            public const int SUMMARY_LENGTH = 40;
            public const int STATE_LENGTH = 15;
            public const int LIVE_TIME_MAX_DAYS = 999;
        }

        /// <summary>
        /// Ответы от весов
        /// </summary>
        public static class Answers
        {
            public const byte READY = 0x80;
            public const byte SUCCESS = 0xAA;
            public const byte EMERGENCY_REQUEST_PLU = 0xDD;
            public const byte ERROR = 0xEE;
        }

        /// <summary>
        /// Команды Обмена
        /// </summary>
        public static class Commands
        {
            /// <summary>
            ///  Чтение из весов PLU
            /// </summary>
            public const byte PLU_GET = 0x81;
            /// <summary>
            /// Запись в весы PLU
            /// </summary>
            public const byte PLU_SET = 0x82;
            /// <summary>
            /// Чтение из весов сообщения
            /// </summary>
            public const byte MSG_GET = 0x83;
            /// <summary>
            /// Запись в весы сообщения
            /// </summary>
            public const byte MSG_SET = 0x84;
            /// <summary>
            /// Чтение общего итога продаж по всем товарам
            /// </summary>
            public const byte SUMMARY_GET = 0x85;
            /// <summary>
            /// Стирание суммарного итога продаж по всем товарам
            /// </summary>
            public const byte SUMMARY_CLEAR = 0x86;
            /// <summary>
            /// Запись границ единственного непрерывного диапазона номеров товаров
            /// </summary>
            public const byte BORDERS_SET = 0x87;
            /// <summary>
            /// Отмена границ диапазона номеров товаров
            /// </summary>
            public const byte BORDERS_CLEAR = 0x88;
            /// <summary>
            /// Чтение текущего состояния весов
            /// </summary>
            public const byte STATE_GET = 0x89;
            /// <summary>
            /// Запись в весы настроек пользователя
            /// </summary>
            public const byte USER_SET = 0x8A;
            /// <summary>
            /// Программирование клавиш цен
            /// </summary>
            public const byte PRICE_BUTTON_SET_PLU = 0x8B;
            /// <summary>
            /// Запись логотипа (LOGO 2)
            /// </summary>
            public const byte LOGO_SET = 0x8C;
            /// <summary>
            /// Стирание из памяти весов PLU
            /// </summary>
            public const byte PLU_CLEAR = 0x8D;
            /// <summary>
            /// Стирание из памяти весов сообщения
            /// </summary>
            public const byte MSG_CLEAR = 0x8E;
            //0x8f  //0x90  //0x91
            /// <summary>
            /// Обнуление в памяти весов итоговых данных по PLU
            /// </summary>
            public const byte PLU_SUMMARY_CLEAR = 0x92;
            //0x93
            /// <summary>
            /// Запись строк рекламной информации
            /// </summary>
            public const byte SHOP_SET = 0x94;
            /// <summary>
            /// Чтение из весов настроек пользователя
            /// </summary>
            public const byte USER_GET = 0x95;
            /// <summary>
            /// Чтение из весов номера товара, назначенного на клавишу цены
            /// </summary>
            public const byte PRICE_BUTTON_GET_PLU = 0x96;
            /// <summary>
            /// Чтение из весов логотипа (LOGO 2)
            /// </summary>
            public const byte LOGO_GET = 0x97;
            /// <summary>
            /// Чтение из весов строк рекламной информации
            /// </summary>
            public const byte SHOP_GET = 0x98;
            /// <summary>
            /// Запись в календарь весов значения даты
            /// </summary>
            public const byte DATE_SET = 0x99;
            /// <summary>
            /// Запись в часы весов значения времени. 
            /// </summary>
            public const byte TIME_SET = 0x9A;
            /// <summary>
            /// Чтение из весов заводских установок весов
            /// </summary>
            public const byte FACTORY_CONF_GET = 0x9B;
        }
    }

}
