/* Title:           Issue Inventory
 * Date:            5-4-16
 * Author:          Terry Holmes
 *
 * Description:     This is used to issue the material */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagesDLL;

namespace TimeWarnerInventory
{
    public partial class IssueMenu : Form
    {
        //Setting up the class
        MessagesClass TheMessagesClass = new MessagesClass();

        public IssueMenu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            //this will open the main menu
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            this.Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            //this will show the about box
            AboutBox AboutBox = new AboutBox();
            AboutBox.ShowDialog();
        }

        private void btnIssueMenu_Click(object sender, EventArgs e)
        {
            Logon.mstrSelectedButton = "ISSUE";
            EnterInventoryInformation EnterInventoryInformation = new EnterInventoryInformation();
            EnterInventoryInformation.Show();
            this.Close();
        }
    }
}
