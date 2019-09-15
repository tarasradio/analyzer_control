using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteppersControlApp
{
    public partial class AuthForm : Form
    {
        public bool IsAuthenticated { get; private set; }

        public AuthForm()
        {
            InitializeComponent();
        }

        private void ButtonServiceMode_Click(object sender, EventArgs e)
        {
            IsAuthenticated = true;
            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}
