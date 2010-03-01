using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ImageHolder : UserControl
    {
        public ImageHolder()
        {
            InitializeComponent();
        }

        private static ImageHolder obj=null;
        public static ImageHolder get()
        {
            if (obj == null)
                obj = new ImageHolder();
            return obj;
        }

        public Image getImage(int index)
        {
            return imageList1.Images[index];
        }
    }
}
