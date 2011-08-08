using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace rabnet
{
    public partial class OptionsForm : Form
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        class OptionsHolder
        {
            [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
            private int ok,vud,c1,c2,c3,br,pok,com,bo,go,sf,ff,mw,vac,gt,su,n,cn,tt,vactime,cand;
            private string gd, sh,xf;
            private RUBOOL ce, ck,crp, uz,sp,ask, fbz;
            private BuchTp bt;

            #region zooTime
            [Category("Зоотехнические сроки"),DisplayName("Окрол"),
            Description("Время от случки(вязки) до окрола")]
			public int okrol { get { return ok; } set { ok = value; } }
            [Category("Зоотехнические сроки"),DisplayName("Выдворение"),
            Description("Назначать Удаление родильного ящика из клетки, где подсосные крольчата достигли указанного возраста.")]
            public int vudvor { get { return vud; } set { vud = value; } }
            [Category("Зоотехнические сроки"),DisplayName("1й подсчет гнездовых"),
            Description("Проверка изменения числа рожденных крольчат в 1ый раз")]
            public int count1 { get { return c1; } set { c1=value;} }
            [Category("Зоотехнические сроки"),DisplayName("2й подсчет гнездовых"),
            Description("Проверка изменения числа рожденных крольчат в 2ый раз")]
            public int count2 { get { return c2; } set { c2 = value; } }
            [Category("Зоотехнические сроки"),DisplayName("3й подсчет гнездовых"),
            Description("Проверка изменения числа рожденных крольчат в 3ый раз")]
            public int count3 { get { return c3; } set { c3 = value; } }
            [Category("Зоотехнические сроки"),DisplayName("Возведение в невесты"),
            Description("Присвоить самкам со статусом Девочка, статус Невеста, достигших указанного возраста")]
            public int brides { get { return br; } set { br = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Возведение в кандидаты"),
            Description("Присвоить самцам со статусом Мальчик, статус Кандидат, достигших указанного возраста")]
            public int candidate { get { return cand; } set { cand = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Предокрольный осмотр"),
            Description("Проверка перед окролом грелки, наличие пригодного сена и состояния крольчихи, в указанный срок")]
            public int preokrol { get { return pok; } set { pok = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Объединение группы"),
            Description("Максимально допустимая разница в возрасте двух объединяемых групп гнездовых/подсосных крольчат")]
            public int combine { get { return com; } set { com = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Отсадка мальчиков"),
            Description("Назначить пересадку мальчиков из клетки кормилицы, достигших указанного возраста")]
            public int boysOut { get { return bo; } set { bo = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Отсадка девочек"),
            Description("Назначить пересдку девочек из клетки кормилицы, достигших указанного возраста")]
            public int girlsOut { get { return go; } set { go = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Назначение штатной на вязку"),
            Description("Назначить Штатную на вязку, если молодняк, сидящий с ней, достигает указанного возраста")]
            public int stateFuck { get { return sf; } set { sf = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Назначение первокролки на вязку"),
            Description("Назначить Перкокролку на вязку, если молодняк, сидящий с ней, достигает указанного возраста")]
            public int firstFuck { get { return ff; } set { ff = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Отдых самца"),
            Description("Сколько суток отдыхает отработавший самец до назначения на работу")]
            public int maleWait { get { return mw; } set { mw = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Прививка"),
            Description("Назначить на прививку молодняк, достигший указанного возраста")]
            public int vacc { get { return vac; } set { vac = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Подсчет подсосных"),
            Description("Возведение гнездовых крольчат в подсосных и подсчет их количества")]
            public int suck { get { return su; } set { su = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Установка гнездовья"),
            Description("Пересадить крольчиху в Юрту(А)")]
            public int nest { get { return n; } set { n = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Установка гнездовья при молодняке"),
            Description("Пересадить крольчиху в Юрту(А) если у нее есть подсосные")]
            public int childnest { get { return cn; } set { cn = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Действие Прививки"),
            Description("Количество дней, сколько действует прививка")]
            public int vaccine_time { get { return vactime; } set { vactime = value; } }
            #endregion zooTime
            #region view
            [Category("Вид"),
            DisplayName("Подтверждение выхода"),
            Description("Спрашивать подтверждение закрытия программы")]
            public RUBOOL confirmExit { get { return ce; } set { ce = value; } }
            [Category("Вид"),
            DisplayName("Подтверждение списания"),
            Description("Спрашивать подтверждение при списании кроликов")]
            public RUBOOL confirmKill { get { return ck; } set { ck = value; } }
            [Category("Вид"),
            DisplayName("Подтверждение пересадку"),
            Description("Спрашивать подтверждение при пересадке/отсадке кролика")]
            public RUBOOL confirmReplace { get { return crp; } set { crp = value; } }
            [Category("Вид"), 
            DisplayName("Деревья роословной"),
            Description("Количество отображаемых Деревьев родословной в Поголовье и Молодняке")]
            public int genTree { get { return gt; } set { gt = value; } }
            [Category("Вид"),
            DisplayName("Обновлять зоотехплан"),
            Description("Будет ли обновляться зоотех план после отметки работы. Внимание!!! Не стоит выключать опцию при работе по сети нескольких человек")]
            public RUBOOL updateZoo { get { return uz; } set { uz = value; } }
            [Category("Вид"),
            DisplayName("Показывать партнеров"),
            Description("Подбирать в Зоотехплане возможных патнеров для Случек и Вязок")]
            public RUBOOL showPartners { get { return sp; } set { sp = value; } }
            [Category("Вид"),
            DisplayName("Заполнять адреса нулями"),
            Description("Заполнять ли символом '0' пробелы в адресе")]
            public RUBOOL fillByZeroes { get { return fbz; } set { fbz = value; } }

            #endregion view
            #region plem
            [Category("Племенные свидетельства"),DisplayName("Номер следующего свидетельства"),Description("")]
            public int nextSvid { get { return tt; } set { tt = value; } }
            [Category("Племенные свидетельства"),DisplayName("Шапка"),Description("Текст  находяшийся в \"Шапке\" племенного свидетельства")]
            public string svidHead { get { return sh; } set { sh = value; } }
            [Category("Племенные свидетельства"),DisplayName("Генеральный директор"),Description("Иницияалы Генерального директора предприятия, отображаемые в конце племенного свидетельства")]
            public string genDir { get { return gd; } set { gd = value; } }
            #endregion plem
#if !DEMO
            #region excel
            [Category("Выгрузка в Excel"), DisplayName("Спрашивать папку"), Description("При выгрузке в Excel спрашивать папку для сохранения")]
            public RUBOOL askFolder 
            { 
                get { return ask; } 
                set 
                { 
                    ask = value;
                    if (value == RUBOOL.Нет)
                        xf = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    else xf = "";
                } 
            }
            [Category("Выгрузка в Excel"), DisplayName("Папка для сохранения"), Description("Папка в которую автоматически сохранять Excel-выгрузку"), 
            Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
            public string xlsFolder 
            { 
                get{return xf; }
                set 
                {
                    if (System.IO.Directory.Exists(value))
                        xf = value;
                }
            }
            #endregion excel
            [Category("Другое"),
            DisplayName("Источник информации о продукции"),
            Description("Что отвечает за внесение новой продукции в программу")]
            public BuchTp bucherType
            {
                get { return bt; }
                set
                {
                    bt = value;
                }
            }
#endif 
            


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
                //zoo time
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
                suck = o.getIntOption(Options.OPT_ID.SUCKERS);
                nest = o.getIntOption(Options.OPT_ID.NEST);
                childnest = o.getIntOption(Options.OPT_ID.CHILD_NEST);
                vaccine_time = o.getIntOption(Options.OPT_ID.VACCINE_TIME);
                candidate = o.getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
                //view
                genTree = o.getIntOption(Options.OPT_ID.GEN_TREE);
                confirmExit = toR(o.getIntOption(Options.OPT_ID.CONFIRM_EXIT));
                confirmKill = toR(o.getIntOption(Options.OPT_ID.CONFIRM_KILL));
                confirmReplace = toR(o.getIntOption(Options.OPT_ID.CONFIRM_REPLACE));
                updateZoo = toR(o.getIntOption(Options.OPT_ID.UPDATE_ZOO));
                showPartners = toR(o.getIntOption(Options.OPT_ID.FIND_PARTNERS));
                fbz = toR(o.getIntOption(Options.OPT_ID.BUILD_FILL_ZERO));

                //svid
                nextSvid = o.getIntOption(Options.OPT_ID.NEXT_SVID);
                svidHead = o.getOption(Options.OPT_ID.SVID_HEAD);
                genDir = o.getOption(Options.OPT_ID.SVID_GEN_DIR);
#if !DEMO
                //xls
                ask = toR(o.getIntOption(Options.OPT_ID.XLS_ASK));
                xf = o.getOption(Options.OPT_ID.XLS_FOLDER);
                if (ask == RUBOOL.Нет && xf == "") 
                    xf = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //buch
                bt = (BuchTp)o.getIntOption(Options.OPT_ID.BUCHER_TYPE);
#endif
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
                o.setOption(Options.OPT_ID.SUCKERS, suck);
                o.setOption(Options.OPT_ID.NEST, nest);
                o.setOption(Options.OPT_ID.CHILD_NEST, childnest);
                o.setOption(Options.OPT_ID.VACCINE_TIME, vaccine_time);
                o.setOption(Options.OPT_ID.MAKE_CANDIDATE, candidate);
                //view
                o.setOption(Options.OPT_ID.GEN_TREE, genTree);
                o.setOption(Options.OPT_ID.CONFIRM_EXIT, fromR(confirmExit));
                o.setOption(Options.OPT_ID.CONFIRM_KILL, fromR(confirmKill));
                o.setOption(Options.OPT_ID.CONFIRM_REPLACE,fromR(confirmReplace));
                o.setOption(Options.OPT_ID.UPDATE_ZOO, fromR(updateZoo));
                o.setOption(Options.OPT_ID.FIND_PARTNERS, fromR(showPartners));
                o.setOption(Options.OPT_ID.BUILD_FILL_ZERO,fromR(fbz));
                //svid
                o.setOption(Options.OPT_ID.NEXT_SVID, nextSvid);
                o.setOption(Options.OPT_ID.SVID_HEAD, svidHead);
                o.setOption(Options.OPT_ID.SVID_GEN_DIR, genDir);
#if !DEMO
                //xls
                o.setOption(Options.OPT_ID.XLS_ASK, fromR(ask));
                o.setOption(Options.OPT_ID.XLS_FOLDER, protectPath(xf));
                //buch
                o.setOption(Options.OPT_ID.BUCHER_TYPE,(int)bt);
#endif
            }

            private string protectPath(string path)
            {
                if (path.Contains("\\"))
                {
                    return path.Replace(@"\", @"\\");
                }
                else return path;
            }
        }

        public OptionsForm()
        {
            InitializeComponent();
            pg.SelectedObject=OptionsHolder.make();
			this.AcceptButton = button1;

//            MoveSplitter(pg, -100);
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
