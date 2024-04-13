/* Title:           Inventory Evaluation
 * Date:            12-27-17
 * Author:          Terry Holmes
 * 
 * Description:     This form is used for Inventory Evaluation */

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
using InventoryValuationDLL;
using NewEventLogDLL;
using KeyWordDLL;
using Microsoft.Win32;

namespace InventoryProject
{
    /// <summary>
    /// Interaction logic for InventoryValuation.xaml
    /// </summary>
    public partial class InventoryValuation : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        InventoryValuationClass TheInventoryValuationClass = new InventoryValuationClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        KeyWordClass TheKeyWordClass = new KeyWordClass();

        FindWarehouseValueDataSet TheFindWarehouseValueDataSet = new FindWarehouseValueDataSet();

        WarehouseValueDataSet TheWarehouseValueDataSetr = new WarehouseValueDataSet();

        decimal gdecTotalValue;
        
        public InventoryValuation()
        {
            InitializeComponent();
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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int intWarehouseCounter;
            int intWarehouseNumberOfRecords;
            int intInventoryCounter;
            int intInventoryNumberOfRecords;
            int intWarehouseID;
            decimal decWarehouseValue;
            bool blnKeyWordNotFound;
            string strWarehouse;
            decimal decTempNumber;

           gdecTotalValue = 0;

            try
            {
                intWarehouseNumberOfRecords = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses.Rows.Count - 1;

                for(intWarehouseCounter = 0; intWarehouseCounter <= intWarehouseNumberOfRecords; intWarehouseCounter++)
                {
                    intWarehouseID = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intWarehouseCounter].EmployeeID;
                    strWarehouse = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intWarehouseCounter].FirstName;

                    blnKeyWordNotFound = true;
                    blnKeyWordNotFound = TheKeyWordClass.FindKeyWord("BJC", strWarehouse);

                    if(blnKeyWordNotFound == true)
                    {
                        blnKeyWordNotFound = TheKeyWordClass.FindKeyWord("JH", strWarehouse);
                    }

                    if(blnKeyWordNotFound == false)
                    {
                        TheFindWarehouseValueDataSet = TheInventoryValuationClass.FindWarehouseValue(intWarehouseID);

                        intInventoryNumberOfRecords = TheFindWarehouseValueDataSet.FindWarehouseValue.Rows.Count - 1;
                        decWarehouseValue = 0;

                        for (intInventoryCounter = 0; intInventoryCounter <= intInventoryNumberOfRecords; intInventoryCounter++)
                        {
                            decTempNumber = Convert.ToDecimal(TheFindWarehouseValueDataSet.FindWarehouseValue[intInventoryCounter].TotalPrice);
                            decTempNumber = Math.Round(decTempNumber, 2);
                            decTempNumber = Decimal.Parse(decTempNumber.ToString("0.00"));
                            decWarehouseValue += decTempNumber;
                            gdecTotalValue += decTempNumber;
                        }

                        WarehouseValueDataSet.warehousevalueRow NewWarehouseRow = TheWarehouseValueDataSetr.warehousevalue.NewwarehousevalueRow();

                        NewWarehouseRow.WarehouseID = intWarehouseID;
                        NewWarehouseRow.Warehouse = MainWindow.TheFindPartsWarehouseDataSet.FindPartsWarehouses[intWarehouseCounter].FirstName;
                        NewWarehouseRow.WarehouseValue = decWarehouseValue;

                        TheWarehouseValueDataSetr.warehousevalue.Rows.Add(NewWarehouseRow);
                    }
                    
                }

                dgrResults.ItemsSource = TheWarehouseValueDataSetr.warehousevalue;
                txtTotalValue.Text = Convert.ToString(gdecTotalValue);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Inventory Valuation // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
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
                PrintDialog pdInventoryValue = new PrintDialog();

                if (pdInventoryValue.ShowDialog().Value)
                {
                    FlowDocument fdInventoryValue = new FlowDocument();
                    Thickness thickness = new Thickness(50, 50, 50, 50);
                    fdInventoryValue.PagePadding = thickness;

                    //Set Up Table Columns
                    Table InventoryValueTable = new Table();
                    fdInventoryValue.Blocks.Add(InventoryValueTable);
                    InventoryValueTable.CellSpacing = 0;
                    intColumns = TheWarehouseValueDataSetr.warehousevalue.Columns.Count;
                    fdInventoryValue.ColumnWidth = 10;
                    fdInventoryValue.IsColumnWidthFlexible = false;


                    for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                    {
                        InventoryValueTable.Columns.Add(new TableColumn());
                    }

                    InventoryValueTable.RowGroups.Add(new TableRowGroup());

                    //Title row
                    InventoryValueTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newTableRow = InventoryValueTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Blue Jay Communications Warehouse Value"))));
                    newTableRow.Cells[0].FontSize = 30;
                    newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                    newTableRow.Cells[0].ColumnSpan = intColumns;
                    newTableRow.Cells[0].TextAlignment = TextAlignment.Center;
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 10);

                    //Header Row
                    InventoryValueTable.RowGroups[0].Rows.Add(new TableRow());
                    intCurrentRow++;
                    newTableRow = InventoryValueTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Warehouse ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Warehouse"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Warehouse Value"))));
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 10);

                    //Format Header Row
                    for (intCounter = 0; intCounter < intColumns; intCounter++)
                    {
                        newTableRow.Cells[intCounter].FontSize = 26;
                        newTableRow.Cells[intCounter].FontFamily = new FontFamily("Times New Roman");
                        newTableRow.Cells[intCounter].BorderBrush = Brushes.Black;
                        newTableRow.Cells[intCounter].TextAlignment = TextAlignment.Center;
                        newTableRow.Cells[intCounter].BorderThickness = new Thickness();

                    }

                    intNumberOfRecords = TheWarehouseValueDataSetr.warehousevalue.Rows.Count;

                    //Data, Format Data

                    for (int intReportRowCounter = 0; intReportRowCounter < intNumberOfRecords; intReportRowCounter++)
                    {
                        InventoryValueTable.RowGroups[0].Rows.Add(new TableRow());
                        intCurrentRow++;
                        newTableRow = InventoryValueTable.RowGroups[0].Rows[intCurrentRow];
                        for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                        {
                            newTableRow.Cells.Add(new TableCell(new Paragraph(new Run(TheWarehouseValueDataSetr.warehousevalue[intReportRowCounter][intColumnCounter].ToString()))));

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

                    Paragraph Title = new Paragraph(new Run("Total Blue Jay Inventory Value:    " + Convert.ToString(gdecTotalValue)));
                    Title.FontSize = 25;
                    Title.TextAlignment = TextAlignment.Right;
                    Title.LineHeight = 3;
                    fdInventoryValue.Blocks.Add(Title);

                    //Set up page and print
                    fdInventoryValue.ColumnWidth = pdInventoryValue.PrintableAreaWidth;
                    fdInventoryValue.PageHeight = pdInventoryValue.PrintableAreaHeight;
                    fdInventoryValue.PageWidth = pdInventoryValue.PrintableAreaWidth;
                    pdInventoryValue.PrintDocument(((IDocumentPaginatorSource)fdInventoryValue).DocumentPaginator, "Warehouse Valuation");
                    intCurrentRow = 0;

                }
            }
            catch (Exception Ex)
            {
                TheMessagesClass.ErrorMessage(Ex.ToString());

                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Inventory Valuation // Print Button " + Ex.Message);
            }
        }

        private void btnExportExcelSpreadsheet_Click(object sender, RoutedEventArgs e)
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

                worksheet.Name = "WarehouseValuation";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;
                intRowNumberOfRecords = TheWarehouseValueDataSetr.warehousevalue.Rows.Count;
                intColumnNumberOfRecords = TheWarehouseValueDataSetr.warehousevalue.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = TheWarehouseValueDataSetr.warehousevalue.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = TheWarehouseValueDataSetr.warehousevalue.Rows[intRowCounter][intColumnCounter].ToString();

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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Inventory Project // Inventory Valuation // Export to Excel " + ex.Message);

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
