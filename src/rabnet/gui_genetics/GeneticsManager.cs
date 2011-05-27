using System;
using System.Collections.Generic;
using log4net;


namespace rabnet
{
    /// <summary>
    /// Менеджер окон генетики
    /// </summary>
	public class GeneticsManager
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(GeneticsManager));
		private static Dictionary<int, GeneticsMainForm> _GenForms = new Dictionary<int, GeneticsMainForm>();
        /// <summary>Добавляет новое окно генетики.</summary>
        /// <param name="rabID">Номер кролика, чью генетику показать</param>
        /// <returns>Успешность операции</returns>
		public static Boolean AddNewGenetics(int rabID)
		{
			log.Debug(String.Format("Adding new genetics windows for rabbit #{0:D}", rabID));
			if (BringUpForm(rabID))
			{
				log.Debug("Window is already exists, bringing up");
				return true;
			}

			if (_GenForms.Count >= _maxFormsCount)
			{
				log.Debug("Exceeded maximum number of windows...");
				return false;
			}

			GeneticsMainForm gmf = new GeneticsMainForm();

			try
			{
				_GenForms.Add(rabID, gmf);
			}
			catch (ArgumentException)
			{
				gmf.Close();
				return false;
			}

			gmf.SetID(rabID);
			gmf.Show();

			return true;

		}

		private static int _maxFormsCount = 10;
        /// <summary>Устанавливает максимальное количество окон генетики</summary>
        /// <value>Максимальное количество окон генетики</value>
		public static int MaxFormsCount
		{
			get { return _maxFormsCount; }
			set { _maxFormsCount = value; }
		}

        /// <summary>Посылает сообщение поиска кролика в окна</summary>
        /// <param name="cmd">Комманда поиска</param>
        /// <see cref="RabbitCommandMessage"/>
        /// <returns></returns>
		public static Boolean BroadcastSearch(RabbitCommandMessage cmd)
		{
			Boolean res = false;
			foreach (KeyValuePair<int, GeneticsMainForm> f in _GenForms)
			{
				res = res || f.Value.SearchWindow(cmd);
			}
			return res;
		}

        /// <summary>Закрывает все окна генетики</summary>
		public static void CloseAllForms()
		{
			if (_GenForms.Count > 0)
			{
				foreach (KeyValuePair<int, GeneticsMainForm> f in _GenForms)
				{
					f.Value.CloseBatch();
				}
				_GenForms.Clear();
			}
		}

        /// <summary>Закрывает окно генетики</summary>
        /// <param name="id">Номер окна</param>
		public static void CloseForm(int id)
		{
			_GenForms[id].CloseBatch();
			_GenForms.Remove(id);
		}

        /// <summary>Удаляет окно генетики</summary>
        /// <param name="id">Номер окна</param>
		public static void RemoveForm(int id)
		{
			_GenForms.Remove(id);
		}

        /// <summary>Делает активным окно генетики</summary>
        /// <param name="id">Номер окна</param>
        /// <returns></returns>
		public static Boolean BringUpForm(int id)
		{
			try
			{
				_GenForms[id].BringToFront();
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
