using System.Xml;

namespace rabnet
{
    public interface IReportInterface
    {
        /// <summary>
        /// Уникальное имя плагина
        /// </summary>
        string UniqueName { get; }
        /// <summary>
        /// Текст в контекстном меню главной формы
        /// </summary>
        string MenuText { get; }
        string FileName { get; }
        void MakeReport();
    }
}
