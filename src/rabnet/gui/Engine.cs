using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Engine
    {
        private static RabNetEngine eng = null;
        public static RabNetEngine get()
        {
            if (eng==null)
                eng = new RabNetEngine();
            return eng;
        }
        public static IRabNetDataLayer db()
        {
            return get().db();
        }
        public static IRabNetDataLayer db2()
        {
            return get().db2();
        }
        public static Options opt()
        {
            return get().options();
        }
		public static void set(RabNetEngine e)
		{
			eng = e;
		}
    }
}
