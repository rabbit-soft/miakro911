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
            private int ok,_nout,c1,c2,c3,br,pok,com,bo,go,sf,ff,mw,vac,gt,n,cn,tt,cand,bbone;
            private string gd, sh,xf;
            private RUBOOL ce, ck,crp, uz,sp,ask, fbz,vIs,vacMoth;
            private BuchTp bt;

            #region zooTime
            [Category("Зоотехнические сроки"),DisplayName("Окрол"),
            Description("Время от случки(вязки) до окрола")]
			public int Okrol { get { return ok; } set { ok = value; } }
            [Category("Зоотехнические сроки"),DisplayName("Выдворение"),
            Description("Назначать Удаление родильного ящика из клетки, где гнездовые крольчата достигли указанного возраста.")]
            public int NestOut { get { return _nout; } set { _nout = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Выдворение при сукрольной"),
            Description("Назначать Выдворение даже если крольчиха сукрольна")]
            public RUBOOL NestOutIfSukrol { get { return vIs; } set { vIs = value; } }
            [Category("Зоотехнические сроки"),DisplayName("1й подсчет гнездовых"),
            Description("Проверка изменения числа рожденных крольчат в 1ый раз")]
            public int Count1 { get { return c1; } set { c1=value;} }
            [Category("Зоотехнические сроки"),DisplayName("2й подсчет гнездовых"),
            Description("Проверка изменения числа рожденных крольчат в 2ый раз")]
            public int Count2 { get { return c2; } set { c2 = value; } }
            [Category("Зоотехнические сроки"),DisplayName("3й подсчет гнездовых"),
            Description("Проверка изменения числа рожденных крольчат в 3ый раз")]
            public int Count3 { get { return c3; } set { c3 = value; } }
            [Category("Зоотехнические сроки"),DisplayName("Возведение в невесты"),
            Description("Присвоить самкам со статусом Девочка, статус Невеста, достигших указанного возраста")]
            public int Brides { get { return br; } set { br = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Возведение в кандидаты"),
            Description("Присвоить самцам со статусом Мальчик, статус Кандидат, достигших указанного возраста")]
            public int Candidate { get { return cand; } set { cand = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Предокрольный осмотр"),
            Description("Проверка перед окролом грелки, наличие пригодного сена и состояния крольчихи, в указанный срок")]
            public int Preokrol { get { return pok; } set { pok = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Объединение группы"),
            Description("Максимально допустимая разница в возрасте двух объединяемых групп гнездовых/подсосных крольчат")]
            public int Combine { get { return com; } set { com = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Отсадка мальчиков"),
            Description("Назначить пересадку мальчиков из клетки кормилицы, достигших указанного возраста")]
            public int BoysOut { get { return bo; } set { bo = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Отсадка девочек"),
            Description("Назначить пересдку девочек из клетки кормилицы, достигших указанного возраста")]
            public int GirlsOut { get { return go; } set { go = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Назначение штатной на вязку"),
            Description("Назначить Штатную на вязку, если молодняк, сидящий с ней, достигает указанного возраста")]
            public int StateFuck { get { return sf; } set { sf = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Назначение первокролки на вязку"),
            Description("Назначить Перкокролку на вязку, если молодняк, сидящий с ней, достигает указанного возраста")]
            public int FirstFuck { get { return ff; } set { ff = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Отдых самца"),
            Description("Сколько суток отдыхает отработавший самец до назначения на работу")]
            public int MaleWait { get { return mw; } set { mw = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Прививка"),
            Description("Назначить на прививку молодняк, достигший указанного возраста")]
            public int Vacc { get { return vac; } set { vac = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Привививать мать вместе с детьми"),
            Description("В зоотех плане на вакцинацию будут назначаться только дети. При прививке детей, будет привита и мать, с которой они сидят в однорй клетке")]
            public RUBOOL VaccWithMother { get { return vacMoth; } set { vacMoth = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Подсчет подсосных"),
            //Description("Возведение гнездовых крольчат в подсосных и подсчет их количества")]
            Description("Данная опция больше не автивна. Возведение в подсосные происходит при выдворении")]
            public int CountSuckers { get { return NestOut/*su; } set { su = value*/; } } //TODO удалить версий через 90
            [Category("Зоотехнические сроки"), DisplayName("Установка гнездовья"),
            Description("Пересадить крольчиху в Юрту(А)")]
            public int NestIn { get { return n; } set { n = value; } }
            [Category("Зоотехнические сроки"), DisplayName("Установка гнездовья при молодняке"),
            Description("Пересадить крольчиху в Юрту(А) если у нее есть подсосные")]
            public int ChildNest { get { return cn; } set { cn = value; } }
            /*[Category("Зоотехнические сроки"), DisplayName("Действие Прививки"),
            Description("Количество дней, сколько действует прививка")]
            public int VaccineTime { get { return vactime; } set { vactime = value; } }*/
            [Category("Зоотехнические сроки"), DisplayName("Рассадка мальчиков по одному"),
            Description("Назначать всем группам мальчиков,достигших указанного возраста, рассадку по одному")]
            public int BoysByOne { get { return bbone; } set { bbone = value; } }
            #endregion zooTime
            #region view
            [Category("Вид"),
            DisplayName("Подтверждение выхода"),
            Description("Спрашивать подтверждение закрытия программы")]
            public RUBOOL ConfirmExit { get { return ce; } set { ce = value; } }
            [Category("Вид"),
            DisplayName("Подтверждение списания"),
            Description("Спрашивать подтверждение при списании кроликов")]
            public RUBOOL ConfirmKill { get { return ck; } set { ck = value; } }
            [Category("Вид"),
            DisplayName("Подтверждение пересадку"),
            Description("Спрашивать подтверждение при пересадке/отсадке кролика")]
            public RUBOOL ConfirmReplace { get { return crp; } set { crp = value; } }
            [Category("Вид"), 
            DisplayName("Деревья роословной"),
            Description("Количество отображаемых Деревьев родословной в Поголовье и Молодняке")]
            public int GenTree { get { return gt; } set { gt = value; } }
            [Category("Вид"),
            DisplayName("Обновлять зоотехплан"),
            Description("Будет ли обновляться зоотех план после отметки работы. Внимание!!! Не стоит выключать опцию при работе по сети нескольких человек")]
            public RUBOOL UpdateZoo { get { return uz; } set { uz = value; } }
            [Category("Вид"),
            DisplayName("Показывать партнеров"),
            Description("Подбирать в Зоотехплане возможных патнеров для Случек и Вязок")]
            public RUBOOL ShowPartners { get { return sp; } set { sp = value; } }
            [Category("Вид"),
            DisplayName("Заполнять адреса нулями"),
            Description("Заполнять ли символом '0' пробелы в адресе")]
            public RUBOOL FillByZeroes { get { return fbz; } set { fbz = value; } }

            #endregion view
            #region plem
            [Category("Племенные свидетельства"),DisplayName("Номер следующего свидетельства"),Description("")]
            public int NextSvid { get { return tt; } set { tt = value; } }
            [Category("Племенные свидетельства"),DisplayName("Шапка"),Description("Текст  находяшийся в \"Шапке\" племенного свидетельства")]
            public string SvidHead { get { return sh; } set { sh = value; } }
            [Category("Племенные свидетельства"),DisplayName("Генеральный директор"),Description("Иницияалы Генерального директора предприятия, отображаемые в конце племенного свидетельства")]
            public string GenDir { get { return gd; } set { gd = value; } }
            #endregion plem
#if !DEMO
            #region excel
            [Category("Выгрузка в Excel"), DisplayName("Спрашивать папку"), Description("При выгрузке в Excel спрашивать папку для сохранения")]
            public RUBOOL XlsAskFolder 
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
            public string XlsFolder 
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
            public BuchTp BucherType
            {
                get { return bt; }
                set
                {
                    bt = value;
                }
            }
#endif 

            public static OptionsHolder Make()
            {
                OptionsHolder op = new OptionsHolder();
                op.load();
                return op;
            }

            private int fromR(RUBOOL value)
            {
                return (value == RUBOOL.Да)?1:0;
            }
            private RUBOOL toR(int value)
            {
                return value==1 ? RUBOOL.Да : RUBOOL.Нет;
            }
            private RUBOOL toR(bool value)
            {
                return value ? RUBOOL.Да : RUBOOL.Нет;
            }

            private void load()
            {
                Options o=Engine.opt();
                //zoo time
                Okrol=o.getIntOption(Options.OPT_ID.OKROL);
                NestOut = o.getIntOption(Options.OPT_ID.NEST_OUT);
                NestOutIfSukrol = toR(o.getIntOption(Options.OPT_ID.NEST_OUT_IF_SUKROL));
                Count1 = o.getIntOption(Options.OPT_ID.COUNT1);
                Count2 = o.getIntOption(Options.OPT_ID.COUNT2);
                Count3 = o.getIntOption(Options.OPT_ID.COUNT3);
                Brides = o.getIntOption(Options.OPT_ID.MAKE_BRIDE);
                Preokrol = o.getIntOption(Options.OPT_ID.PRE_OKROL);
                Combine = o.getIntOption(Options.OPT_ID.COMBINE_AGE);
                BoysOut = o.getIntOption(Options.OPT_ID.BOYS_OUT);
                GirlsOut = o.getIntOption(Options.OPT_ID.GIRLS_OUT);
                StateFuck = o.getIntOption(Options.OPT_ID.STATE_FUCK);
                FirstFuck = o.getIntOption(Options.OPT_ID.FIRST_FUCK);
                MaleWait = o.getIntOption(Options.OPT_ID.MALE_WAIT);
                Vacc = o.getIntOption(Options.OPT_ID.VACC);
                //suck = o.getIntOption(Options.OPT_ID.COUNT_SUCKERS);
                NestIn = o.getIntOption(Options.OPT_ID.NEST_IN);
                ChildNest = o.getIntOption(Options.OPT_ID.CHILD_NEST);
                //VaccineTime = o.getIntOption(Options.OPT_ID.VACCINE_TIME);
                VaccWithMother = toR(o.getBoolOption(Options.OPT_ID.VACC_MOTHER));
                Candidate = o.getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
                bbone = o.getIntOption(Options.OPT_ID.BOYS_BY_ONE);
                //view
                GenTree = o.getIntOption(Options.OPT_ID.GEN_TREE);
                ConfirmExit = toR(o.getIntOption(Options.OPT_ID.CONFIRM_EXIT));
                ConfirmKill = toR(o.getIntOption(Options.OPT_ID.CONFIRM_KILL));
                ConfirmReplace = toR(o.getIntOption(Options.OPT_ID.CONFIRM_REPLACE));
                UpdateZoo = toR(o.getIntOption(Options.OPT_ID.UPDATE_ZOO));
                ShowPartners = toR(o.getIntOption(Options.OPT_ID.FIND_PARTNERS));
                FillByZeroes = toR(o.getIntOption(Options.OPT_ID.BUILD_FILL_ZERO));

                //svid
                NextSvid = o.getIntOption(Options.OPT_ID.NEXT_SVID);
                SvidHead = o.getOption(Options.OPT_ID.SVID_HEAD);
                GenDir = o.getOption(Options.OPT_ID.SVID_GEN_DIR);
#if !DEMO
                //xls
                XlsAskFolder = toR(o.getIntOption(Options.OPT_ID.XLS_ASK));
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
                o.setOption(Options.OPT_ID.OKROL, Okrol);
                o.setOption(Options.OPT_ID.NEST_OUT, NestOut);
                o.setOption(Options.OPT_ID.NEST_OUT_IF_SUKROL, fromR(NestOutIfSukrol));
                o.setOption(Options.OPT_ID.COUNT1, Count1);
                o.setOption(Options.OPT_ID.COUNT2, Count2);
                o.setOption(Options.OPT_ID.COUNT3, Count3);
                o.setOption(Options.OPT_ID.MAKE_BRIDE, Brides);
                o.setOption(Options.OPT_ID.PRE_OKROL, Preokrol);
                o.setOption(Options.OPT_ID.COMBINE_AGE, Combine);
                o.setOption(Options.OPT_ID.BOYS_OUT, BoysOut);
                o.setOption(Options.OPT_ID.GIRLS_OUT, GirlsOut);
                o.setOption(Options.OPT_ID.STATE_FUCK, StateFuck);
                o.setOption(Options.OPT_ID.FIRST_FUCK, FirstFuck);
                o.setOption(Options.OPT_ID.MALE_WAIT, MaleWait);
                o.setOption(Options.OPT_ID.VACC, Vacc);
                o.setOption(Options.OPT_ID.VACC_MOTHER, fromR(VaccWithMother));
                //o.setOption(Options.OPT_ID.COUNT_SUCKERS, suck);
                o.setOption(Options.OPT_ID.NEST_IN, NestIn);
                o.setOption(Options.OPT_ID.CHILD_NEST, ChildNest);
                //o.setOption(Options.OPT_ID.VACCINE_TIME, VaccineTime);
                o.setOption(Options.OPT_ID.MAKE_CANDIDATE, Candidate);
                o.setOption(Options.OPT_ID.BOYS_BY_ONE, bbone);
                //view
                o.setOption(Options.OPT_ID.GEN_TREE, GenTree);
                o.setOption(Options.OPT_ID.CONFIRM_EXIT, fromR(ConfirmExit));
                o.setOption(Options.OPT_ID.CONFIRM_KILL, fromR(ConfirmKill));
                o.setOption(Options.OPT_ID.CONFIRM_REPLACE,fromR(ConfirmReplace));
                o.setOption(Options.OPT_ID.UPDATE_ZOO, fromR(UpdateZoo));
                o.setOption(Options.OPT_ID.FIND_PARTNERS, fromR(ShowPartners));
                o.setOption(Options.OPT_ID.BUILD_FILL_ZERO,fromR(fbz));
                //svid
                o.setOption(Options.OPT_ID.NEXT_SVID, NextSvid);
                o.setOption(Options.OPT_ID.SVID_HEAD, SvidHead);
                o.setOption(Options.OPT_ID.SVID_GEN_DIR, GenDir);
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
            pg.SelectedObject=OptionsHolder.Make();
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
