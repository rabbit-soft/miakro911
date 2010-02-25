using System;
using log4net;

namespace rabnet
{
	class GeneticsManagerSafe
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(GeneticsManagerSafe));
		private static Boolean _HasModule = false;
	
		[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
		public static Boolean GeneticsModuleTest()
		{
			log.Debug("Test assembly 'gui_genetics.dll' presence.");
			try
			{
				System.Reflection.Assembly.Load("gui_genetics");
			}
			catch 
			{
				log.Debug("Assembly 'gui_genetics.dll' is not present.");
				return false;
			}
			log.Debug("Assembly 'gui_genetics.dll' is present.");
			_HasModule = true;
			return true;
		}


		public static int MaxFormsCount
		{
			get
			{
				if (_HasModule)
				{
					return GeneticsManager.MaxFormsCount;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				if (_HasModule)
				{
					GeneticsManager.MaxFormsCount = value;
				}
			}
		}

		public static Boolean AddNewGenetics(int rab_id)
		{
			if (_HasModule)
			{
				return GeneticsManager.AddNewGenetics(rab_id);
			}
			else
			{
				return false;
			}
		}
	}
}
