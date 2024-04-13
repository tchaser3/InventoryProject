/* Title:           Main Menu
 * Date:            12-7-17
 * Author:          Terry Holmes
 * 
 * Description:     This is the main menu for the program */

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

namespace InventoryProject
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        public MainMenu()
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

        private void btnPrintInventorySheet_Click(object sender, RoutedEventArgs e)
        {
            PrintInventorySheet PrintInventorySheet = new PrintInventorySheet();
            PrintInventorySheet.Show();
            Close();
        }

        private void btnEImportInventoryCount_Click(object sender, RoutedEventArgs e)
        {
            ImportInventoryCount ImportInventoryCount = new ImportInventoryCount();
            ImportInventoryCount.Show();
            Close();
        }

        private void btnInputInventory_Click(object sender, RoutedEventArgs e)
        {
            InputInventoryCount InputInventoryCount = new InputInventoryCount();
            InputInventoryCount.Show();
            Close();
        }

        private void btnInventoryValuation_Click(object sender, RoutedEventArgs e)
        {
            InventoryValuation InventoryValuation = new InventoryValuation();
            InventoryValuation.Show();
            Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox AboutBox = new AboutBox();
            AboutBox.ShowDialog();
        }
    }
}
