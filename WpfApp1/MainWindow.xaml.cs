using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.models;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static BindingList<SuperShrek> eldata;
        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            eldata = new BindingList<SuperShrek>();
            dataGrid1.ItemsSource = eldata;
            dataGrid2.ItemsSource = eldata;

            try { getExcelFile(); }
            catch(Exception)
            {
                MessageBoxResult result = MessageBox.Show("Не удалось открыть файл C:/thrlist.xlsx. Загрузить из интернета?", "Oh shit, I'm sorry", MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        parseHtml();
                        break;
                    case MessageBoxResult.No:

                        break;
                }
            }

        }

        private static void parseHtml()
        {

        }
        private static void getExcelFile()
        {

            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"s:\thrlist.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 3; i <= rowCount; i++)
            {
                eldata.Add(new SuperShrek() { Id = "УБИ."+ Convert.ToInt32(xlRange.Cells[i, 1].Value).ToString("d3"),
                    Name = xlRange.Cells[i, 2].Value,
                    Description= xlRange.Cells[i, 3].Value,
                    Source= xlRange.Cells[i, 4].Value,
                    Obyect= xlRange.Cells[i, 5].Value,
                    Nk= (xlRange.Cells[i, 6].Value==1)?true:false,
                    Nc= (xlRange.Cells[i, 7].Value == 1) ? true : false,
                    Nd= (xlRange.Cells[i, 8].Value == 1) ? true : false });
                
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
