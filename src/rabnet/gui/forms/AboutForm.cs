﻿using System;
using System.Reflection;
using System.Windows.Forms;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet.forms
{
    partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            //this.Text = String.Format("About {0} {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct + " (" + AssemblyTitle+")";
            labelVersion.Text = String.Format("Версия {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            //            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;
            linkLabel.Links.Add(18,18,"www.rabbit-soft.ru");
            lbBuildDate.Text += MainForm.BuildDate.ToLongDateString();
        }

        public string licFarms()
        {
#if PROTECTED
            string info = "";
            //            return String.Format("Лицензия - {0:d} ферм",PClient.get().farms());
            info += String.Format("Лицензия{0:s}:", GRD.Instance.GetClientID()>0?" зарегистрирована":"");
            info += Environment.NewLine;
            info += String.Format("Владелец - "+ GRD.Instance.GetClientName());
            info += Environment.NewLine;
            info += String.Format("Адрес - "+GRD.Instance.GetClientAddress());
            info += Environment.NewLine;
            info += String.Format("Ограничение на количство ферм - {0:d}", GRD.Instance.GetFarmsCnt());
            info += Environment.NewLine;
            info += String.Format("Период действия - с {0} по {1}", GRD.Instance.GetDateStart().ToShortDateString(), GRD.Instance.GetDateEnd().ToShortDateString());
            info += Environment.NewLine;
            info += String.Format("Поддержка до {0}", GRD.Instance.GetSupportEnd().ToShortDateString());
            return info;
#else
#if DEMO
            return string.Format("Демонстрационная версия{0:s}Ограничение на {1:d} ферм", Environment.NewLine,panels.BuildingsPanel.DEMO_MAX_FARMS);
#else
            return "Без ограничений";
#endif
#endif
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return ""+licFarms();
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description+licFarms();
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }


 

    }
}
