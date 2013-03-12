using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace rabnet
{
#if !DEMO
    public enum myReportType
    {
        TEST, BREEDS, AGE, FUCKER, DEAD, DEADREASONS, REALIZE, USER_OKROLS, SHED, REPLACE, REVISION, BY_MONTH, FUCKS_BY_DATE, BUTCHER_PERIOD, RABBIT, PRIDE, ZOOTECH
    }

    public class ReportHelper
    {
        public static string GetRusName(myReportType type)
        {
            switch (type)
            {
                case myReportType.AGE: return "Статистика возрастного поголовья";
                case myReportType.BREEDS: return "Отчет по породам";
                case myReportType.BUTCHER_PERIOD: return "Стерильный цех";
                case myReportType.BY_MONTH: return "Количество по месяцам";
                case myReportType.DEAD: return "Списания";
                case myReportType.DEADREASONS: return "Причины списаний";
                case myReportType.FUCKER: return "Статистика продуктивности";
                case myReportType.FUCKS_BY_DATE: return "Список случек и вязок";
                case myReportType.PRIDE: return "Племенной список";
                case myReportType.RABBIT: return "Племенное свидетельство";
                case myReportType.REALIZE: return "Кандидаты на реализацию";
                case myReportType.REPLACE: return "План пересадок";
                case myReportType.REVISION: return "Ревизия свободных клеток";
                case myReportType.SHED: return "Шедовый отчет";
                case myReportType.USER_OKROLS: return "Окролы по пользователям";
                default: return "test";
            }
        }

        public static string GetFileName(myReportType type)
        {
            switch (type)
            {
                case myReportType.AGE: return "age";
                case myReportType.BREEDS: return "breeds";
                case myReportType.BUTCHER_PERIOD: return "butcher";
                case myReportType.BY_MONTH: return "by_month";
                case myReportType.DEAD: return "dead";
                case myReportType.DEADREASONS: return "deadreason";
                case myReportType.FUCKER: return "fucker";
                case myReportType.FUCKS_BY_DATE: return "fucks_by_date";
                case myReportType.PRIDE: return "plem";
                case myReportType.RABBIT: return "rabbit";
                case myReportType.REALIZE: return "realization";
                case myReportType.REPLACE: return "replace_plan";
                case myReportType.REVISION: return "empty_rev";
                case myReportType.SHED: return "shed";
                case myReportType.USER_OKROLS: return "okrol_user";
                default: return "test";
            }
        }

        public static string[] GetHeaders(myReportType repType)
        {
            switch (repType)
            {
                case myReportType.BREEDS: return new string[]{
                      "№",
                      "Порода",
                      "Производители",
                      "Кандидаты",
                      "Мальчики",
                      "Штатные",
                      "Первокролки",
                      "Невесты",
                      "Девочки",
                      "Безполые",
                      "Всего"};

                case myReportType.AGE: return new string[]{
                      "Возраст",
                      "Количество"};

                case myReportType.BY_MONTH: return new string[]{
                      "Дата",
                      "Всего",
                      "Осталось"};

                case myReportType.DEADREASONS: return new string[]{
                     "Причина",
                     "Количество"};

                case myReportType.DEAD: return new string[]{
                     "Дата",
                     "Имя",
                     "Количество",
                     "Причина",
                     "Заметки"};

                case myReportType.FUCKS_BY_DATE: return new string[]{
                     "Дата",
                     "Самка",
                     "Самец",
                     "Работник"};

                default: return new string[] { };
            }
        }

        public static void Append(XmlElement rw, XmlDocument doc, string name, string value)
        {
            rw.AppendChild(doc.CreateElement(name)).AppendChild(doc.CreateTextNode(value));
        }

        public static string GetChildNodeVal(XmlNode rw, string name)
        {
            return rw[name].InnerText;
        }

        internal static void AppendAttribute(XmlElement node, XmlDocument doc, string name, string value)
        {
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.Value = value;
            node.Attributes.Append(attr);
        }
    }
#endif
}
