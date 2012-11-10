using System.Windows.Forms;
using System;
using TLII.IO.Utilities;

namespace GUIDgen.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnClickGenerate(object sender, EventArgs e)
        {
            if (!btnCopy.Enabled)
            {
                btnCopy.Enabled = true;
            }

            inpGUID.Text = GUIDUtility.GenerateGUID().ToString();
        }

        private void OnClickCopy(object sender, EventArgs e)
        {
            Clipboard.SetText(inpGUID.Text);
        }
    }
}
