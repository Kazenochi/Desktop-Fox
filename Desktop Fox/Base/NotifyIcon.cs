using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using Application = System.Windows.Application;

namespace DesktopFox
{
    internal class NotifyIcon
    {

        private System.Windows.Forms.NotifyIcon? notifyIcon;
        private System.Windows.Forms.ContextMenuStrip NotifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutostartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutoSetChangeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LanguageMenuItem;
        private String _language = "en-EN";
        private Fox DF;
        private Settings _settings;

        /// <summary>
        /// Konstuktor
        /// </summary>
        /// <param name="DesktopFox"></param>
        public NotifyIcon(Fox DesktopFox)
        {
            DF = DesktopFox;
            notifyIcon = new System.Windows.Forms.NotifyIcon();
           
            //Bedauerlicherweise ist das erstellen eines Notifyicons in einer WPF Application nicht so einfach umzusetzten wie in einer WinForm
            //Funktion ist jedoch gegeben und erfüllt die nötigen Aufgaben -> Vollständiges Beenden der Application

            //Festlegen der Notify Eigenschaften

            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            Application.Current.Exit += (obj, args) => { notifyIcon.Dispose(); };


            //Erstellen des Context Menüs 
            NotifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip();
            CloseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            LanguageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            NotifyIconContextMenu.SuspendLayout();

            //NotifyIcon Paras
            NotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CloseMenuItem });
            NotifyIconContextMenu.Name = "NIContextMenu";
            NotifyIconContextMenu.Size = new System.Drawing.Size(150, 75);
            //NotifyIconContextMenu.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32("FF2C394A", 16));
            //NotifyIconContextMenu.ForeColor = System.Drawing.Color.White;


            //Close Paras
            CloseMenuItem.Name = "CloseMenuItem";
            CloseMenuItem.Size = new System.Drawing.Size(150, 75);
            CloseMenuItem.Text = "Close the App";
            CloseMenuItem.Click += new EventHandler(CloseMenuItem_Click);

            //Language Paras
            LanguageMenuItem.Name = "Language";
            LanguageMenuItem.Size = new System.Drawing.Size(150, 75);
            LanguageMenuItem.Text = "Change Language";
            LanguageMenuItem.Click += new EventHandler(LanguageMenuItem_Click);

            NotifyIconContextMenu.ResumeLayout(false);
            notifyIcon.ContextMenuStrip = NotifyIconContextMenu;
        }

        /// <summary>
        /// Komplettes beenden der Application mit dem Systray Kontextmenü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Close the Application?", "Realy?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Current.Shutdown(); 
                //Close();
            }
        }

        /// <summary>
        /// Öffnet beim Doppelten linksklick auf den Systray das Hauptfenster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            DF.makeMainWindow();
        }

        private void LanguageMenuItem_Click(object sender, EventArgs e)
        {
            
            if(_language == "en")
            {
                _language = "de-DE";             
            }
            else
            {
                _language = "en";
            }
            var language = XmlLanguage.GetLanguage(_language);
            Application.Current.MainWindow.Language = language;

            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(_language);
        }

    }
}
