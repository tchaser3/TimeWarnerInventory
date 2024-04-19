﻿/* Title:           Logon
 * Datew:           5-1-16
 * Author:          Terry Holmes
 *
 * Description:     This is the the Log On Form */

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
using EmployeeDLL;
using EventLogDLL;
using KeyWordDLL;
using DataValidationDLL;

namespace TimeWarnerInventory
{
    public partial class Logon : Form
    {
        //setting up the class
        MessagesClass TheMessagesClass = new MessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        PleaseWait PleaseWait = new PleaseWait();
        EmployeeDataSet TheEmployeeDataSet;
        KeyWordClass TheKeyWordClass = new KeyWordClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting global variables
        public static string mstrErrorMessage;
        public static int mintWarehouseEmployeeID;
        public static int mintEmployeeID;
        public static int mintPartsWarehouseID;
        public static string mstrLastTransactionSummary;
        public static string mstrSelectedButton;
        public static int mintInternalProjectID;
        public static string mstrTWCProjectID;
        public static string mstrPartNumber;
        public static string mintQuantity;
        public static string mstrMSRNumber;
        public static string mstrWarehouse;
        public static DateTime mdatTransactionDate;
        int mintNumberOfMisses;
        public static string mstrEmployeeGroup;
        public static string mstrLastName;
        public static string mstrFirstName;

        public Logon()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //This will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void Logon_Load(object sender, EventArgs e)
        {
            //setting local variables
            bool blnFatalError = false;

            PleaseWait.Show();

            mintNumberOfMisses = 0;

            //beginning functions
            blnFatalError = LoadComboBox();

            PleaseWait.Hide();

            if(blnFatalError == true)
            {
                //message to user
                TheMessagesClass.ErrorMessage(mstrErrorMessage);

                //event log entry
                TheEventLogClass.CreateEventLogEntry("TWC Inventory Logon " + mstrErrorMessage);

                btnLogon.Enabled = false;
            }
        }
        private bool LoadComboBox()
        {
            //setting local variables
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;
            bool blnKeyWordNotFound;
            string strFirstName;
            string strLastName;
            string strActive;

            //setting up the data
            TheEmployeeDataSet = TheEmployeeClass.GetEmployeeInfo();

            //getting the number of records
            intNumberOfRecords = TheEmployeeClass.EmployeeNumberOfRecords();
            cboWarehouse.Items.Add("SELECT");

            //loop
            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                //getting the variables
                strLastName = Convert.ToString(TheEmployeeDataSet.employees.Rows[intCounter][2]).ToUpper();
                strFirstName = Convert.ToString(TheEmployeeDataSet.employees.Rows[intCounter][1]).ToUpper();
                strActive = Convert.ToString(TheEmployeeDataSet.employees.Rows[intCounter][5]).ToUpper();

                //if statements
                if(strLastName == "PARTS")
                    if(strActive == "YES")
                        {
                            blnKeyWordNotFound = TheKeyWordClass.FindKeyWord("TWC", strFirstName);

                            if(blnKeyWordNotFound == false)
                            {
                                cboWarehouse.Items.Add(strFirstName);
                            }
                        }
            }

            //setting the selected index
            cboWarehouse.SelectedIndex = 0;

            //return to calling method
            return blnFatalError;
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            //setting local variables
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            string strValueForValidation;
            bool blnInformationVerified;
            
            //beginning data validation
            if(cboWarehouse.Text == "SELECT")
            {
                blnThereIsAProblem = true;
                strErrorMessage = strErrorMessage + "The Warehouse Was Not Selected\n";
            }
            strValueForValidation = txtEmployeeID.Text;
            blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
            if(blnFatalError == true)
            {
                blnThereIsAProblem = true;
                strErrorMessage = strErrorMessage + "The Value for Employee ID is not an Integer\n";
            }
            mstrLastName = txtLogonLastName.Text;
            blnFatalError = TheDataValidationClass.VerifyTextData(mstrLastName);
            if(blnFatalError == true)
            {
                blnThereIsAProblem = true;
                strErrorMessage = strErrorMessage + "The Last Name Was Not Entered\n";
            }
            if(blnThereIsAProblem == true)
            {
                TheMessagesClass.ErrorMessage(strErrorMessage);
                return;
            }
            //checking employee login
            mintWarehouseEmployeeID = Convert.ToInt32(txtEmployeeID.Text);

            //checking logged in
            blnInformationVerified = TheEmployeeClass.VerifyLogon(mintWarehouseEmployeeID, mstrLastName);

            if(blnInformationVerified == true)
            {
                //getting the information
                mstrEmployeeGroup = TheEmployeeClass.FindEmployeeGroup(mintWarehouseEmployeeID);

                //getting the first name
                mstrFirstName = TheEmployeeClass.FindEmployeeFirstNamewithID(mintWarehouseEmployeeID);

                //getting the warehouse id
                mintPartsWarehouseID = TheEmployeeClass.GetEmployeeID(cboWarehouse.Text, "PARTS");

                TheMessagesClass.InformationMessage(mstrFirstName + " " + mstrLastName + " Has Successfully Logged Into the TWC Inventory Program");

                MainMenu MainMenu = new MainMenu();
                MainMenu.Show();
                this.Hide();
            }
            else
            {
                //message to user
                TheMessagesClass.InformationMessage("The Login Information Is Incorrect");

                //incrementing the number of misses
                mintNumberOfMisses++;

                if(mintNumberOfMisses == 3)
                {
                    TheMessagesClass.ErrorMessage("There Have Been Three Attempts To Log In And Failed\n The Application Will Now Close");

                    TheEventLogClass.CreateEventLogEntry("Three Attemps Have Been Made to the Time Warner Inventory Program");

                    Application.Exit();
                }
            }
        }
    }
}
