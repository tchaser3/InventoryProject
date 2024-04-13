/* Title:           Print Inventory Sheet
 * Date:            12-7-17
 * Author:          Terry Holmes
 * 
 * Description:     This program is used to print the inventory sheets */

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
using NewEventLogDLL;
using InventoryDLL;
using NewPartNumbersDLL;
using System.Printing;
using Microsoft.Win32;

namespace InventoryProject
{
    /// <summary>
    /// Interaction logic for PrintInventorySheet.xaml
    /// </summary>
    public partial class PrintInventorySheet : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        InventoryClass TheInventoryClass = new InventoryClass();
        PartNumberClass ThePartNumberClass = new PartNumberClass();

        FindWarehouseInventoryDataSet TheFindWarehouseInventoryDataSet = new FindWarehouseInventoryDataSet();
        InventorySheetDataSet TheInventorySheetDataSet = new InventorySheetDataSet();
        FindPartByPartNumberDataSet TheFindPartByPartNumberDataSet = new FindPartByPartNumberDataSet();
        
        public PrintInventorySheet()
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
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Print Inventory Sheet // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void cboSelectWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intCounter;
            int intNumberOfRecords;
            string strPartNumber;
            int intPartID;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                intSelectedIndex = cboSelectWarehouse.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    TheInventorySheetDataSet.inventorysheet.Rows.Clear();

                    MainWindow.gintWarehouseID = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intSelectedIndex].EmployeeID;

                    TheFindWarehouseInventoryDataSet = TheInventoryClass.FindWarehouseInventory(MainWindow.gintWarehouseID);

                    intNumberOfRecords = TheFindWarehouseInventoryDataSet.FindWarehouseInventory.Rows.Count - 1;

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        strPartNumber = TheFindWarehouseInventoryDataSet.FindWarehouseInventory[intCounter].PartNumber;

                        TheFindPartByPartNumberDataSet = ThePartNumberClass.FindPartByPartNumber(strPartNumber);

                        intPartID = TheFindPartByPartNumberDataSet.FindPartByPartNumber[0].PartID;

                        InventorySheetDataSet.inventorysheetRow NewInventoryRow = TheInventorySheetDataSet.inventorysheet.NewinventorysheetRow();

                        NewInventoryRow.Description = TheFindWarehouseInventoryDataSet.FindWarehouseInventory[intCounter].PartDescription;
                        NewInventoryRow.JDEPartNumber = TheFindWarehouseInventoryDataSet.FindWarehouseInventory[intCounter].JDEPartNumber;
                        NewInventoryRow.PartID = intPartID;
                        NewInventoryRow.PartNumber = strPartNumber;
                        NewInventoryRow.Quantity = "";

                        TheInventorySheetDataSet.inventorysheet.Rows.Add(NewInventoryRow);
                    }

                    dgrResults.ItemsSource = TheInventorySheetDataSet.inventorysheet;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Print Inventory Sheet // Combo Box Select " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //this will print the report
            int intCurrentRow = 0;
            int intCounter;
            int intColumns;
            int intNumberOfRecords;


            try
            {
                PrintDialog pdInventorySheet = new PrintDialog();

                if (pdInventorySheet.ShowDialog().Value)
                {
                    FlowDocument fdInventorySheet = new FlowDocument();
                    Thickness thickness = new Thickness(50, 50, 50, 50);
                    fdInventorySheet.PagePadding = thickness;

                    pdInventorySheet.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                    //Set Up Table Columns
                    Table InventorySheetTable = new Table();
                    fdInventorySheet.Blocks.Add(InventorySheetTable);
                    InventorySheetTable.CellSpacing = 0;
                    intColumns = TheInventorySheetDataSet.inventorysheet.Columns.Count;
                    fdInventorySheet.ColumnWidth = 10;
                    fdInventorySheet.IsColumnWidthFlexible = false;


                    for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                    {
                        InventorySheetTable.Columns.Add(new TableColumn());
                    }

                    InventorySheetTable.RowGroups.Add(new TableRowGroup());

                    //Title row
                    InventorySheetTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newTableRow = InventorySheetTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Inventory Sheet"))));
                    newTableRow.Cells[0].FontSize = 30;
                    newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                    newTableRow.Cells[0].ColumnSpan = intColumns;
                    newTableRow.Cells[0].TextAlignment = TextAlignment.Center;
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 10);

                    //Header Row
                    InventorySheetTable.RowGroups[0].Rows.Add(new TableRow());
                    intCurrentRow++;
                    newTableRow = InventorySheetTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Part ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Part Number"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("JDE Part Number"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Description"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Quantity"))));
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 10);
                    newTableRow.Cells[0].ColumnSpan = 1;
                    newTableRow.Cells[1].ColumnSpan = 1;
                    newTableRow.Cells[2].ColumnSpan = 1;
                    newTableRow.Cells[3].ColumnSpan = 2;
                    newTableRow.Cells[4].ColumnSpan = 1;

                    //Format Header Row
                    for (intCounter = 0; intCounter < intColumns; intCounter++)
                    {
                        newTableRow.Cells[intCounter].FontSize = 26;
                        newTableRow.Cells[intCounter].FontFamily = new FontFamily("Times New Roman");
                        newTableRow.Cells[intCounter].BorderBrush = Brushes.Black;
                        newTableRow.Cells[intCounter].TextAlignment = TextAlignment.Center;
                        newTableRow.Cells[intCounter].BorderThickness = new Thickness();

                    }

                    intNumberOfRecords = TheInventorySheetDataSet.inventorysheet.Rows.Count;

                    //Data, Format Data

                    for (int intReportRowCounter = 0; intReportRowCounter < intNumberOfRecords; intReportRowCounter++)
                    {
                        InventorySheetTable.RowGroups[0].Rows.Add(new TableRow());
                        intCurrentRow++;
                        newTableRow = InventorySheetTable.RowGroups[0].Rows[intCurrentRow];
                        for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                        {
                            newTableRow.Cells.Add(new TableCell(new Paragraph(new Run(TheInventorySheetDataSet.inventorysheet[intReportRowCounter][intColumnCounter].ToString()))));

                            newTableRow.Cells[intColumnCounter].FontSize = 20;
                            newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                            newTableRow.Cells[intColumnCounter].BorderBrush = Brushes.LightSteelBlue;
                            newTableRow.Cells[intColumnCounter].BorderThickness = new Thickness(0, 0, 0, 1);
                            newTableRow.Cells[intColumnCounter].TextAlignment = TextAlignment.Center;
                            if (intColumnCounter == 3)
                            {
                                newTableRow.Cells[intColumnCounter].ColumnSpan = 2;
                            }

                        }
                    }

                    //Set up page and print
                    fdInventorySheet.ColumnWidth = pdInventorySheet.PrintableAreaWidth;
                    fdInventorySheet.PageHeight = pdInventorySheet.PrintableAreaHeight;
                    fdInventorySheet.PageWidth = pdInventorySheet.PrintableAreaWidth;
                    pdInventorySheet.PrintDocument(((IDocumentPaginatorSource)fdInventorySheet).DocumentPaginator, "Inventory Sheet");
                    intCurrentRow = 0;

                }
            }
            catch (Exception Ex)
            {
                TheMessagesClass.ErrorMessage(Ex.ToString());

                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Print Inventory Sheet // Print Button " + Ex.Message);
            }
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            int intRowCounter;
            int intRowNumberOfRecords;
            int intColumnCounter;
            int intColumnNumberOfRecords;

            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "InventorySheet";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;
                intRowNumberOfRecords = TheInventorySheetDataSet.inventorysheet.Rows.Count;
                intColumnNumberOfRecords = TheInventorySheetDataSet.inventorysheet.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = TheInventorySheetDataSet.inventorysheet.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = TheInventorySheetDataSet.inventorysheet.Rows[intRowCounter][intColumnCounter].ToString();

                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                //Getting the location and file name of the excel to save from user. 
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;

                saveDialog.ShowDialog();

                workbook.SaveAs(saveDialog.FileName);
                MessageBox.Show("Export Successful");

            }
            catch (System.Exception ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Print Inventory Sheet // Export to Excel " + ex.Message);

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }
    }
}
