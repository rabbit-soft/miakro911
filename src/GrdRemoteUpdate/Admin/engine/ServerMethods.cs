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
        [XmlRpcMethod("client.add")]
        void AddClient(string orgName, string contact, string address, string saas);
        [XmlRpcMethod("client.money.add")]
        void AddClientMoney(string orgId, string money);
        [XmlRpcMethod("user.genkey")]
        string UserGenerateKey(string userId);
        [XmlRpcMethod("ping")]
        string Ping();
        [XmlRpcMethod("clients.get")]
        sClient[] GetClients();
        

        [XmlRpcMethod("vendor.add.dongle")]
        void VendorAddDongle(string dongleId, string orgId, string model);
        [XmlRpcMethod("vendor.update.dongle")]
        string VendorUpdateDongle(string base64_question, string orgId, string farms, string flags, string startDate, string endDate, string dongleId);
    }
    
    /// <summary>
    /// Список имеющихся методов на сервере (MethodName)
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
    ///     <para>bool - строка содержащая "true" или "false"</para>
    /// </para>
    /// </remarks>
    /// <see cref="IServerProxy"/>
    public enum MName
    {
        /// <summary>
        /// Генерирует новый ключ пользователя
        /// <para>PARAM: int userId - id пользователя</para>
        /// <para>Return: string - Ключ пользователя (base64)</para>
        /// </summary>
        UserGenerateKey,

        /// <summary>
        /// Проверка подключения
        /// </summary>
        Ping,

        /// <summary>
        /// Получает список клиентов и их ключей
        /// <para>Return: sClients[] - Массив клиентов</para>
        /// </summary>
        GetClients,

        /// <summary>
        /// Добавляет нового клиента
        /// <para>PARAM: string orgId - Id клиента</para>
        /// <para>PARAM: string money -Сколько денег добавить</para>
        /// </summary>
        AddClient,

        /// <summary>
        /// Добавляет нового клиента
        /// <para>PARAM: string orgName - Название организации</para>
        /// <para>PARAM: string contact - Контактное лицо</para>
        /// <para>PARAM: string address - Адрес организации</para>
        /// <para>PARAM: string saas - СаасВерсия или нет</para>
        /// </summary>
        AddClientMoney,

        /// <summary>
        /// Прошивает ключ для нового пользователя
        /// <para>PARAM: string base64_question - число вопрос</para>
        /// <para>PARAM: int orgId - ID организации</para>
        /// <para>PARAM: int farms - количество ферм</para>
        /// <para>PARAM: int flags - маска ролей</para>
        /// <para>PARAM: MysqlDate startDate - начало работы ключа</para>
        /// <para>PARAM: MysqlDate endDate - окончание работы ключа</para>
        /// <para>PARAM: int dongleId - ID ключа</para>
        /// <para>Return: string - base64 ответ</para>
        /// </summary>
        VendorUpdateDongle,

        /// <summary>
        /// Прошивает ключ для нового пользователя
        /// <para>PARAM: int dongleId - ID ключа</para>
        /// <para>PARAM: int orgId - ID организации</para>
        /// <para>PARAM: int type - Тип Ключа</para>
        /// </summary>
        VendorAddDongle
    }
    /// <summary>
    /// Возможные имена аргументов функции
    /// MethodParamName
    /// </summary>
    public static class MPN
    {
        public const string userId = "userId";
        public const string dongleId = "dongleId";
        public const string orgName = "orgName";
        public const string contact = "contact";
        public const string address = "address";
        public const string base64_question = "base64_question";
        public const string farms = "farms";
        public const string flags = "flags";
        public const string startDate = "startDate";
        public const string endDate = "endDate";
        public const string orgId = "orgId";
        public const string model = "model";
        public const string money = "money";
        public const string saas = "saas";
    }
}
