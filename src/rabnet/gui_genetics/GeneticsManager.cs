using System;
using System.Collections.Generic;
using log4net;


namespace rabnet
{
	public class GeneticsManager
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(GeneticsManager));
		private static Dictionary<int, GeneticsMainForm> _GenForms = new Dictionary<int, GeneticsMainForm>();
		public static Boolean AddNewGenetics(int rab_id)
		{
			log.Debug(String.Format("Adding new genetics windows for rabbit #{0:D}", rab_id));
			if (BringUpForm(rab_id))
			{
				log.Debug("Window is already exists, bringing up");
				return true;
			}

			if (_GenForms.Count >= _MaxFormsCount)
			{
				log.Debug("Exceeded maximum number of windows...");
				return false;
			}

			GeneticsMainForm gmf = new GeneticsMainForm();

			try
			{
				_GenForms.Add(rab_id, gmf);
			}
			catch (ArgumentException)
			{
				gmf.Close();
				return false;
			}

			gmf.SetID(rab_id);
			gmf.Show();

			return true;

		}

		private static int _MaxFormsCount = 10;
		public static int MaxFormsCount
		{
			get { return _MaxFormsCount; }
			set { _MaxFormsCount = value; }
		}

		public static Boolean BroadcastSearch(RabbitCommandMessage cmd)
		{
			Boolean res = false;
			foreach (KeyValuePair<int, GeneticsMainForm> f in _GenForms)
			{
				res = res || f.Value.SearchWindow(cmd);
			}
			return res;
		}

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

		public static void CloseForm(int id)
		{
			_GenForms[id].CloseBatch();
			_GenForms.Remove(id);
		}

		public static void RemoveForm(int id)
		{
			_GenForms.Remove(id);
		}

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
