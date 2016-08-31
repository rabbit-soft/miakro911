/**
 * Смысл такой загонности с Reflection методами и большой структурой вызова методов, 
 *  нужна для того, чтобы была возможность выполнить несколько методов на сервере ассинхронно.
 *  если мы использовали в старой Админке xml, было все просто: Сформировали все запросы в один файл, отправили - получили ответ одним файлом.
 *  Здесь же нужно вызвать каждуй метод друг за другом, но вс их нужно вызвать Ассинхронно.
 */
using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace pEngine
{
    /// <summary>
    /// Все параметры должны быть STRING !!!
    /// Описание смотри в MethodName
    /// </summary>
    public interface IServerProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("fi.add.button")]
        void AddFIButton(sFIButton button);
        [XmlRpcMethod("fi.add.menu")]
        void AddFIMenu(sFIMenu value);

        [XmlRpcMethod("add.station")]
        void AddStation(string name,string address,string groupId,string tariffId);

        [XmlRpcMethod("accept.incase")]
        void AcceptIncase(string terminalId,string startCheck,string endCheck,string summ);
        [XmlRpcMethod("accept.incase")]
        void AcceptIncase(string terminalId,string startCheck,string endCheck,string summ,string units);
        [XmlRpcMethod("accept.incase")]
        void AcceptIncase(string terminalId, string startCheck, string endCheck, string summ, string units, string date);

        [XmlRpcMethod("station.block")]
        void BlockStation(string stationId, string value);

        [XmlRpcMethod("change.group.tariff")]
        void ChangeFIButton(sFIButton button);

        [XmlRpcMethod("change.group.tariff")]
        void ChangeGroupTariff(string groupId, string tariffId);
        [XmlRpcMethod("change.station.tariff")]
        void ChangeStationTariff(string stationId, string tariffId);

        [XmlRpcMethod("change.processgroups")]
        void ChangeProcessGroup(string groupId, string procgroupId);

        [XmlRpcMethod("change.terminal.accnum")]
        void ChangeTerminalAccNumber(string terminalId, string account,string number);       

        [XmlRpcMethod("close.station")]
        void CloseStation(string stationId);

        [XmlRpcMethod("fi.delete.button")]
        void DeleteFIButton(string id);
        [XmlRpcMethod("fi.delete.menu")]
        void DeleteFIMenu(string id);

        [XmlRpcMethod("fi.get.buttons")]
        sFIButton[] GetFIButtons();
        [XmlRpcMethod("fi.get.menus")]
        sFIMenu[] GetFIMenus();

        [XmlRpcMethod("get.types.account")]
        sAccountType[] GetAccountTypes();

        [XmlRpcMethod("get.groups")]
        sGroup[] GetGroups();
        [XmlRpcMethod("get.groups")]
        sGroup[] GetGroups(string groupId);

        [XmlRpcMethod("get.incases")]
        sIncase[] GetIncases(string datefrom, string dateto, string parameters);

        [XmlRpcMethod("get.operators")]
        sOperator[] GetOperators();

        [XmlRpcMethod("get.service_types")]
        sServiceType[] GetServiceType();

        [XmlRpcMethod("get.processgroups")]
        sProcessGroup[] GetProcessGroups();

        [XmlRpcMethod("get.report")]
        sReport[] GetReport(string type, string datefrom, string dateto, string parameters);

        [XmlRpcMethod("get.resend.comments")]
        sResendComment[] GetResendComments();

        [XmlRpcMethod("get.servers")]
        sServer[] GetServers();

        //[XmlRpcMethod("get.station")]
        //sStation GetStation(string stationId);

        [XmlRpcMethod("get.station.info")]
        sStationInfo GetStationInfo(string stationId,string terminalId);

        [XmlRpcMethod("get.stations")]
        sStation[] GetStations();
        [XmlRpcMethod("get.stations")]
        sStation[] GetStations(string groupId);
        

        [XmlRpcMethod("get.stations.lite")]
        sStation[] GetStationsLite();
        [XmlRpcMethod("get.stations.lite")]
        sStation[] GetStationsLite(string value);

        //[XmlRpcMethod("get.stations.nums")]
        //string[] GetStationsNums(string groupId);

        [XmlRpcMethod("get.stations.unreg")]
        sStation[] GetStationsUnreg(string groupId);

        [XmlRpcMethod("get.tariffs")]
        sTariff[] GetTariffs();

        [XmlRpcMethod("get.update.files")]
        sUpdateFile[] GetUpdateFiles();

        [XmlRpcMethod("get.users.permissions")]
        sUserPermission[] GetUsersPermissions();

        [XmlRpcMethod("pay.repair")]
        void PayRepair(string payId, string accounttype, string account, string comment);

        [XmlRpcMethod("pay.resend")]
        string PayResend(string payId);

        [XmlRpcMethod("rename.station")]
        void RenameStation(string stationId, string name, string address);
        
        [XmlRpcMethod("search.pays")]
        sPay[] SearchPays(string parameters);

        [XmlRpcMethod("terminal.move")]
        void MoveTerminal(string stationId, string name, string address);
        [XmlRpcMethod("terminal.move")]
        void MoveTerminal(string stationId, string name, string address, string date);
        [XmlRpcMethod("terminal.move")]
        void MoveTerminal(string stationId, string name, string address, string date, string groupId);

        [XmlRpcMethod("user.save.permissions")]
        void SaveUserPermissions(string userId,string csv,string groupId);

        [XmlRpcMethod("terminal.command")]
        void TerminalCommand(string terminalId, string command);
        [XmlRpcMethod("terminal.command")]
        void TerminalCommand(string terminalId, string command, string parameters);

        [XmlRpcMethod("user.add")]
        string UserAdd(string name);

        [XmlRpcMethod("user.block")]
        void UserBlock(string userId,string value);

        [XmlRpcMethod("user.genkey")]
        string UserGenerateKey(string userId);

        [XmlRpcMethod("user.permissions")]
        sPermission[] UserPermissions();
        [XmlRpcMethod("user.permissions")]
        sPermission[] UserPermissions(string userId);
    }
    
    /// <summary>
    /// Список имеющихся методов на сервере
    /// <para>[X] - Необязательный параметр.</para>
    /// </summary>
    /// <remarks>
    /// <para>Название членов этого класса должны совпадать с именами методов интерфейса IServerProxy</para>
    /// <para>При вызове метода удаленного метода класс RequestPack имена параметров MethodParams также должны 
    /// совпадать с параметрами методов интерфейса IServerProxy</para>
    /// <para>Тип всех передаваемых параметров должен быть String.</para>
    /// <para>
    ///     <para>Тип "int" означает, что параметр должен быть числом;</para>
    ///     <para>MysqlDate - дата в формате yyyy-MM-dd</para>
    ///     <para>params - строка параметров в формате "имя1=знач1;имя2=знач2;"</para>
    ///     <para>CSV - Значения разделенные запятой (v1,v2,v3, ...)</para>
    ///     <para>bool - строка содержащая [true|false] | [1|0]</para>
    /// </para>
    /// </remarks>
    /// <see cref="IServerProxy"/>
    public enum MethodName
    {
        /// <summary>
        /// Добавляет новую кнопку для Flash интерфейса
        /// </summary>
        AddFIButton,

        /// <summary>
        /// Добавляет новую кнопку для Flash интерфейса
        /// </summary>
        /// <para>PARAM: sFIMenu value</para>
        AddFIMenu,

        /// <summary>
        /// Создает станцию.
        /// <para>PARAM: string name</para>
        /// <para>PARAM: string address</para>
        /// <para>PARAM: int groupId</para>
        /// <para>PARAM: int tariffId</para>
        /// </summary>
        AddStation,

        /// <summary>
        /// Принятие инкассации
        /// <para>PARAM: int terminalId</para>
        /// <para>PARAM: int startCheck</para>
        /// <para>PARAM: int endCheck</para>
        /// <para>PARAM: int summ</para>
        /// <para>PARAM: [X] string units - Значения по купюрам и монетам, разделенные через ";"</para>
        /// <para>PARAM: [X] MysqlDate date</para>
        /// </summary>
        AcceptIncase,

        /// <summary>
        /// Блокирует станцию
        /// <para>PARAM: int stationId - номер станции</para>
        /// <para>PARAM: bool value - если true, то блокируем</para>
        /// </summary>
        BlockStation,

        /// <summary>
        /// Изменяет свойства кнопки FlashInterface'а
        /// </summary>
        ChangeFIButton,

        /// <summary>
        /// Изменяет тариф группы
        /// <para>PARAM: int groupId</para>
        /// <para>PARAM: int tariffId</para>
        /// </summary>
        ChangeGroupTariff,

        /// <summary>
        /// Изменяет тариф аппарата
        /// <para>PARAM: int stationId</para>
        /// <para>PARAM: int tariffId</para>
        /// </summary>
        ChangeStationTariff,

        /// <summary>
        /// Изменяет группу проведения терминалам группы
        /// <para>PARAM: int groupId</para>
        /// <para>PARAM: int procgroupId</para>
        /// </summary>
        ChangeProcessGroup,

        /// <summary>
        /// Изменяет аккаунта и номера терминала
        /// <para>PARAM: int terminalId</para>
        /// <para>PARAM: intstring account</para>
        /// <para>PARAM: int number</para>
        /// </summary>
        ChangeTerminalAccNumber,

        /// <summary>
        /// Перенос терминала.
        /// <para>PARAM: int stationId</para>
        /// <para>PARAM: string name - Новое название</para>
        /// <para>PARAM: string address - Новый адрес</para>
        /// <para>PARAM: [X]MysqlDate date - Дата после которой терминал можно переносить терминал.</para>
        /// <para>PARAM: [X]int groupId - Новая группа терминала</para>
        /// <remarks>
        /// При переносе терминала, старая станция закрывает.Создается новая станция с этим терминалом.
        /// Станция закрывается только если нет не инкассированных денег.
        /// Если произведена инкассация, а дата переноса назначена позже, то терминал не переносится.
        /// </remarks>
        /// </summary>
        MoveTerminal,

        /// <summary>
        /// Прекращет работу станции
        /// <para>PARAM: int stationId</para>
        /// </summary>
        CloseStation,

        /// <summary>
        /// Удаляет кнопку Flash Интерфейса
        /// </summary>
        /// <para>PARAM: int id</para>
        DeleteFIButton,

        /// <summary>
        /// Удаляет меню Flash Интерфейса
        /// </summary>
        /// <para>PARAM: int id</para>
        DeleteFIMenu,

        /// <summary>
        /// Получение списка групп
        /// <para>PARAM: [X]int groupId</para>
        /// <para>Return: sGroup[]</para>
        /// </summary>      
        GetGroups,

        /// <summary>
        /// Получает кнопки на Терминале
        /// </summary>
        /// <para>Return: sFIButton[]</para>
        GetFIButtons,

        /// <summary>
        /// Получает кнопки на Терминале
        /// </summary>
        /// <para>Return: sFIMenu[]</para>
        GetFIMenus,

        /// <summary>
        /// Получает список инкасаций 
        /// <para>PARAM: MysqlDate datefrom -  c даты</para>
        /// <para>PARAM: MysqlDate dateto - по дату</para>
        /// <para>PARAM: params parameters</para>
        /// <para>Return: sIncase[]</para>
        /// </summary>
        GetIncases,

        /// <summary>
        /// Получает список операторов
        /// </summary>
        GetProcessGroups,

        /// <summary>
        /// Список групп проведения
        /// <para>Return: sProcessGroup[]</para>
        /// </summary>
        GetOperators,

        /// <summary>
        /// Получает список типов имеющихся сервисов оплат услуг.
        /// </summary>
        /// <para>Return: sServiceType[]</para>
        GetServiceType,

        /// <summary>
        /// Получить отчет
        /// <para>PARAM: ReportType stationId</para>
        /// <para>PARAM: MysqlDate dateFrom</para>
        /// <para>PARAM: MysqlDate dateTo</para>
        /// <para>PARAM: params parameters</para>
        /// <para>Return: sReport[]</para>
        /// </summary>
        GetReport,

        /// <summary>
        /// Список коментариев при перепроведении
        /// <para>Return: sResendComments[]</para>
        /// </summary>
        GetResendComments,

        /// <summary>
        /// Получает список серверов
        /// </summary>
        /// <para>Return: sServer[]</para>
        GetServers,

        /// <summary>
        /// Получение списка станций
        /// <para>PARAM: int stationId</para>
        /// <para>PARAM: int terminalId</para>
        /// <para>Return: sStationInfo</para>
        /// </summary>
        GetStationInfo,

        /// <summary>
        /// Получает станцию с указанным ID
        /// <para>PARAM: [X]int stationId</para>
        /// <para>Return: sStation</para>
        /// </summary>
        //GetStation,

        /// <summary>
        /// Получение списка станций и информацию о каждой
        /// <para>PARAM: [X]int groupId</para>
        /// <para>Return: sStation[]</para>
        /// </summary>
        GetStations,

        /// <summary>
        /// Получение спискf Номеров станций
        /// <para>PARAM: [X]int groupId</para>
        /// <para>Return: int[]</para>
        /// </summary>
        //GetStationsNums,

        /// <summary>
        /// Получение список новых не зарегистрированных станций
        /// <para>PARAM: [X]int groupId</para>
        /// <para>Return: int[]</para>
        /// </summary>
        GetStationsUnreg,

        /// <summary>
        /// Получение списка станций, для списка групп
        /// <para>PARAM: [X]bool value - загружать ли удаленные</para>
        /// <para>Return: sStationLite[]</para>
        /// </summary>
        GetStationsLite,

        /// <summary>
        /// Получение всех существующих тарифов
        /// <para>Return: sTariff[]</para>
        /// </summary>
        GetTariffs,

        /// <summary>
        /// Получает список файлов программы, доступных для скачивания.
        /// </summary>
        GetUpdateFiles,

        /// <summary>
        /// Получение пользователей и их прав
        /// <para>Return: sUsersPermissions[]</para>
        /// </summary>
        GetUsersPermissions,

        /// <summary>
        /// Получение всех существующих типов операторов
        /// <para>Return: sAccountType[]</para>
        /// </summary>
        GetAccountTypes,

        /// <summary>
        /// Изменяет реквизиты ошибочного платежа
        /// <para>PARAM: int payId - номер платежа</para>
        /// <para>PARAM: int accounttype - новый сервис</para>
        /// <para>PARAM: int account - новый номер</para>
        /// <para>PARAM: string comment - коментарий</para>
        /// </summary>
        PayRepair,

        /// <summary>
        /// Перепроводит платеж
        /// <para>PARAM: int payId</para>
        /// <para>Return: string</para>
        /// </summary>
        PayResend,

        /// <summary>
        /// Изменяет название и адрес станции
        /// <para>PARAM: int stationId</para>
        /// <para>PARAM: string name</para>
        /// <para>PARAM: string address</para>
        /// </summary>
        RenameStation,

        /// <summary>
        /// Устанавливает разрешения, которые доступны пользователю
        /// <para>PARAM: int userId</para>
        /// <para>PARAM: CSV csv</para>
        /// <para>PARAM: int groupId</para>
        /// </summary>
        SaveUserPermissions,

        /// <summary>
        /// Поиск платежей по заданным параметрам
        /// <para>PARAM: params parameters </para>
        /// </summary>
        SearchPays,

        /// <summary>
        /// Посылает терминалу команду, которую надо выполнить
        /// <para>PARAM: string terminalId</para>
        /// <para>PARAM: string command</para>
        /// <para>PARAM: [X]params parameters</para>
        /// </summary>
        TerminalCommand,

        /// <summary>
        /// Создает нового пользователя
        /// <para>PARAM:string name - имя нового пользователя</para>
        /// <para>Return: string - Ключ пользователя (base64)</para>
        /// </summary>
        UserAdd,

        /// <summary>
        /// Блокируем или разблокируем пользователя.
        /// <para>PARAM: int userId - id пользователя</para>
        /// <para>PARAM: bool value - блокировать или разблокировать</para>
        /// </summary>
        UserBlock,

        /// <summary>
        /// Генерирует новый ключ пользователя
        /// <para>PARAM: int userId - id пользователя</para>
        /// <para>Return: string - Ключ пользователя (base64)</para>
        /// </summary>
        UserGenerateKey,

        /// <summary>
        /// Получает список прав(разрешений) пользователя
        /// <para>PARAM: [X]int userId</para>
        /// <para>Return: string[]</para>
        /// </summary>
        UserPermissions,
    }
    
    /// <summary>
    /// Возможные имена аргументов функции - Method's Param Name
    /// </summary>
    public static class MPN
    {
        public const string account = "account";
        public const string accounttype = "accounttype";
        public const string address = "address";
        public const string button = "button";
        public const string check = "check"; 
        public const string command = "command";
        public const string comment = "comment";
        public const string csv = "csv";
        public const string date = "date";
        public const string datefrom = "datefrom";
        public const string dateto = "dateto";
        public const string endCheck = "endCheck";
        public const string errornum = "errornum";
        public const string erroronly = "erroronly";
        public const string groupId = "groupId";
        public const string id = "id";
        public const string limit = "limit";
        public const string name = "name";
        public const string number = "number";
        public const string operId = "operId";
        public const string payId = "payId";
        public const string parameters = "parameters";
        public const string procgroupId = "procgroupId";
        public const string startCheck = "startCheck";
        public const string stationId = "stationId";
        public const string summ = "summ"; 
        public const string tariffId = "tariffId";      
        public const string terminalId = "terminalId";
        public const string timeFrom = "timeFrom";
        public const string timeTo = "timeTo";
        public const string type = "type";
        public const string value = "value";
        public const string units = "units";
        public const string userId = "userId";
    }

    public static class ReportType
    {
        public const string Stations = "stations";
        public const string Operators = "operators";
        public const string Dates = "dates";
        public const string PaySystems = "paySystems";
        public const string ErrorPays = "errorPays";
    }
}
