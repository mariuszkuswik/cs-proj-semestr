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
using System.ComponentModel;
using System.Collections.Specialized;

namespace cs_proj_ostateczny
{
    /// <summary>
    /// Logika interakcji dla klasy TrasyWindow.xaml
    /// </summary>
    public partial class TrasyWindow : Window
    {
        csprojEntities context = new csprojEntities();
        CollectionViewSource trasyViewSource;
        CollectionViewSource przystankiNaTrasieViewSource;
        CollectionViewSource przystankiViewSource;

        public TrasyWindow()
        {
            InitializeComponent();
        }
        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            trasyViewSource.View.MoveCurrentToNext();
            if (trasyViewSource.View.IsCurrentAfterLast)
            {
                trasyViewSource.View.MoveCurrentToPrevious();
            }
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void SwitchCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void CommitCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Przystanki.Load();
            przystankiViewSource = ((CollectionViewSource)(this.FindResource("przystankiViewSource")));
            przystankiViewSource.Source = context.Przystanki.Local;

            context.Przystanki_na_trasie.Load();
            przystankiNaTrasieViewSource = ((CollectionViewSource)(this.FindResource("przystankiNaTrasieViewSource")));
            przystankiNaTrasieViewSource.Source = context.Przystanki_na_trasie.Local;

            context.Trasy.Load();
            trasyViewSource = ((CollectionViewSource)(this.FindResource("trasyViewSource")));
            trasyViewSource.Source = context.Trasy.Local;
            

            przystankiNaTrasieViewSource.Filter += new FilterEventHandler(filterByPrzystanekId);
        }

        private void filterByPrzystanekId(object sender, FilterEventArgs e)
        {
            var obj = e.Item as Przystanki_na_trasie;
            var currentPrzystanek = trasyViewSource.View.CurrentItem as Trasy;
            if (currentPrzystanek != null)
            {
                if (obj != null)
                {
                    if (obj.id_trasy == currentPrzystanek.id)
                        e.Accepted = true;
                    else
                        e.Accepted = false;
                }
            }
        }

        private void trasyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            przystankiNaTrasieViewSource.View.Refresh();
        }
    }
}
