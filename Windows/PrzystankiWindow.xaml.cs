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
    /// Logika interakcji dla klasy PrzystankiWindow.xaml.
    /// Pozwala przeprowadzać operacje CRUD na tabeli przystanki.
    /// </summary>
    public partial class PrzystankiWindow : Window
    {
        csprojEntities context=new csprojEntities();
        CollectionViewSource przystankiViewSource;

        public PrzystankiWindow()
        {
            InitializeComponent();
        }
        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            przystankiViewSource.View.MoveCurrentToNext();
            if (przystankiViewSource.View.IsCurrentAfterLast)
            {
                przystankiViewSource.View.MoveCurrentToPrevious();
            }
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            przystankiViewSource.View.MoveCurrentToPrevious();
            if (przystankiViewSource.View.IsCurrentBeforeFirst)
            {
                przystankiViewSource.View.MoveCurrentToNext();
            }
        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedPrzystanek = przystankiViewSource.View.CurrentItem as Przystanki;

            if (selectedPrzystanek == null)
            {
                MessageBox.Show("Nie wybrano żadnego przystanku");
                return;
            }

            var przystanek = (from c in context.Przystanki
                          where c.id == selectedPrzystanek.id
                          select c).FirstOrDefault();

            if (przystanek != null)
            {
                foreach (var przystanekNaTrasie in context.Przystanki_na_trasie)
                {
                    if (przystanekNaTrasie.id_przystanku == przystanek.id)
                    {
                        MessageBox.Show("Co najmniej jedna trasa korzysta z tego przystanku");
                        return;
                    }
                }
                    context.Przystanki.Remove(przystanek);
            }
            context.SaveChanges();
            przystankiViewSource.View.Refresh();
        }
        private void CommitCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newPrzystanekGrid.IsVisible)
            {

                Przystanki newPrzystanek = new Przystanki
                {
                    id = Utils.getNextId(context.Przystanki),
                    nazwa = addNazwaTextBox.Text,
                };

                if (newPrzystanek.nazwa.Length <= 0)
                {
                    MessageBox.Show("Nazwa nie może być pusta");
                    return;
                }

                context.Przystanki.Local.Add(newPrzystanek);
                przystankiViewSource.View.Refresh();
                przystankiViewSource.View.MoveCurrentTo(newPrzystanek);


                newPrzystanekGrid.Visibility = Visibility.Collapsed;
                existingPrzystanekGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = true;
            }
            else
            {
                Przystanki currentPrzystanek = (Przystanki)przystankiViewSource.View.CurrentItem;

                if (currentPrzystanek == null)
                {
                    MessageBox.Show("Należy wybrać przystanek");
                    return;
                }

                if (currentPrzystanek.nazwa.Length <= 0)
                {
                    MessageBox.Show("Nazwa nie może być pusta");
                    return;
                }

            }

            context.SaveChanges();

        }
        private void SwitchCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingPrzystanekGrid.Visibility == Visibility.Collapsed)
            {
                existingPrzystanekGrid.Visibility = Visibility.Visible;
                newPrzystanekGrid.Visibility = Visibility.Collapsed;
                btnDelete.IsEnabled = true;
            }
            else
            {
                existingPrzystanekGrid.Visibility = Visibility.Collapsed;
                newPrzystanekGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = false;

                // Clear all the text boxes before adding a new customer.
                foreach (var child in newPrzystanekGrid.Children)
                {
                    var tb = child as TextBox;
                    if (tb != null)
                    {
                        tb.Text = "";
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Przystanki.Load();
            przystankiViewSource = ((CollectionViewSource)(this.FindResource("przystankiViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            przystankiViewSource.Source = context.Przystanki.Local; 
        }
    }
}
