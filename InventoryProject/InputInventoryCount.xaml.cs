/* Title:           Input Inventory Count
 * Date:            12-11-17
 * Author:          Terrance Holmes
 * 
 * Description:     This form is used to input manual counts */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NewPartNumbersDLL;
using NewEventLogDLL;
using InventoryDLL;
using DataValidationDLL;

namespace InventoryProject
{
    /// <summary>
    /// Interaction logic for InputInventoryCount.xaml
    /// </summary>
    public partial class InputInventoryCount : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        InventoryClass TheInventoryClass = new InventoryClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        FindPartByPartIDDataSet TheFindPartByPartIDDataSet = new FindPartByPartIDDataSet();
        FindPartByPartNumberDataSet TheFindPartByPartNumberDataSet = new FindPartByPartNumberDataSet();
        FindPartByJDEPartNumberDataSet TheFindPartByJDEPartNumberDataSet = new FindPartByJDEPartNumberDataSet();
        FindWarehouseInventoryPartDataSet TheFindWarehouseInventoryPartDataSet = new FindWarehouseInventoryPartDataSet();

        //setting global variables
        int gintPartID;
        string gstrPartNumber;
        string gstrJDEPartNumber;
        string gstrDescription;
        int gintOldQuantity;
        bool gblnNewInventory;
        int gintTransactionID;

        public InputInventoryCount()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;

            try
            {
                intNumberOfRecords = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses.Rows.Count - 1;
                cboSelectWarehouse.Items.Add("Select Warehouse");

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    cboSelectWarehouse.Items.Add(MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intCounter].FirstName);
                }

                cboSelectWarehouse.SelectedIndex = 0;

                btnSearch.IsEnabled = false;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Input Inventory Count // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void cboSelectWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //setting local variables
            int intSelectedIndex;

            try
            {
                intSelectedIndex = cboSelectWarehouse.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    MainWindow.gintWarehouseID = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intSelectedIndex].EmployeeID;

                    btnSearch.IsEnabled = true;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Invenventory Project // Input Inventory Count // cbo Select Warehouse Selection Changed " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int intRecordsReturned;
            bool blnNotInteger;
            bool blnItemFound = false;

            try
            {
                //loading the variables
                gstrPartNumber = txtEnterPartNumber.Text;
                if(gstrPartNumber == "")
                {
                    TheMessagesClass.ErrorMessage("Part Number Not Entered");
                    return;
                }

                blnNotInteger = TheDataValidationClass.VerifyIntegerData(gstrPartNumber);

                if (blnNotInteger == false)
                {
                    gintPartID = Convert.ToInt32(gstrPartNumber);

                    TheFindPartByPartIDDataSet = ThePartNumberClass.FindPartByPartID(gintPartID);

                    intRecordsReturned = TheFindPartByPartIDDataSet.FindPartByPartID.Rows.Count;

                    if(intRecordsReturned > 0)
                    {
                        gstrPartNumber = TheFindPartByPartIDDataSet.FindPartByPartID[0].PartNumber;
                        gstrJDEPartNumber = TheFindPartByPartIDDataSet.FindPartByPartID[0].JDEPartNumber;
                        gstrDescription = TheFindPartByPartIDDataSet.FindPartByPartID[0].PartDescription;
                        blnItemFound = true;
                    }
                }
                if(blnItemFound == false)
                {
                    TheFindPartByPartNumberDataSet = ThePartNumberClass.FindPartByPartNumber(gstrPartNumber);

                    intRecordsReturned = TheFindPartByPartNumberDataSet.FindPartByPartNumber.Rows.Count;

                    if(intRecordsReturned > 0)
                    {
                        gintPartID = TheFindPartByPartNumberDataSet.FindPartByPartNumber[0].PartID;
                        gstrJDEPartNumber = TheFindPartByPartNumberDataSet.FindPartByPartNumber[0].JDEPartNumber;
                        gstrDescription = TheFindPartByPartNumberDataSet.FindPartByPartNumber[0].PartDescription;
                        blnItemFound = true;
                    }
                    else
                    {
                        TheFindPartByJDEPartNumberDataSet = ThePartNumberClass.FindPartByJDEPartNumber(gstrPartNumber);

                        intRecordsReturned = TheFindPartByJDEPartNumberDataSet.FindPartByJDEPartNumber.Rows.Count;

                        if(intRecordsReturned > 0)
                        {
                            gstrJDEPartNumber = gstrPartNumber;
                            gstrPartNumber = TheFindPartByJDEPartNumberDataSet.FindPartByJDEPartNumber[0].PartNumber;
                            gstrDescription = TheFindPartByJDEPartNumberDataSet.FindPartByJDEPartNumber[0].PartDescription;
                            gintPartID = TheFindPartByJDEPartNumberDataSet.FindPartByJDEPartNumber[0].PartID;
                            blnItemFound = true;
                        }
                    }
                }

                if(blnItemFound == false)
                {
                    TheMessagesClass.ErrorMessage("Part Number Does Not Exist");
                    return;
                }

                TheFindWarehouseInventoryPartDataSet = TheInventoryClass.FindWarehouseInventoryPart(gintPartID, MainWindow.gintWarehouseID);

                intRecordsReturned = TheFindWarehouseInventoryPartDataSet.FindWarehouseInventoryPart.Rows.Count;

                if(intRecordsReturned > 0)
                {
                    gintOldQuantity = TheFindWarehouseInventoryPartDataSet.FindWarehouseInventoryPart[0].Quantity;
                    gblnNewInventory = false;
                    gintTransactionID = TheFindWarehouseInventoryPartDataSet.FindWarehouseInventoryPart[0].TransactionID;
                }
                else
                {
                    TheMessagesClass.InformationMessage("No Inventory Was Found For this Part In This Warehouse\nThis Will Be A New Entry");
                    gintOldQuantity = 0;
                    gblnNewInventory = true;
                }

                txtJDEPartNumber.Text = gstrJDEPartNumber;
                txtPartDescription.Text = gstrDescription;
                txtPartNumber.Text = gstrPartNumber;
                txtPartID.Text = Convert.ToString(gintPartID);
                txtOldQuantity.Text = Convert.ToString(gintOldQuantity);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Input Inventory Counts // Search Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            bool blnFatalError;
            int intNewInventory = 0;
            string strValueForValidation;

            try
            {
                strValueForValidation = txtNewQuantity.Text;
                blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage("New Quantity is not an Integer");
                    return;
                }

                intNewInventory = Convert.ToInt32(strValueForValidation);

                if(gblnNewInventory == false)
                {
                    blnFatalError = TheInventoryClass.UpdateInventoryPart(gintTransactionID, intNewInventory);

                    if (blnFatalError == true)
                        throw new Exception();
                }
                if(gblnNewInventory == true)
                {
                    blnFatalError = TheInventoryClass.InsertInventoryPart(gintPartID, intNewInventory, MainWindow.gintWarehouseID);

                    if (blnFatalError == true)
                        throw new Exception();
                }

                TheMessagesClass.InformationMessage("Record Updated");

                txtEnterPartNumber.Text = "";
                txtJDEPartNumber.Text = "";
                txtNewQuantity.Text = "";
                txtOldQuantity.Text = "";
                txtPartDescription.Text = "";
                txtPartID.Text = "";
                txtPartNumber.Text = "";
                cboSelectWarehouse.SelectedIndex = 0;
                btnSearch.IsEnabled = false;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Input Inventory Count // Save Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
