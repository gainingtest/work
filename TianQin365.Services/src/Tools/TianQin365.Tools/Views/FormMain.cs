using System;
using System.Windows.Forms;
using TianQin365.Tools.Models;

namespace TianQin365.Tools
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnGetKeys_Click(object sender, EventArgs e)
        {
            this.txtKeys.Text = $"DecryptionKey:{MachineKey.Create(24)}{Environment.NewLine}ValidationKey:{MachineKey.Create(64)}";
        }
    }
}
