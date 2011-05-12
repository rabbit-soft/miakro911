using System;
using System.Collections.Generic;
using log4net;


namespace rabnet
{
	public class GeneticsManager
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(GeneticsManager));
		private static Dictionary<int, GeneticsMainForm> _GenForms = new Dictionary<int, GeneticsMainForm>();
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
		public static int MaxFormsCount
		{
			get { return _maxFormsCount; }
			set { _maxFormsCount = value; }
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
