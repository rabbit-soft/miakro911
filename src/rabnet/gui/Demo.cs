using System;
using System.Windows.Forms;

namespace rabnet
{
#if DEMO
    public static class DemoErr
    {
        public static void DemoNoReportMsg()
        {
            MessageBox.Show("Генерация отчетов недоступна в демонстрационной версии." + Environment.NewLine + "По вопросам приобретения посетите сайт www.rabbit-soft.ru", "Демонстрационная версия", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
#endif
}
