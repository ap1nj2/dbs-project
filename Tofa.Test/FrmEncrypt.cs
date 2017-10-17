using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web;
using CBS.Common;


namespace Tofa.Test
{
    public partial class FrmEncrypt : Form
    {
        public FrmEncrypt()
        {
            InitializeComponent();
        }
       
        private void button14_Click(object sender, EventArgs e)
        {
            txtResult.Text = EncryptDecrypt.EncryptString(txtInput.Text);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtResult.Text = EncryptDecrypt.DecryptString(txtInput.Text);
        }
    }
}