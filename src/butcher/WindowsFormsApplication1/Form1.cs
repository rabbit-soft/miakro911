using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Declare the hook handle as an int.
        static int hHook = 0;

        //Declare the mouse hook constant.
        //For other hook types, you can obtain these values from Winuser.h in the Microsoft SDK.
        public const int WH_KEYBOARD = 2;
        public const int WH_MOUSE = 7;
//      private System.Windows.Forms.Button button1;

        //Declare MouseHookProcedure as a HookProc type.
        HookProc MouseHookProcedure;

        //Declare the wrapper managed POINT class.
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        //Declare the wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //This is the Import for the SetWindowsHookEx function.
        //Use this function to install a thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,IntPtr hInstance, int threadId);

        //This is the Import for the UnhookWindowsHookEx function.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //This is the Import for the CallNextHookEx function.
        //Use this function to pass the hook information to the next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto,CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,IntPtr wParam, IntPtr lParam);

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (hHook == 0)
            {
                // Create an instance of HookProc.
                MouseHookProcedure = new HookProc(Form1.MouseHookProc);

                //устанавливаем обработку
                hHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, (IntPtr)0, AppDomain.GetCurrentThreadId());
                //If the SetWindowsHookEx function fails.
                if (hHook == 0)
                {
                    MessageBox.Show("SetWindowsHookEx Failed");
                    return;
                }
                button1.Text = "UnHook Windows Hook";
            }
            else
            {
                bool ret = UnhookWindowsHookEx(hHook);
                //If the UnhookWindowsHookEx function fails.
                if (ret == false)
                {
                    MessageBox.Show("UnhookWindowsHookEx Failed");
                    return;
                }
                hHook = 0;
                button1.Text = "Set Windows Hook";
                this.Text = "Mouse Hook";
            }

        }

        public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //Marshall the data from the callback.
            MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                //Create a string variable that shows the current mouse coordinates.
                String strCaption = "x = " +
                        MyMouseHookStruct.pt.x.ToString("d") +
                            "  y = " +
                MyMouseHookStruct.pt.y.ToString("d");
                //You must get the active form because it is a static function.
                Form tempForm = Form.ActiveForm;

                //Set the caption of the form.
                tempForm.Text = strCaption;
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }
    }
}
