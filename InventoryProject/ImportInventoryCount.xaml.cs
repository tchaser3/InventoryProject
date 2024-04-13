/* Title:           Import Inventory Count
 * Date:            12-8-17
 * Author:          Terry Holmes
 * 
 * Description:     This form is used to import the Inventory Spreadsheet */

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
using InventoryDLL;
using NewEventLogDLL;
using NewPartNumbersDLL;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace InventoryProject
{
    /// <summary>
    /// Interaction logic for ImportInventoryCount.xaml
    /// </summary>
    public partial class ImportInventoryCount : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        InventoryClass TheInventoryClass = new InventoryClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();

        //setting up the data set
        FindWarehouseInventoryDataSet TheFindWarehouseInventoryDataSet = new FindWarehouseInventoryDataSet();
        FindPartByPartNumberDataSet TheFindPartByPartNumberDataSet = new FindPartByPartNumberDataSet();
        ImportInventoryDataSet TheImportInventoryDataSet = new ImportInventoryDataSet();
        FindWarehouseInventoryPartDataSet TheFindWarehouseInventoryPartDataSet = new FindWarehouseInventoryPartDataSet();

        public ImportInventoryCount()
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
                btnProcess.IsEnabled = false;
                btnImportExcelSheet.IsEnabled = false;

                intNumberOfRecords = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses.Rows.Count - 1;

                cboSelectWarehouse.Items.Add("Select Warehouse");

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    cboSelectWarehouse.Items.Add(MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intCounter].FirstName);
                }

                cboSelectWarehouse.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Import Inventory Count // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void cboSelectWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            try
            {
                intSelectedIndex = cboSelectWarehouse.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    MainWindow.gintWarehouseID = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intSelectedIndex].EmployeeID;

                    TheFindWarehouseInventoryDataSet = TheInventoryClass.FindWarehouseInventory(MainWindow.gintWarehouseID);

                    btnImportExcelSheet.IsEnabled = true;
                }
                else
                {
                    btnProcess.IsEnabled = false;
                    btnImportExcelSheet.IsEnabled = false;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Import Inventory Count // cbo Select Warehouse Changed " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnImportExcelSheet_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application xlDropOrder;
            Excel.Workbook xlDropBook;
            Excel.Worksheet xlDropSheet;
            Excel.Range range;

            string strInformation = "";
            int intColumnRange = 0;
            int intCounter;
            int intNumberOfRecords;
            int intPartID;
            
            try
            {
                TheImportInventoryDataSet.inventory.Rows.Clear();

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "Document"; // Default file name
                dlg.DefaultExt = ".xlsx"; // Default file extension
                dlg.Filter = "Excel (.xlsx)|*.xlsx"; // Filter files by extension

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                }

                PleaseWait PleaseWait = new PleaseWait();
                PleaseWait.Show();

                xlDropOrder = new Excel.Application();
                xlDropBook = xlDropOrder.Workbooks.Open(dlg.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlDropSheet = (Excel.Worksheet)xlDropOrder.Worksheets.get_Item(1);

                range = xlDropSheet.UsedRange;
                intNumberOfRecords = range.Rows.Count - 1;
                intColumnRange = range.Columns.Count;

                for (intCounter = 2; intCounter <= intNumberOfRecords; intCounter++)
                {
                    ImportInventoryDataSet.inventoryRow NewInventoryRow = TheImportInventoryDataSet.inventory.NewinventoryRow();

                    string strTesting = Convert.ToString((range.Cells[intCounter, 1] as Excel.Range).Value2);

                    intPartID = Convert.ToInt32((range.Cells[intCounter, 1] as Excel.Range).Value2);

                    TheFindWarehouseInventoryPartDataSet = TheInventoryClass.FindWarehouseInventoryPart(intPartID, MainWindow.gintWarehouseID);

                    NewInventoryRow.TransactionID = TheFindWarehouseInventoryPartDataSet.FindWarehouseInventoryPart[0].TransactionID;
                    NewInventoryRow.PartID = intPartID;
                    NewInventoryRow.OldQuantity = TheFindWarehouseInventoryPartDataSet.FindWarehouseInventoryPart[0].Quantity;
                    NewInventoryRow.PartNumber = Convert.ToString((range.Cells[intCounter, 2] as Excel.Range).Value2);
                    NewInventoryRow.JDEPartNumber = Convert.ToString((range.Cells[intCounter, 3] as Excel.Range).Value2);
                    NewInventoryRow.Description = Convert.ToString((range.Cells[intCounter, 4] as Excel.Range).Value2);
                    NewInventoryRow.NewQuantity = Convert.ToInt32((range.Cells[intCounter, 5] as Excel.Range).Value2);
                    NewInventoryRow.Accept = false;

                    TheImportInventoryDataSet.inventory.Rows.Add(NewInventoryRow);
                }

                PleaseWait.Close();
                dgrResults.ItemsSource = TheImportInventoryDataSet.inventory;
                btnProcess.IsEnabled = true;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Import Inventory Count // Select Spreadsheet Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //this will process the import
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            int intTransactionID;
            int intQuantity;
            bool blnFatalError;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                intNumberOfRecords = TheImportInventoryDataSet.inventory.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    if (TheImportInventoryDataSet.inventory[intCounter].Accept == true)
                    {
                        intTransactionID = TheImportInventoryDataSet.inventory[intCounter].TransactionID;
                        intQuantity = TheImportInventoryDataSet.inventory[intCounter].NewQuantity;

                        blnFatalError = TheInventoryClass.UpdateInventoryPart(intTransactionID, intQuantity);

                        if (blnFatalError == true)
                            throw new Exception();
                    }
                }

                TheImportInventoryDataSet.inventory.Rows.Clear();
                dgrResults.ItemsSource = TheImportInventoryDataSet.inventory;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Import Inventory Count // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
    }
}
