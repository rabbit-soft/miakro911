using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace rabnet
{
    public partial class OptionsForm : Form
    {
        class OptionsHolder
        {
            public enum RUBOOL {Да,Нет};

            [Category("Зоотехнические сроки"),DisplayName("Окрол"),
            Description("Время со случки(вязки) до окрола")]
            public int okrol{get;set;}
            [Category("Зоотехнические сроки"),DisplayName("Выдворение"),
            Description("")]
            public int vudvor { get; set; }
            [Category("Зоотехнические сроки"),DisplayName("Первый подсчет гнездовых"),
            Description("")]
            public int count1{get;set;}
            [Category("Зоотехнические сроки"),DisplayName("Второй подсчет гнездовых"),
            Description("")]
            public int count2 { get; set; }
            [Category("Зоотехнические сроки"),DisplayName("Третий подсчет гнездовых"),
            Description("")]
            public int count3 { get; set; }
            [Category("Зоотехнические сроки"),DisplayName("Возведение в невесты"),
            Description("")]
            public int brides{get;set;}
            [Category("Зоотехнические сроки"), DisplayName("Предокрольный осмотр"),
            Description("")]
            public int preokrol { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Объединение группы"),
            Description("Максимально допустимая разница в возрасте двую объединяемых групп")]
            public int combine { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Отсадка мальчиков"),
            Description("")]
            public int boysOut { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Отсадка девочек"),
            Description("")]
            public int girlsOut { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Назначение штатной на вязку"),
            Description("")]
            public int stateFuck { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Назначение первокролки на случку"),
            Description("")]
            public int firstFuck { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Отдых самца"),
            Description("Сколько суток отдыхает отработавший самец")]
            public int maleWait { get; set; }
            [Category("Зоотехнические сроки"), DisplayName("Прививка"),
            Description("")]
            public int vacc { get; set; }

            [Category("Вид"),
            DisplayName("Подтверждение выхода"),
            Description("Спрашивать подтверждение закрытия программы")]
            public RUBOOL confirmExit { get; set; }
            [Category("Вид"),
            DisplayName("Подтверждение списания"),
            Description("Спрашивать подтверждение при списании кроликов")]
            public RUBOOL confirmKill { get; set; }
            [Category("Вид"), 
            DisplayName("Генетические деревья"),
            Description("Количество генетических деревьев")]
            public int genTree { get; set; }

            public int fromR(RUBOOL value)
            {
                return (value == RUBOOL.Да)?1:0;
            }
            public RUBOOL toR(int value)
            {
                return value==1 ? RUBOOL.Да : RUBOOL.Нет;
            }

            public OptionsHolder()
            {
            }
            public static OptionsHolder make()
            {
                OptionsHolder op = new OptionsHolder();
                op.load();
                return op;
            }
            public void load()
            {
                Options o=Engine.opt();
                okrol=o.getIntOption(Options.OPT_ID.OKROL);
                vudvor = o.getIntOption(Options.OPT_ID.VUDVOR);
                count1 = o.getIntOption(Options.OPT_ID.COUNT1);
                count2 = o.getIntOption(Options.OPT_ID.COUNT2);
                count3 = o.getIntOption(Options.OPT_ID.COUNT3);
                brides = o.getIntOption(Options.OPT_ID.MAKE_BRIDE);
                preokrol = o.getIntOption(Options.OPT_ID.PRE_OKROL);
                combine = o.getIntOption(Options.OPT_ID.COMBINE_AGE);
                boysOut = o.getIntOption(Options.OPT_ID.BOYS_OUT);
                girlsOut = o.getIntOption(Options.OPT_ID.GIRLS_OUT);
                stateFuck = o.getIntOption(Options.OPT_ID.STATE_FUCK);
                firstFuck = o.getIntOption(Options.OPT_ID.FIRST_FUCK);
                maleWait = o.getIntOption(Options.OPT_ID.MALE_WAIT);
                vacc = o.getIntOption(Options.OPT_ID.VACC);
                //view
                genTree = o.getIntOption(Options.OPT_ID.GEN_TREE);
                confirmExit = toR(o.getIntOption(Options.OPT_ID.CONFIRM_EXIT));
                confirmKill = toR(o.getIntOption(Options.OPT_ID.CONFIRM_KILL));
            }
            public void save()
            {
                Options o = Engine.opt();
                o.setOption(Options.OPT_ID.OKROL, okrol);
                o.setOption(Options.OPT_ID.VUDVOR, vudvor);
                o.setOption(Options.OPT_ID.COUNT1, count1);
                o.setOption(Options.OPT_ID.COUNT2, count2);
                o.setOption(Options.OPT_ID.COUNT3, count3);
                o.setOption(Options.OPT_ID.MAKE_BRIDE, brides);
                o.setOption(Options.OPT_ID.PRE_OKROL, preokrol);
                o.setOption(Options.OPT_ID.COMBINE_AGE, combine);
                o.setOption(Options.OPT_ID.BOYS_OUT, boysOut);
                o.setOption(Options.OPT_ID.GIRLS_OUT, girlsOut);
                o.setOption(Options.OPT_ID.STATE_FUCK, stateFuck);
                o.setOption(Options.OPT_ID.FIRST_FUCK, firstFuck);
                o.setOption(Options.OPT_ID.MALE_WAIT, maleWait);
                o.setOption(Options.OPT_ID.VACC, vacc);
                //view
                o.setOption(Options.OPT_ID.GEN_TREE, genTree);
                o.setOption(Options.OPT_ID.CONFIRM_EXIT, fromR(confirmExit));
                o.setOption(Options.OPT_ID.CONFIRM_KILL, fromR(confirmKill));
            }
        }

        private void MoveSplitter(PropertyGrid propertyGrid, int x)
        {
            object propertyGridView =
   typeof(PropertyGrid).InvokeMember("gridView", BindingFlags.GetField |
   BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGrid, null);
            propertyGridView.GetType().InvokeMember("MoveSplitterTo",
   BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
   null, propertyGridView, new object[] { x });
        }
        public OptionsForm()
        {
            InitializeComponent();
            pg.SelectedObject=OptionsHolder.make();
            MoveSplitter(pg, 300);
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (pg.SelectedObject as OptionsHolder).save();
            Close();
        }
    }
}
