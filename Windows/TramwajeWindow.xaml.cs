using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Logika interakcji dla klasy TramwajeWindow.xaml
    /// </summary>
    public partial class TramwajeWindow : Window
    {
        csprojEntities context = new csprojEntities();
        CollectionViewSource tramwajeViewSource;
        CollectionViewSource trasyViewSource;

        public TramwajeWindow()
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
            context.Tramwaje.Load();
            context.Trasy.Load();
            System.Windows.Data.CollectionViewSource tramwajeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tramwajeViewSource")));
            System.Windows.Data.CollectionViewSource trasyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("trasyViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            tramwajeViewSource.Source = context.Tramwaje.Local;
            trasyViewSource.Source = context.Trasy.Local;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
