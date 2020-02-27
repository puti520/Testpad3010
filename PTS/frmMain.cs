using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PTS
{
    public partial class frmMain : Form
    {
        private Testpad.NPT NPT;
        private readonly string DefaultParameterFile;
        private Testpad.NPT.DataItem Item;

        public frmMain()
        {
            InitializeComponent();

            DefaultParameterFile = Path.Combine(Application.StartupPath, "Parameter.enc");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            NPT = new Testpad.NPT();
            Item = new Testpad.NPT.DataItem();
            NPT.UpgradeProgress += NPT_UpgradeProgress;
            NPT.EventReceived += NPT_EventReceived;
            NPT.DataReceived += NPT_DataReceived;
            NPT.In1Pressed += NPT_In1Pressed;
            NPT.TouchStart = false;

            NPT.ChannelResistanceVersion = 1;

            if(File.Exists(DefaultParameterFile))
            {
                NPT.LoadParameter(DefaultParameterFile);
            }

            ParameterGrid.SelectedObject = NPT.CurrentParameter;
            ParameterGrid.CollapseAllGridItems();

            T_Refresh.Enabled = true;

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            NPT.SaveParameter(DefaultParameterFile);

            e.Cancel = false;
        }

        private void NPT_In1Pressed(object sender, EventArgs e)
        {
            Console.WriteLine("In1Pressed");
        }

        private void NPT_UpgradeProgress(double progress, double speed)
        {
            Console.WriteLine("Tick:{0}", Item.Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<double, double>(NPT_UpgradeProgress), progress, speed);
            }
            else
            {
                this.Text = string.Format("{0:P2}", progress);
            }
        }

        private void NPT_EventReceived(uint Channel, string Name, string Value, long Tick)
        {
            Display(string.Format("[{0}]:{1},{2}", Channel, Name, Value));
        }

        private void NPT_DataReceived(uint Channel, Testpad.NPT.DataItem Item)
        {
            Console.WriteLine("Tick:{0}", Item.Tick);
            Display(string.Format("[{0}]:{1},{2},{3}", Channel, Item.Name, Item.Value, Item.Flag));
        }

        private void Display(string s)
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new Action<string>(Display), s);
            }
            else
            {
                txtOutput.AppendText(s + "\r\n");
            }
        }

        private void btnExpendAll_Click(object sender, EventArgs e)
        {
            if (ParameterGrid != null)
            {
                ParameterGrid.ExpandAllGridItems();
            }
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            if (ParameterGrid != null)
            {
                ParameterGrid.CollapseAllGridItems();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.enc|*.enc";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                NPT.SaveParameter(dlg.FileName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.enc|*.enc";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                NPT.LoadParameter(dlg.FileName);
                NPT.SaveParameter(DefaultParameterFile);
                ParameterGrid.SelectedObject = NPT.CurrentParameter;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if(NPT!=null)
            {
                NPT.Download_Parameter();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(NPT!=null)
            {
				NPT.Temperature = NPT.TemperatureSensor.Temperature;
                NPT.Start(0, 1, 2, 18, "234646549");
            }
        }

        private void btnUpgrade_Click(object sender, EventArgs e)
        {
            if(NPT!=null)
            {
                NPT.Upgrade();
            }
        }

        private void T_Refresh_Tick(object sender, EventArgs e)
        {
            btnStart.Text = $"Start({NPT.TemperatureSensor.Temperature:F1})";
        }

		private void btnClear_Click(object sender, EventArgs e)
		{
			this.txtOutput.Text = "";
		}
    }
}
