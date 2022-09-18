using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Application = System.Windows.Application;

namespace Desktop_Fox
{
    internal class NotifyIcon
    {

        private System.Windows.Forms.NotifyIcon? notifyIcon;
        private System.Windows.Forms.ContextMenuStrip NotifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutostartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutoSetChangeMenuItem;

        private MainWindow MW;
        private SettingsManager SM;
        public NotifyIcon(MainWindow mainWindow)
        {
            MW = mainWindow;
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
            AutostartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AutoSetChangeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            NotifyIconContextMenu.SuspendLayout();

            //NotifyIcon Paras
            NotifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AutoSetChangeMenuItem, AutostartMenuItem, CloseMenuItem });
            NotifyIconContextMenu.Name = "NIContextMenu";
            NotifyIconContextMenu.Size = new System.Drawing.Size(150, 75);
            //NotifyIconContextMenu.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32("FF2C394A", 16));
            //NotifyIconContextMenu.ForeColor = System.Drawing.Color.White;


            //Close Paras
            CloseMenuItem.Name = "CloseMenuItem";
            CloseMenuItem.Size = new System.Drawing.Size(150, 75);
            CloseMenuItem.Text = "Close the App";
            CloseMenuItem.Click += new EventHandler(CloseMenuItem_Click);

            //Autostart Paras
            AutostartMenuItem.Name = "Autostart";
            AutostartMenuItem.Size = new System.Drawing.Size(150, 75);
            //AutostartMenuItem.Text = "Autostart = " + SM.getAutostart().ToString();
            AutostartMenuItem.Click += new EventHandler(AutostartMenuItem_Click);

            //AutoSetChange Paras
            AutoSetChangeMenuItem.Name = "AutoSetChange";
            AutoSetChangeMenuItem.Size = new System.Drawing.Size(150, 75);
            //AutoSetChangeMenuItem.Text = "Auto SetChange = " + SM.getAutoSetChange().ToString();
            AutoSetChangeMenuItem.Click += new EventHandler(AutoSetChangeMenuItem_Click);

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
            MW.Show();
        }

        /// <summary>
        /// NotifyIcon Menu Autostart Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutostartMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// NotifyIcon Menu Auto SetChange Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoSetChangeMenuItem_Click(object sender, EventArgs e)
        {
        }


    }
}
