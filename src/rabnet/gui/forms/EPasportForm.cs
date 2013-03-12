#if DEBUG
#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using gamlib;
using org.phprpc.util;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet.forms
{
    public partial class EPasportForm : Form
    {
        class LVSorter : IComparer
        {            
            public int Compare(object x, object y)
            {
                return (x as ListViewItem).SubItems[0].Text.CompareTo((y as ListViewItem).SubItems[0].Text);
            }
        }

        class LVExpRabIdSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                OneRabbit r1 = (x as ListViewItem).Tag as OneRabbit;
                OneRabbit r2 = (y as ListViewItem).Tag as OneRabbit;
                return r1.ID.CompareTo(r2.ID);
            }
        }

        const int COUNT_INDEX = 4;
        const int EXP_CNT_INDEX = 5;
        const int IMP_NEW_NAME = 5;
        const int IMP_ADDRESS = 6;
        Color IMPORTED_RAB_RAB = Color.Brown;
        Color IMPORTED_RAB_ASC = Color.SteelBlue;
        Color IMPORTED_ASC_RAB = Color.Chocolate;
        Color IMPORTED_ASC_ASC = Color.Coral;
        Color NOT_EXISTS = Color.LightSeaGreen;
        Color EXISTS = Color.ForestGreen;       
        Color EXISTS_NOT_ID_MATCH = Color.BlueViolet;
        Color EXISTS_IN_USE = Color.Olive;
        Color EXISTS_NOT_ID_MATCH_IN_USE = Color.Red;


        bool _export = true;
        bool _manual = true;
#if !DEMO

        readonly byte[] FILE_MARK = new byte[] { 0x19, 0xBE, 0xF8 };
        readonly byte[] KEY_CODE = Encoding.UTF8.GetBytes("035gja[ yeuql");
        BreedsList _breeds;
        RabNamesList _names;
        RabExporter _rabExport;
        BuildingList _freeBuildings;
        
        string _importFileGuid;
        //int _clientId = int.MaxValue;

        protected EPasportForm()
        {
            InitializeComponent();
#if PROTECTED
            int clientId = GRD.Instance.GetClientID();
            string clientName = GRD.Instance.GetOrganizationName();
            if (clientId == 0)
                throw new RabNetException("Клиент не зарегистрирован. Чтобы экспортировать кролика, вам необходимо зарегистрировать свою ферму на сервере разработчика.");
#else
            int clientId = int.MaxValue;
            string clientName = "Гамбито ферма";
#endif

            _rabExport = new RabExporter(clientId, clientName,Engine.get().GetDBGuid());
            _breeds = Engine.db().GetBreeds();
            _names = Engine.db().GetNames();            

            lvAscendants.ListViewItemSorter =
                lvNames.ListViewItemSorter =
                lvBreeds.ListViewItemSorter = new LVSorter();    
        }

        #region export
        public EPasportForm(List<int> rIds)
            : this()
        {
            _export = true;
            Text = "ЭКСПОРТ информации о кроликах в файл.";

            lvBreeds.Height += 30;
            lExists.Visible = lExistIDNotMatch.Visible = lMatchInUse.Visible = lNotMatchInUse.Visible = lNotExists.Visible =
                lNewName.Visible = cbNewName.Visible =
                lBreedAnalog.Visible = cbBreedAnalog.Visible =
                lReplace.Visible = cbFreeBuildings.Visible = false;

            lvBreeds.Columns.Remove(chrLocalBreedAnalog);
            lvExportRabbits.Columns.Remove(chrNewRabName);
            lvExportRabbits.Columns.Remove(chrNewRabAddress);

            exportPasport(rIds);
        }       

        public void exportPasport(List<int> rIds)
        {
            lFile.Visible = tbFileFrom.Visible = btOpenFile.Visible = false;
            rIds.Sort();
            
            List<int> noNeedAsc = new List<int>();
            List<int> breedsIds = new List<int>();
            List<int> nameIds = new List<int>();

            foreach (int rid in rIds)
            {
                OneRabbit r = Engine.db().GetRabbit(rid);
                if (r == null)
                    continue;
                if (r.EventDate != DateTime.MinValue)
                    throw new RabNetException("Нельзя экспортировать сукрольную крольчиху.");
                if(r.Sex == Rabbit.SexType.VOID)
                    throw new RabNetException("Нельзя экспортировать бесполых.");
                addExpRabbit(r, breedsIds, nameIds);

                noNeedAsc.Add(r.ID);
                addAscends(r, noNeedAsc, breedsIds, nameIds);
            }

            nameIds.Sort();
            foreach (int nId in nameIds)
            {
                RabName n = _names.Search(nId);

                addExpName(n);
            }

            breedsIds.Sort();
            foreach (int bId in breedsIds)
            {
                foreach (Breed b in _breeds)
                {
                    if (b.ID == bId)
                    {
                        ListViewItem lvi = lvBreeds.Items.Add(b.Name);
                        lvi.Tag = b;
                        break;
                    }
                }           
            }

            
            lvAscendants.Sort();
            lvNames.Sort();
            lvBreeds.Sort();
        }        

        private RabNamesList getNamesForExport()
        {
            RabNamesList result = new RabNamesList();
            foreach (ListViewItem lvi in lvNames.Items)
                result.Add(lvi.Tag as RabName);
            return result;
        }

        private BreedsList getBreedsForExport()
        {
            BreedsList result = new BreedsList();
            foreach (ListViewItem lvi in lvBreeds.Items)
                result.Add(lvi.Tag as Breed);
            return result;
        }

        private List<OneRabbit> getAscendantsForExport()
        {
            List<OneRabbit> result = new List<OneRabbit>();
            foreach (ListViewItem lvi in lvAscendants.Items)
                result.Add(lvi.Tag as OneRabbit);
            return result;
        }

        private List<OneRabbit> getRabForExport()
        {
            List<OneRabbit> result = new List<OneRabbit>();
            foreach (ListViewItem lvi in lvExportRabbits.Items)
            {
                OneRabbit r = lvi.Tag as OneRabbit;
                r.Group = int.Parse(lvi.SubItems[EXP_CNT_INDEX].Text);
                result.Add(r);
            }
            return result;
        }

        private bool exportToFile()
        {
            saveFileDialog1.FileName = String.Format("{0:s}_{1:s}_{2:s}",
#if PROTECTED
                    GRD.Instance.GetOrganizationName(),
#else
                    "Гамбито ферма \"Пыщт-Пыщь\" ",
#endif
                Engine.get().FarmName, DateTime.Now.ToShortDateString()).Replace("\"", "");
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return false;

            string data = _rabExport.Export(getRabForExport(), getAscendantsForExport(), getBreedsForExport(), getNamesForExport());
            makeExportFile(saveFileDialog1.OpenFile(), data);
            return true;
        }

        private void makeExportFile(Stream s, string data)
        {
            byte[] buff = Encoding.UTF8.GetBytes(data);
            byte[] md5 = Helper.GetMD5(buff);
            buff = XXTEA.Encrypt(buff, KEY_CODE);
            s.Write(FILE_MARK, 0, FILE_MARK.Length);
            s.Write(md5, 0, md5.Length);
            s.Write(buff, 0, buff.Length);
            s.Close();
        }

        #endregion export

        #region import

        public EPasportForm(bool import)
            : this()
        {
            _export = false;

            fillFreeBuildings();

            Text = "ИМПОРТ информации о кроликах из файла.";
            lFile.Visible = tbFileFrom.Visible = btOpenFile.Visible = true;
            lCount.Visible = nudExportCnt.Visible = chKill.Visible = chPrintPasport.Visible =false;
           
            lvExportRabbits.Columns.Remove(chrExportCount);          

            lExists.ForeColor = EXISTS;
            lExistIDNotMatch.ForeColor = EXISTS_NOT_ID_MATCH;
            lNotExists.ForeColor = NOT_EXISTS;
            lMatchInUse.ForeColor = EXISTS_IN_USE;
            lNotMatchInUse.ForeColor = EXISTS_NOT_ID_MATCH_IN_USE;

            cbBreedAnalog.Items.Add("");
            foreach (Breed b in _breeds)
                cbBreedAnalog.Items.Add(b.Name);

            //btOpenFile_Click(null, null);
        }

        private void fillFreeBuildings()
        {
            _freeBuildings = Engine.db().getBuildings(new Filters(Filters.FREE, '1'));
            cbFreeBuildings.Items.Clear();
            cbFreeBuildings.Items.Add("");
            foreach (Building b in _freeBuildings)
            {
                for (int i = 0; i < b.Sections; i++)
                {
                    if (b.Busy[i].ID == 0)
                        cbFreeBuildings.Items.Add(b.MedName(i));
                }
            }
        }

        private void updateNewNames(Rabbit.SexType sex)
        {
            cbNewName.Items.Clear();
            cbNewName.Items.Add("");
            foreach (RabName b in _names)
                if(b.Sex==sex && b.Use==0)
                    cbNewName.Items.Add(b.Name); 
        }

        /// <summary>
        /// Заполняет таблицы данными, как они выглядели при Экспорте.
        /// Красит Породы и Имена в различные цвета в зависимости от условий (есть в базе ID не совпадают, есть в базе ID совпадают, нет в базе)
        /// </summary>
        /// <param name="data"></param>
        private void import(string data)
        {
            List<OneRabbit> exportRab;
            List<OneRabbit> ascendants;
            BreedsList breeds;
            RabNamesList names;
           
            _importFileGuid = _rabExport.Import(data, out exportRab, out ascendants, out breeds, out names);
            if (Engine.db().ImportSearch(new Filters(Filters.GUID, _importFileGuid)).Count > 0)
                throw new RabNetException("Импорт из данного файла уже был произведен");

            Color cl = EXISTS;            

            foreach (RabName n in names)
            {
                cl = EXISTS;
                RabName localAnalog = _names.Search(n.Name, n.Sex);
                if (localAnalog == null)
                {
                    //Engine.db().AddName(n.Sex, n.Name, n.Surname);
                    cl = NOT_EXISTS;
                }
                else if (localAnalog.ID != n.ID)
                {
                    cl = EXISTS_NOT_ID_MATCH;
                }

                addExpName(n, cl);
            }

            foreach (Breed b in breeds)
            {
                cl = EXISTS;
                Breed localAnalog = _breeds.Search(b.Name);
                if (localAnalog == null)
                {
                    //Engine.db().AddName(n.Sex, n.Name, n.Surname);
                    cl = NOT_EXISTS;
                }
                else if (localAnalog.ID != b.ID)
                {
                    cl = EXISTS_NOT_ID_MATCH;
                }
                addExpBreed(b,cl);
            }
          
            foreach (OneRabbit r in ascendants)
                addExpAscend(r);

            foreach (OneRabbit r in exportRab)
            {
                addRabbitToLV(r);
            }
            checkImportDataAfterLoad();
        }

        private void checkImportDataAfterLoad()
        {
            foreach (ListViewItem lviRab in lvExportRabbits.Items)
            {
                OneRabbit r = lviRab.Tag as OneRabbit;
                if (r.NameID == 0) continue;///перебираем всех кроликов с именами

                foreach (ListViewItem lviName in lvNames.Items)
                {
                    RabName rn = lviName.Tag as RabName;

                    if (r.NameID == rn.ID) ///находим имя кролика в списке имен
                    {
                        if (lviName.ForeColor != NOT_EXISTS) ///если имя существует в базе
                        {
                            RabName localAnalog = _names.Search(rn.Name,rn.Sex);
                            if (localAnalog.Use != 0)///если имя в данной базе уже используется
                            {
                                if (lviRab.SubItems[IMP_NEW_NAME].Text == "")
                                    lviName.ForeColor = lviName.ForeColor == EXISTS ? EXISTS_IN_USE : EXISTS_NOT_ID_MATCH_IN_USE;
                                else
                                    lviName.ForeColor = lviName.ForeColor == EXISTS_IN_USE ? EXISTS : EXISTS_NOT_ID_MATCH;
                            }
                        }
                        break;
                    }
                }
            }

            ///проверка не были ли предки экспортированы ранее
            foreach (ListViewItem lviAsc in lvAscendants.Items)///todo протестировать
            {
                OneRabbit r = lviAsc.Tag as OneRabbit;
                List<OneImport> imp = Engine.db().ImportSearch(new Filters(Filters.OLD_RID, r.ID));
                if (imp.Count > 0)
                    lviAsc.ForeColor = IMPORTED_ASC_RAB;
                if (Engine.db().ImportAscendantExists(r.ID, r.BirthPlace))
                    lviAsc.ForeColor = IMPORTED_ASC_ASC;
            }

            ///проверка не были ли кролики экспортированы ранее
            foreach (ListViewItem lviRab in lvExportRabbits.Items)///todo протестировать
            {
                OneRabbit r = lviRab.Tag as OneRabbit;
                List<OneImport> imp = Engine.db().ImportSearch(new Filters(Filters.OLD_RID, r.ID));
                if (imp.Count > 0)
                    lviRab.ForeColor = IMPORTED_RAB_RAB;
                if (Engine.db().ImportAscendantExists(r.ID, r.BirthPlace))
                    lviRab.ForeColor = IMPORTED_RAB_ASC;                
            }    
        }

        private string parseImportFile(Stream s)
        {
            if (s.Length > int.MaxValue)
                throw new RabNetException("Файл слишком большой");
            byte[] buff = new byte[(int)FILE_MARK.Length];
            s.Read(buff, 0, buff.Length);
            if (!Helper.ArraysEquals(buff, FILE_MARK))
                throw new RabNetException("Не тип верный файла");

            buff = new byte[(int)s.Length - (int)FILE_MARK.Length];
            s.Read(buff, 0, buff.Length);
            s.Close();
            byte[] md5 = new byte[16];
            Array.Copy(buff, 0, md5, 0, md5.Length);
            byte[] data = new byte[buff.Length - md5.Length];
            Array.Copy(buff, md5.Length, data, 0, data.Length);
            try
            {
                data = XXTEA.Decrypt(data, KEY_CODE);
            }
            catch
            {
                throw new RabNetException("Не удалось расшифровать файл. Возможно файл поврежден");
            }
            if (!Helper.ArraysEquals(md5, Helper.GetMD5(data)))
                throw new RabNetException("Не верная контрольная сумма. Возможно файл поврежден");
            return Encoding.UTF8.GetString(data);
        }

        private bool importToBaseTest(out string message)
        {
            message = "";
            bool canContinue = true;
            ///проверка пород
            string tmp="";
            foreach (ListViewItem lviBreed in lvBreeds.Items)
            {
                if (lviBreed.ForeColor == NOT_EXISTS && lviBreed.SubItems[1].Text == "")
                    tmp += lviBreed.SubItems[0].Text + ",";
            }
            if(tmp!="")
                message += String.Format("Породам [{0:s}] не назначены локальные аналоги. Данные породы будут добавлены в текущую БД.{1:s}{1:s}", tmp.TrimEnd(','), Environment.NewLine);

            ///проверка имен на занятость
            tmp="";
            foreach (ListViewItem lviName in lvNames.Items)
            {
                if(lviName.ForeColor == EXISTS_IN_USE || lviName.ForeColor == EXISTS_NOT_ID_MATCH_IN_USE)
                    tmp += lviName.SubItems[0].Text + ",";
            }            
            if (tmp != "")
            {
                canContinue = false;
                message += String.Format("Имена [{0:s}] уже используются. Необходимо назначить другие.{1:s}{1:s}", tmp.TrimEnd(','), Environment.NewLine);
            }

            ///проверка на назначения новых имен и адресов
            tmp = "";
            string addr = "";
            foreach (ListViewItem lviRab in lvExportRabbits.Items)
            {
                if (lviRab.ForeColor == IMPORTED_RAB_RAB /*|| lviRab.ForeColor == IMPORTED_RAB_ASC*/) continue;

                OneRabbit r = lviRab.Tag as OneRabbit;
                if(r.Group==1 && r.NameID==0 && lviRab.SubItems[IMP_NEW_NAME].Text=="")
                    tmp += r.NameFull+ ",";
                //if(lviRab.SubItems[IMP_ADDRESS].Text=="")
                    //addr += r.NameFull + ",";
            }
            if (tmp != "")
                message += String.Format("Кроликам [{0:s}] не были назначены имена.{1:s}{1:s}", tmp.TrimEnd(','), Environment.NewLine);
            if (addr != "")
            {
                canContinue = false;
                message += String.Format("Кроликам [{0:s}] не были назначены адреса.", addr.TrimEnd(','), Environment.NewLine);
            }
            message = message.TrimEnd(Environment.NewLine.ToCharArray());

            return canContinue;
        }  

        #endregion import

        #region addexp
        private void addExpRabbit(OneRabbit r, List<int> breedsIds, List<int> nameIds)
        {
            if (!breedsIds.Contains(r.BreedID))
                breedsIds.Add(r.BreedID);
            addExpNameId(r, nameIds);

            addRabbitToLV(r);                  
        }

        private void addRabbitToLV(OneRabbit r)
        {
            ListViewItem lvi = lvExportRabbits.Items.Add(r.NameFull);
            lvi.SubItems.Add(r.BreedName);
            lvi.SubItems.Add(r.Sex== Rabbit.SexType.MALE? "м":"ж");
            lvi.SubItems.Add(r.Age.ToString());
            lvi.SubItems.Add(r.Group.ToString());
            lvi.SubItems.Add(_export ? "1" : "");
            if (!_export)
                lvi.SubItems.Add("");
            lvi.Tag = r; 
        }

        private void addExpAscend(OneRabbit m)
        {
            ListViewItem lviM = lvAscendants.Items.Add(m.NameFull);
            lviM.SubItems.Add(m.BreedName);
            lviM.SubItems.Add(m.FSex());
            lviM.Tag = m;
        }

        private void addExpName(RabName n, Color cl)
        {
            ListViewItem lvi = lvNames.Items.Add(n.Name);
            lvi.SubItems.Add(n.Surname);
            lvi.SubItems.Add(Rabbit.SexToRU(n.Sex));
            lvi.Tag = n;
            lvi.ForeColor = cl;
        }
        private void addExpName(RabName n) { addExpName(n, Color.Black); }

        private void addExpBreed(Breed b, Color cl)
        {
            ListViewItem lvi = lvBreeds.Items.Add(b.Name);
            if (!_export)
                lvi.SubItems.Add("");
            lvi.Tag = b;
            lvi.ForeColor = cl;
        }
        private void addExpBreed(Breed b) { addExpBreed(b, Color.Black); }

        private void addAscends(OneRabbit r, List<int> noNeedAsc, List<int> breedsIds, List<int> nameIds)
        {
            //r.BirthPlace = _clientId;

            if (!breedsIds.Contains(r.BreedID))
                breedsIds.Add(r.BreedID);

            addExpNameId(r,nameIds);

            if (r.MotherID != 0 && !noNeedAsc.Contains(r.MotherID))
            {
                OneRabbit m = Engine.db().GetRabbit(r.MotherID, RabAliveState.ANY);
                addExpAscend(m);

                noNeedAsc.Add(r.MotherID);
                addAscends(m, noNeedAsc, breedsIds, nameIds);
            }

            if (r.FatherID != 0 && !noNeedAsc.Contains(r.FatherID))
            {
                OneRabbit f = Engine.db().GetRabbit(r.FatherID, RabAliveState.ANY);
                addExpAscend(f);

                noNeedAsc.Add(r.FatherID);
                addAscends(f, noNeedAsc, breedsIds, nameIds);
            }
        }

        private void addExpNameId(OneRabbit r,List<int> nameIds)
        {
            if (r.NameID != 0 && !nameIds.Contains(r.NameID))
                nameIds.Add(r.NameID);
            if (r.SurnameID != 0 && !nameIds.Contains(r.SurnameID))
                nameIds.Add(r.SurnameID);
            if (r.SecnameID != 0 && !nameIds.Contains(r.SecnameID))
                nameIds.Add(r.SecnameID);
        }
        #endregion addexp

        #region methods
        /// <summary>
        /// Добавляет все данные в базу
        /// </summary>
        private void importToBase()
        {
            ///Добавляем породы в базу и переопределяем ID породы у кроликов, относительно текущей базы
            List<KeyValuePair<int, int>> wasBecome = new List<KeyValuePair<int, int>>();
            foreach (ListViewItem lviBreed in lvBreeds.Items)
            {
                if (lviBreed.ForeColor == EXISTS) continue;

                Breed b = lviBreed.Tag as Breed;
                int bID=0;
                if (lviBreed.ForeColor == NOT_EXISTS)
                {
                    if (lviBreed.SubItems[1].Text == "")
                        bID = Engine.db().AddBreed(b.Name, b.ShortName, "");
                    else
                        bID = _breeds.Search(lviBreed.SubItems[1].Text).ID;
                }
                else if (lviBreed.ForeColor == EXISTS_NOT_ID_MATCH)
                {
                    bID = _breeds.Search(lviBreed.SubItems[0].Text).ID;                   
                }
                wasBecome.Add(new KeyValuePair<int, int>(b.ID, bID));
            }
            adaptBreed(wasBecome);

            ///добавляем имена и переопределяем ID имен и фамилий относительно текущей базы
            wasBecome = new List<KeyValuePair<int, int>>();
            foreach (ListViewItem lviName in lvNames.Items)
            {
                if (lviName.ForeColor == EXISTS) continue;

                RabName rn = lviName.Tag as RabName;
                int nId=0;
                if (lviName.ForeColor == EXISTS_NOT_ID_MATCH)
                {
                    nId= _names.Search(rn.Name, rn.Sex).ID;                  
                }
                else if (lviName.ForeColor == NOT_EXISTS)
                {
                    nId = Engine.db().AddName(rn.Sex,rn.Name,rn.Surname);                    
                }              
                wasBecome.Add(new KeyValuePair<int,int>(rn.ID,nId));
            }
            adaptName(wasBecome);

            ///импортируем кроликов
            sortExpRabById();
            foreach (ListViewItem lviRab in lvExportRabbits.Items)
            {
                if (lviRab.ForeColor == IMPORTED_RAB_RAB /*|| lviRab.ForeColor == IMPORTED_RAB_ASC*/) continue;

                OneRabbit r = lviRab.Tag as OneRabbit;
                if (lviRab.SubItems[IMP_NEW_NAME].Text != "")
                {                    
                    r.NameID = _names.Search(lviRab.SubItems[IMP_NEW_NAME].Text, r.Sex).ID;
                }
                int oldRid = r.ID;
                Engine.db().NewRabbit(r, r.ParentID, r.BirthPlace, _importFileGuid);
                ///селим по назначенный адрес
                Address a = _freeBuildings.SearchByMedName(lviRab.SubItems[IMP_ADDRESS].Text);
                Engine.db().replaceRabbit(r.ID, a.Farm, a.Tier, a.Section);
                Engine.get().logs().log(LogType.REPLACE, r.ID, 0, lviRab.SubItems[IMP_ADDRESS].Text);
                lviRab.ForeColor = IMPORTED_RAB_RAB;
                adaptParent(r, oldRid);
            }
            
            foreach (ListViewItem lviAsc in lvAscendants.Items)
            {
                if (/*lviAsc.ForeColor == IMPORTED_ASC_RAB ||*/ lviAsc.ForeColor == IMPORTED_ASC_ASC) continue;

                OneRabbit r = lviAsc.Tag as OneRabbit;
                if (lviAsc.ForeColor == IMPORTED_ASC_RAB)
                {

                }
                else
                    Engine.db().ImportAscendant(r);
            }
        }

        /// <summary>
        /// Сортирует живых Кроликов по убыванию ID.
        /// </summary>
        private void sortExpRabById()
        {
            lvExportRabbits.ListViewItemSorter = new LVExpRabIdSorter();
            lvExportRabbits.Sorting = SortOrder.Ascending;
            lvExportRabbits.Sort();
        }

        /// <summary>
        /// После импорта кролика изменяет его ID у потомков (MotherID или FatherID)
        /// </summary>
        /// <param name="parent">Кролик,которого только что импортировали</param>
        /// <param name="oldPRid">ID из экспортируемой базы</param>
        private void adaptParent(OneRabbit parent, int oldPRid)
        {
            ///ищем потомков среди кроликов, которыебудут импортированы
            foreach (ListViewItem lviRab in lvExportRabbits.Items)
            {
                if (lviRab.ForeColor == IMPORTED_RAB_RAB) continue;
                OneRabbit r = lviRab.Tag as OneRabbit;
                adaptRabParentTry(parent,oldPRid,r);
            }

            foreach (ListViewItem lviRab in lvAscendants.Items)
            {
                //if (lviRab.ForeColor == IMPORTED_RAB_RAB) continue;
                OneRabbit r = lviRab.Tag as OneRabbit;
                adaptRabParentTry(parent, oldPRid, r);
            }
        }

        private void adaptRabParentTry(OneRabbit parent, int oldPRid, OneRabbit r)
        {
            if (r.Age < parent.Age)
            {
                if (parent.Sex == Rabbit.SexType.FEMALE && oldPRid == r.MotherID && parent.NameID == r.SurnameID)
                    r.MotherID = parent.ID;
                if (parent.Sex == Rabbit.SexType.MALE && oldPRid == r.FatherID && parent.NameID == r.SecnameID)
                    r.MotherID = parent.ID;
            }
        }

        private void adaptName(List<KeyValuePair<int,int>> wasBecome)
        {
            foreach (ListViewItem lviAsc in lvAscendants.Items)
            {
                OneRabbit r = lviAsc.Tag as OneRabbit;
                adaptRabName(wasBecome, r);
            }

            foreach (ListViewItem lviRab in lvExportRabbits.Items)
            {
                OneRabbit r = lviRab.Tag as OneRabbit;
                adaptRabName(wasBecome, r);
            }
        }

        private void adaptRabName(List<KeyValuePair<int, int>> wasBecome, OneRabbit r)
        {
            bool nm = false,
                    surn = false,
                    sec = false;
            foreach (KeyValuePair<int, int> kvp in wasBecome)
            {
                if (r.NameID == kvp.Key && !nm)
                {
                    r.NameID = kvp.Value;
                    nm = true;
                }
                if (r.SurnameID == kvp.Key && !surn)
                {
                    r.SurnameID = kvp.Value;
                    surn = true;
                }
                if (r.SecnameID == kvp.Key && !sec)
                {
                    r.SecnameID = kvp.Value;
                    sec = true;
                }
            }
        }

        private void adaptBreed(List<KeyValuePair<int, int>> wasBecome)
        {
            foreach (ListViewItem lviAsc in lvAscendants.Items)
            {
                OneRabbit r = lviAsc.Tag as OneRabbit;
                adaptRabBreed(wasBecome, r);
            }

            foreach (ListViewItem lviRab in lvExportRabbits.Items)
            {
                OneRabbit r = lviRab.Tag as OneRabbit;
                adaptRabBreed(wasBecome, r);
            }
        }

        private void adaptRabBreed(List<KeyValuePair<int, int>> wasBecome, OneRabbit r)
        {
            bool br = false;
            foreach (KeyValuePair<int, int> kvp in wasBecome)
            {
                if (r.BreedID == kvp.Key && !br)
                {
                    r.BreedID = kvp.Value;
                    br = true;
                    ///todo по идее тут должен быть break но я че-то забыл как тут все работает, так что не поставил
                }
            }
        }

        private void checkExporter(string data)
        {
            Client exporter = _rabExport.GetExporterInfo(data);
            if (exporter.ID == 0)
                throw new RabNetException("Нельзя импортировать поголовье от незарегистрированного пользователя.");
            ///проверяем есть ли клиент в базе, если нет, то добавляем
            ClientsList list = Engine.db().GetClients();
            bool exists = false;
            foreach (Client c in list)
            {
                if (c.ID == exporter.ID)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
                Engine.db().AddClient(exporter.ID, exporter.Name, exporter.Address);
        }
        #endregion  methods
#endif

        private void nudExportCnt_ValueChanged(object sender, EventArgs e)
        {
            lvExportRabbits.SelectedItems[0].SubItems[EXP_CNT_INDEX].Text = nudExportCnt.Value.ToString();
        }

        private void lvExportRabbits_SelectedIndexChanged(object sender, EventArgs e)
        {
#if !DEMO
            if (lvExportRabbits.SelectedItems.Count != 1) return;
            _manual = false;
            ListViewItem lvi = lvExportRabbits.SelectedItems[0];
            OneRabbit r = lvi.Tag as OneRabbit;
            if (_export)
            {                
                nudExportCnt.Enabled = r.Group != 1;
                nudExportCnt.Maximum = r.Group;
            }
            else
            {
                cbNewName.Enabled = r.Group == 1;
                updateNewNames(r.Sex);
                cbNewName.Text = lvi.SubItems[IMP_NEW_NAME].Text;
                cbFreeBuildings.Text = lvi.SubItems[IMP_ADDRESS].Text;
                cbNewName.Enabled 
                    = cbFreeBuildings.Enabled = !(lvi.ForeColor == IMPORTED_RAB_RAB /*|| lvi.ForeColor == IMPORTED_RAB_ASC*/);
            }
            _manual = true;
#endif
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            #if !DEMO
#if !NOCATCH
            try
            {
#endif
                if (_export)
                {
                    if(!exportToFile()) return;
                    if (chKill.Checked)
                    {
                        OneRabbit r;
                        foreach (ListViewItem lvi in lvExportRabbits.Items)
                        {
                            r = lvi.Tag as OneRabbit;
                            int killID=r.ID;
                            if (lvi.SubItems[COUNT_INDEX].Text != lvi.SubItems[EXP_CNT_INDEX].Text)
                            {
                                killID = Engine.db().cloneRabbit(r.ID, int.Parse(lvi.SubItems[EXP_CNT_INDEX].Text), 0, 0, 0, r.Sex, 0);
                            }
                            Engine.db().KillRabbit(killID, 0, DeadReason_Static.Selled, " экспорт");

                        }
                    }
                }
                else
                {
                    string msg = "";
                    bool b = importToBaseTest(out msg);
                    if (msg != "" && MessageBox.Show(msg + (b ? "Продолжить ?" : ""),
                        b ? "Имеются сомнения" : "Ошибки",
                        b ? MessageBoxButtons.YesNo : MessageBoxButtons.OK,
                        b ? MessageBoxIcon.Question : MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.None;
                        return;
                    }
                    importToBase();
                }
#if !NOCATCH
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
            }
#endif
#endif
        }
        
        private void btOpenFile_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (_export || openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            
            try
            {
                tbFileFrom.Text = openFileDialog1.FileName;
                Stream s = openFileDialog1.OpenFile();
                string data = parseImportFile(s);
                checkExporter(data);
                import(data);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btCancel.PerformClick();
            }
#endif
        }
      
        private void lvBreeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_export || lvBreeds.SelectedItems.Count == 0 || !_manual) return;

            _manual = false;
            if (lvBreeds.SelectedItems[0].ForeColor == NOT_EXISTS)
            {
                cbBreedAnalog.Enabled = true;
                cbBreedAnalog.Text = lvBreeds.SelectedItems[0].SubItems[1].Text;
            }
            else
            {
                cbBreedAnalog.Enabled = false;
                cbBreedAnalog.SelectedIndex = 0;
            }
            _manual = true;
        }

        private void cbBreedAnalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_export || lvBreeds.SelectedItems.Count == 0 || !_manual ) return;

            lvBreeds.SelectedItems[0].SubItems[1].Text = cbBreedAnalog.Text;
        }

        private void cbNewName_SelectedIndexChanged(object sender, EventArgs e)
        {
#if !DEMO
            if (_export || lvExportRabbits.SelectedItems.Count == 0 || !_manual) return;

            _manual = false;

            if (cbNewName.Text != "")
                foreach (ListViewItem lvi in lvExportRabbits.Items)
                    if (lvi != lvExportRabbits.SelectedItems[0] && lvi.SubItems[IMP_NEW_NAME].Text == cbNewName.Text)
                    {
                        MessageBox.Show("Данное имя уже кому-то назначено");
                        cbNewName.Text = lvExportRabbits.SelectedItems[0].SubItems[IMP_NEW_NAME].Text;
                        _manual = true;
                        return;
                    }

            lvExportRabbits.SelectedItems[0].SubItems[IMP_NEW_NAME].Text = cbNewName.Text;
            checkImportDataAfterLoad();
            _manual = true;
#endif
        }

        private void cbFreeBuildings_SelectedIndexChanged(object sender, EventArgs e)
        {
            #if !DEMO
            if (_export || lvExportRabbits.SelectedItems.Count == 0 || !_manual) return;

            _manual = false;
            if (cbFreeBuildings.Text != "")
                foreach (ListViewItem lvi in lvExportRabbits.Items)
                    if (lvi != lvExportRabbits.SelectedItems[0] && lvi.SubItems[IMP_ADDRESS].Text == cbFreeBuildings.Text)
                    {
                        MessageBox.Show("Данный адрес уже кому-то назначен");
                        cbFreeBuildings.Text = lvExportRabbits.SelectedItems[0].SubItems[IMP_ADDRESS].Text;
                        _manual = true;
                        return;
                    }
            lvExportRabbits.SelectedItems[0].SubItems[IMP_ADDRESS].Text = cbFreeBuildings.Text;
            _manual = true;
#endif
        }

        private void EPasportForm_Load(object sender, EventArgs e)
        {
            try
            {
            if (!_export)
                btOpenFile.PerformClick();
            }
            catch (RabNetException exc)
            {
                MessageBox.Show(exc.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.No;
            }
        }
    }
}

