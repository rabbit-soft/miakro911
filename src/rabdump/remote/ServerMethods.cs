using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;
using log4net;
using System.IO;

namespace pEngine
{
    public partial interface IServerProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("client.get.update")]
        string ClientGetUpdate(string question);

        [XmlRpcMethod("get.dumplist")]
        sDump[] GetDumpList();

        [XmlRpcMethod("get.update.files")]
        sUpdateFile[] GetUpdateFiles();

        [XmlRpcMethod("vendor.update.dongle")]
        string VendorUpdateDongle(string base64_question, string clientId, string farms, string flags, string startDate, string endDate, string dongleId);
        [XmlRpcMethod("dongle.update.success")]
        void SuccessUpdate(string dongleId);

        [XmlRpcMethod("webrep.send.global")]
        void WebRep_SendGlobal(string db,sWebRepOneDay[] value);
        [XmlRpcMethod("webrep.get.lastdate")]
        string WebRep_GetLastDate(string db);

        [XmlRpcMethod("get.update.info")]
        UpdateInfo GetUpdateInfo();
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
    ///     <para>bool - строка содержащая "true" или "false"</para>
    /// </para>
    /// </remarks>
    /// <see cref="IServerProxy"/>
    public enum MethodName
    {
        GetUpdateFiles,

        /// <summary>
        /// Получает сортированный по убыванию дат список РКБД, хранящихся на сервере.
        /// </summary>
        GetDumpList,
        

        /// <summary>
        /// Сообщает серверу, что обновления ключа прошло успешно
        /// <para>PARAM: int dongleId - ID ключа</para>
        /// </summary>
        //SuccessUpdate,

        WebRep_GetLastDate,
        WebRep_SendGlobal,
        ClientGetUpdate
    }

    /// <summary>
    /// Возможные имена аргументов функции
    /// </summary>
    public static class MPN
    {
        public const string base64_question = "base64_question";
        public const string db = "db";        
        public const string dongleId = "dongleId";
        public const string clientId = "clientId";
        public const string farm = "farm";
        public const string farms = "farms";
        public const string flags = "flags";        
        public const string question = "question";
        public const string startDate = "startDate";
        public const string endDate = "endDate";
        public const string value = "value";

    }
}
