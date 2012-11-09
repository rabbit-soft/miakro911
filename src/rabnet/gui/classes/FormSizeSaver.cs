using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    static class FormSizeSaver
    {
        internal static void Append(Form frm)
        {
            loadSize(frm);
            frm.ResizeEnd += new EventHandler(form_ResizeEnd);
        }

        private static bool checkFrm(object sender, out Options.OPT_ID opt)
        {
            opt = Options.OPT_ID.BOYS_BY_ONE;
            if (sender is ReplaceForm) opt = Options.OPT_ID.FS_REPLACE;
            else if (sender is KillForm) opt = Options.OPT_ID.FS_KILL;
            else if (sender is ReplaceYoungersForm) opt = Options.OPT_ID.FS_REPLACE_YOUNG;
            else if (sender is MakeFuckForm) opt = Options.OPT_ID.FS_FUCK;
            else if (sender is DeadForm) opt = Options.OPT_ID.FS_DEAD_ARCH;
            else if (sender is GenomViewForm) opt = Options.OPT_ID.FS_GENOM_VIEW;
            else return false;
            return true;
        }

        private static void form_ResizeEnd(object sender, EventArgs e)
        {
            Options.OPT_ID opt;
            if (!checkFrm(sender, out opt)) return;
            Engine.opt().setOption(opt,String.Format("{0:d}:{1:d}",(sender as Form).Width,(sender as Form).Height));
        }     

        private static void loadSize(Form frm)
        {
            Options.OPT_ID opt;
            if (!checkFrm(frm, out opt)) return;

            string[] sizes = Engine.opt().getOption(opt).Split(':');
            if (sizes.Length < 2) return;
            int w = -1, h = -1;
            int.TryParse(sizes[0], out w);
            int.TryParse(sizes[1], out h);
            if (w > 0 && h > 0)
            {
                frm.Width = w;
                frm.Height = h;
            }
        } 
    }
}
