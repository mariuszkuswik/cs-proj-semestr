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
            klienciViewSource.View.MoveCurrentToNext();
            if (klienciViewSource.View.IsCurrentAfterLast)
            {
                klienciViewSource.View.MoveCurrentToPrevious();
            }
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            klienciViewSource.View.MoveCurrentToPrevious();
            if (klienciViewSource.View.IsCurrentBeforeFirst)
            {
                klienciViewSource.View.MoveCurrentToNext();
            }
        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedKlient = klienciViewSource.View.CurrentItem as Klienci;

            if (selectedKlient == null)
            {
                MessageBox.Show("Nie wybrano żadnego klienta");
                return;
            }

            var klient = (from c in context.Klienci
                              where c.id == selectedKlient.id
                              select c).FirstOrDefault();

            if (klient != null)
            {
                context.Klienci.Remove(klient);
            }
            context.SaveChanges();
            klienciViewSource.View.Refresh();
        }
        private void CommitCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newKlienciGrid.IsVisible)
            {

                Klienci newKlient = new Klienci
                {
                    id = Utils.getNextId(context.Klienci),
                    imie = addImieTextBox.Text,
                    nazwisko = addNazwiskoTextBox.Text,
                };

                if (newKlient.imie.Length <= 0)
                {
                    MessageBox.Show("Imię nie może być puste");
                    return;
                }

                if (newKlient.nazwisko.Length <= 0)
                {
                    MessageBox.Show("Nazwisko nie może być puste");
                    return;
                }

                context.Klienci.Local.Add(newKlient);
                klienciViewSource.View.Refresh();
                klienciViewSource.View.MoveCurrentTo(newKlient);


                newKlienciGrid.Visibility = Visibility.Collapsed;
                existingKlienciGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = true;
            }
            else
            {
                Klienci currentKlient = (Klienci)klienciViewSource.View.CurrentItem;

                if (currentKlient == null)
                {
                    MessageBox.Show("Należy wybrać klienta");
                    return;
                }

                if (currentKlient.imie.Length <= 0)
                {
                    MessageBox.Show("Imię nie może być puste");
                    return;
                }

                if (currentKlient.nazwisko.Length <= 0)
                {
                    MessageBox.Show("Nazwisko nie może być puste");
                    return;
                }

            }
            
            context.SaveChanges();

        }
        private void SwitchCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingKlienciGrid.Visibility == Visibility.Collapsed)
            {
                existingKlienciGrid.Visibility = Visibility.Visible;
                newKlienciGrid.Visibility = Visibility.Collapsed;
                btnDelete.IsEnabled = true;
            }
            else
            {
                existingKlienciGrid.Visibility = Visibility.Collapsed;
                newKlienciGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = false;

                // Clear all the text boxes before adding a new customer.
                foreach (var child in newKlienciGrid.Children)
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
            context.Klienci.Load();
            klienciViewSource = ((CollectionViewSource)(this.FindResource("klienciViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            klienciViewSource.Source = context.Klienci.Local;
        }
    }
}
