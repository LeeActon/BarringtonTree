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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BarringtonFile;

namespace Barrington_Tree
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();

            Importer importer = new Importer();

            importer.Load(@"C:\Users\Lee\OneDrive\Documents\Ancestry\barrington.txt");
            importer.ExportGEDCOM(@"C:\Users\Lee\OneDrive\Documents\Ancestry\barrington.ged");
            }
        }
    }
