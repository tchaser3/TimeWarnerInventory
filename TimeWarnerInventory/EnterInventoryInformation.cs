/* Title:           Enter Inventory Information
 * Date:            5-4-16
 * Author:          Terry Holmes
 *
 * Description:     This form is used for all data entry */

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
using BOMPartsDLL;
using CreateIDDLL;
using DataValidationDLL;
using EmployeeDLL;
using EventLogDLL;
using InventoryDLL;
using IssuedPartsDLL;
using LastTransactionDLL;
using PartNumberDLL;
using ReceivedMaterialDLL;

namespace TimeWarnerInventory
{
    public partial class EnterInventoryInformation : Form
    {
        //setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();
        BOMPartsClass TheBOMPartsClass = new BOMPartsClass();
        CreateIDClass TheCreateIDClass = new CreateIDClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        InventoryClass TheInventoryClass = new InventoryClass();
        IssuedPartsClass TheIssuedPartsClass = new IssuedPartsClass();
        LastTransactionClass TheLastTransactionClass = new LastTransactionClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();
        ReceivedMaterialClass TheReceivedMaterialClass = new ReceivedMaterialClass();

        //setting up the data
        ReceivedPartsDataSet TheReceivedPartsDataSet;
        IssuedPartsDataSet TheIssuedPartsDataSet;
        BOMPartsDataSet TheBOMPartsDataSet;

        public EnterInventoryInformation()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void btnReceiveMenu_Click(object sender, EventArgs e)
        {
            //determining which menu to go to
            if (Logon.mstrSelectedButton == "RECEIVE")
            {
                ReceiveMenu ReceiveMenu = new ReceiveMenu();
                ReceiveMenu.Show();
            }
            else if (Logon.mstrSelectedButton == "ISSUE")
            {
                IssueMenu IssueMenu = new IssueMenu();
                IssueMenu.Show();
            }
            else if (Logon.mstrSelectedButton == "BOM")
            {
                BOMMenu BOMMenu = new BOMMenu();
                BOMMenu.Show();
            }

            //this will close the form
            this.Close();
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            this.Close();
        }

        private void EnterInventoryInformation_Load(object sender, EventArgs e)
        {
            //this is form load
            if (Logon.mstrSelectedButton == "RECEIVE")
            {
                TheReceivedPartsDataSet = TheReceivedMaterialClass.GetReceivedPartsInfo();
                btnReceiveMenu.Text = "Receive Menu";
                lblEnterInventory.Text = "Enter MSR Information and Receive Material"; 
            }
            else if (Logon.mstrSelectedButton == "ISSUE")
            {
                TheIssuedPartsDataSet = TheIssuedPartsClass.GetIssuedPartsInfo();
                btnReceiveMenu.Text = "Issue Menu";
                lblEnterInventory.Text = "Enter Material Sheet Information and Issue Material";
            }
            else if (Logon.mstrSelectedButton == "BOM")
            {
                TheBOMPartsDataSet = TheBOMPartsClass.GetBOMPartsInfo();
                btnReceiveMenu.Text = "BOM Menu";
                lblEnterInventory.Text = "Enter BOM Information";
            }

            //create grid
            dgvLastTransactions.ColumnCount = 7;
            dgvLastTransactions.Columns[0].Name = "Transaction ID";
            dgvLastTransactions.Columns[0].Width = 75;
            dgvLastTransactions.Columns[1].Name = "Date";
            dgvLastTransactions.Columns[1].Width = 100;
            dgvLastTransactions.Columns[2].Name = "Project ID";
            dgvLastTransactions.Columns[2].Width = 100;
            dgvLastTransactions.Columns[3].Name = "MSR Number";
            dgvLastTransactions.Columns[3].Width = 100;
            dgvLastTransactions.Columns[4].Name = "Part Number";
            dgvLastTransactions.Columns[4].Width = 100;
            dgvLastTransactions.Columns[5].Name = "QTY";
            dgvLastTransactions.Columns[5].Width = 50;
            dgvLastTransactions.Columns[6].Name = "Warehouse ID";
            dgvLastTransactions.Columns[6].Width = 75;

            btnAdd.PerformClick();           
       
        }
        private void LoadControls()
        {
            //setting local variables
            int intTransactionID;

            //getting the id
            intTransactionID = TheCreateIDClass.CreateInventoryID();

            txtTransactionID.Text = Convert.ToString(intTransactionID);
            txtWarehouseID.Text = Convert.ToString(Logon.mintPartsWarehouseID);
            txtDate.Text = Convert.ToString(DateTime.Now);
            txtDataEntryComplete.Text = "NO";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //setting local variables
            bool blnFatalError = false;
            bool blnItemFound = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            string strValueForValidation;

            if(btnAdd.Text == "Add")
            {
                LoadControls();
                btnAdd.Text = "Save";
            }
            else
            {
                try
                {
                    //beginning data validation
                    strValueForValidation = txtDate.Text;
                    blnFatalError = TheDataValidationClass.VerifyDateData(strValueForValidation);
                    if(blnFatalError == true)
                    {
                        blnThereIsAProblem = true;
                        strErrorMessage = strErrorMessage + "The Information for Date is not a Date\n";
                    }
                    else
                    {
                        Logon.mdatTransactionDate = Convert.ToDateTime(txtDate.Text);
                    }
                    
                    //checking project information and loading variables
                    Logon.mstrTWCProjectID = txtProjectID.Text;
                    Logon.mstrMSRNumber = txtMSRNumber.Text;

                    //if statements
                    if ((Logon.mstrTWCProjectID == "") && (Logon.mstrMSRNumber != ""))
                    {
                        TheMessagesClass.InformationMessage("Mother Fucker");
                    }
                    else if((Logon.mstrTWCProjectID != "") && (Logon.mstrMSRNumber == ""))
                    {
                        TheMessagesClass.InformationMessage("Suck My Dick");
                    }
                    else if((Logon.mstrTWCProjectID != "") && (Logon.mstrMSRNumber != ""))
                    {
                        TheMessagesClass.InformationMessage("Lick my balls");
                    }
                    else
                    {
                        blnThereIsAProblem = true;
                        strErrorMessage = strErrorMessage + "Neither the Project ID or MSR Number Were Provided About\n";
                    }


                    //message to the user
                    if(blnThereIsAProblem == true)
                    {
                        TheMessagesClass.ErrorMessage(strErrorMessage);
                        return;
                    }

                }
                catch (Exception Ex)
                {
                    //Event Log Entry
                    TheEventLogClass.CreateEventLogEntry("Enter Inventory Information " + Ex.Message);

                    //message to user
                    TheMessagesClass.ErrorMessage(Ex.Message);
                }
            }
        }
    }
}
