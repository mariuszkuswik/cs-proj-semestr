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
using System.Data.Entity;

namespace cs_proj_ostateczny
{
    /// <summary>
    /// Logika interakcji dla klasy KlienciWindow.xaml
    /// </summary>
    public partial class KlienciWindow : Window
    {
        csprojEntities context = new csprojEntities();
        CollectionViewSource klienciViewSource;

        public KlienciWindow()
        {
            InitializeComponent();
        }

        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            context.Klienci.Load();
            System.Windows.Data.CollectionViewSource klienciViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("klienciViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            klienciViewSource.Source = context.Klienci.Local;
        }
    }
}
