/* Title:           About Box
 * Date:            5-4-16
 * Author:          Terry Holmes
 *
 * Description:     This is the about box for the program */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeWarnerInventory
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the about box
            this.Close();
        }
    }
}
