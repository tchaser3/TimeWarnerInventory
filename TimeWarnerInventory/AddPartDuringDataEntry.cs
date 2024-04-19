/* Title:           Add Part During Data Entry
 * Date:            5-4-16
 * Author:          Terry Holmes
 *
 * Description      This is used to add a part during the data entry process */

using System;
using System.Windows.Forms;
using MessagesDLL;
using EventLogDLL;
using PartNumberDLL;
using DataValidationDLL;


namespace TimeWarnerInventory
{
    public partial class AddPartDuringDataEntry : Form
    {
        //setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data
        PartNumbersDataSet ThePartNumbersDataSet;

        public AddPartDuringDataEntry()
        {
            InitializeComponent();
        }

        private void AddPartDuringDataEntry_Load(object sender, EventArgs e)
        {
            //setting up the controls
            txtPartNumber.Text = Logon.mstrPartNumber;
            ThePartNumbersDataSet = ThePartNumberClass.GetPartNumbersInfo();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //this will save the record
            //setting local variables
            bool blnFatalError = false;
            string strDescrpition;
            string strPartNumber;

            try
            {
                //data validation
                strDescrpition = txtDescription.Text;
                blnFatalError = TheDataValidationClass.VerifyTextData(strDescrpition);
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage("The Description Was Not Entered");
                    return;
                }

                //loading variables
                strPartNumber = txtPartNumber.Text;

                //creating new table row
                PartNumbersDataSet.partnumbersRow NewTableRow = ThePartNumbersDataSet.partnumbers.NewpartnumbersRow();

                //setting up the row
                NewTableRow.PartID = ThePartNumberClass.CreatePartID();
                NewTableRow.Description = strDescrpition;
                NewTableRow.PartNumber = strPartNumber;
                NewTableRow.TimeWarnerPart = "YES";
                NewTableRow.Active = "YES";
            }
            catch (Exception Ex)
            {
                //message to user
                TheMessagesClass.ErrorMessage(Ex.Message);

                //event log entry
                TheEventLogClass.CreateEventLogEntry("Add Part During Data Entry " + Ex.Message);
            }
        }
    }
}
