using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbyte_Common_Code.Clients_Main_files;

namespace Testting_Dll
{
    public partial class Form1 : Form
    {
        P_And_G_Common P_And_G_Common;

        public Form1()
        {
            InitializeComponent();
            P_And_G_Common = new P_And_G_Common();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool issue= P_And_G_Common.CheckData_Core("1010");            
        }
    }
}
