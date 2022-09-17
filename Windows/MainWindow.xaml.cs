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

namespace cs_proj_ostateczny
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml.
    /// Pozwala na otwieranie pozostałych okien aplikacji.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PrzystankiButton_Click(object sender, RoutedEventArgs e)
        {
            PrzystankiWindow subWindow = new PrzystankiWindow();
            subWindow.Show();
        }

        private void KlienciButton_Click(object sender, RoutedEventArgs e)
        {
            KlienciWindow subWindow = new KlienciWindow();
            subWindow.Show();

        }

        private void MotorniczyButton_Click(object sender, RoutedEventArgs e)
        {
            MotorniczyWindow subWindow = new MotorniczyWindow();
            subWindow.Show();
        }
        private void TramwajeButton_Click(object sender, RoutedEventArgs e)
        {
            TramwajeWindow subWindow = new TramwajeWindow();
            subWindow.Show();
        }
        private void TrasyButton_Click(object sender, RoutedEventArgs e)
        {
            TrasyWindow subWindow = new TrasyWindow();
            subWindow.Show();
        }
    }
}
