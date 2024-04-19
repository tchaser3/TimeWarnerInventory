/* Title:           Main Menu
 * Date:            5-2-16
 * Author:          Terry Holmes
 *
 * Description:     This is the main menu */

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
    public partial class MainMenu : Form
    {
        //Setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //This will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void btnReceiveInventory_Click(object sender, EventArgs e)
        {
            //this shows the receive menu
            ReceiveMenu ReceiveMenu = new ReceiveMenu();
            ReceiveMenu.Show();
            this.Close();
        }

        private void btnIssueInventory_Click(object sender, EventArgs e)
        {
            //this will show the issue menu
            IssueMenu IssueMenu = new IssueMenu();
            IssueMenu.Show();
            this.Close();
        }

        private void btnBOMInformation_Click(object sender, EventArgs e)
        {
            BOMMenu BOMMenu = new BOMMenu();
            BOMMenu.Show();
            this.Close();
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
        {
            TheMessagesClass.UnderDevelopment();
        }

        private void btnViewProjects_Click(object sender, EventArgs e)
        {
            TheMessagesClass.UnderDevelopment();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            TheMessagesClass.UnderDevelopment();
        }

        private void btnAdministrativeMenu_Click(object sender, EventArgs e)
        {
            TheMessagesClass.UnderDevelopment();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            //this will open the about box
            AboutBox AboutBox = new AboutBox();
            AboutBox.ShowDialog();
        }
    }
}
