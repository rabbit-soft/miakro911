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
        [XmlRpcMethod("user.genkey")]
        string UserGenerateKey(string userId);
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
        /// <summary>
        /// Генерирует новый ключ пользователя
        /// <para>PARAM: int userId - id пользователя</para>
        /// <para>Return: string - Ключ пользователя (base64)</para>
        /// </summary>
        UserGenerateKey,
    }
    /// <summary>
    /// Возможные имена аргументов функции
    /// </summary>
    public static class MethodParamName
    {
        public const string userId = "userId";
    }
}
