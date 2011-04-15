using System;
using System.Windows.Forms;

namespace rabnet
{
#if DEMO
    public static class DemoErr
    {
        /// <summary>
        /// Генерация отчетов недоступна в демонстрационной версии
        /// </summary>
        public static void DemoNoReportMsg()
        {
            MessageBox.Show("Генерация отчетов недоступна в демонстрационной версии." + Environment.NewLine + "По вопросам приобретения посетите сайт www.rabbit-soft.ru", "Демонстрационная версия", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Для Дэмо-версии данная функция не доступна
        /// </summary>
        public static void DemoNoModuleMsg()
        {
            MessageBox.Show("Для Дэмо-версии данная функция не доступна." + Environment.NewLine + "По вопросам приобретения посетите сайт www.rabbit-soft.ru", "Демонстрационная версия", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

#endif
}
