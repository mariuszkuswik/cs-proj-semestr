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
            tramwajeViewSource.View.MoveCurrentToNext();
            if (tramwajeViewSource.View.IsCurrentAfterLast)
            {
                tramwajeViewSource.View.MoveCurrentToPrevious();
            }
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            tramwajeViewSource.View.MoveCurrentToPrevious();
            if (tramwajeViewSource.View.IsCurrentBeforeFirst)
            {
                tramwajeViewSource.View.MoveCurrentToNext();
            }
        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedTramwaj = tramwajeViewSource.View.CurrentItem as Tramwaje;

            if (selectedTramwaj == null)
            {
                MessageBox.Show("Nie wybrano żadnego tramwaju");
                return;
            }

            var tramwaj = (from c in context.Tramwaje
                              where c.id == selectedTramwaj.id
                              select c).FirstOrDefault();

            if (tramwaj != null)
            {
                foreach (var motorniczy in context.Motorniczy)
                {
                    if (motorniczy.id_tramwaju == tramwaj.id)
                    {
                        MessageBox.Show("Ten tramwaj jest przypisany do motorniczego!");
                        return;
                    }
                }

                context.Tramwaje.Remove(tramwaj);
            }
            context.SaveChanges();
            tramwajeViewSource.View.Refresh();
        }
        private void CommitCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newTramwajeGrid.IsVisible)
            {

                if (addTramwajeComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Należy wybrać trasę");
                    return;
                }

                if (addTramwajeDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Należy wybrać datę");
                    return;
                }

                if (addNumerTextBox.Text == "")
                {
                    MessageBox.Show("Numer nie może być pusty");
                    return;
                }

                Tramwaje newTramwaj = new Tramwaje
                {
                    id = Utils.getNextId(context.Tramwaje),
                    numer = Int32.Parse(addNumerTextBox.Text),
                    data_ostatniego_przeglądu = (DateTime)addTramwajeDatePicker.SelectedDate,
                    id_trasy = Int32.Parse(addTramwajeComboBox.SelectedValue.ToString())
                };

                context.Tramwaje.Local.Add(newTramwaj);
                tramwajeViewSource.View.Refresh();
                tramwajeViewSource.View.MoveCurrentTo(newTramwaj);


                newTramwajeGrid.Visibility = Visibility.Collapsed;
                existingTramwajeGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = true;
            }
            else
            {
                Tramwaje currentTramwaj = (Tramwaje)tramwajeViewSource.View.CurrentItem;

                if (currentTramwaj == null)
                {
                    MessageBox.Show("Należy wybrać tramwaj");
                    return;
                }

                if (existingNumerTextBox.Text == "")
                {
                    MessageBox.Show("Numer nie może być pusty");
                    return;
                }

                var trasa = (from c in context.Trasy
                               where c.id == currentTramwaj.id_trasy
                               select c).FirstOrDefault();
                currentTramwaj.Trasy = trasa;
            }

            context.SaveChanges();
            tramwajeViewSource.View.Refresh();
        }
        private void SwitchCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingTramwajeGrid.Visibility == Visibility.Collapsed)
            {
                existingTramwajeGrid.Visibility = Visibility.Visible;
                newTramwajeGrid.Visibility = Visibility.Collapsed;
                btnAdd.Content = "Dodaj przystanek";
                btnDelete.IsEnabled = true;
            }
            else
            {
                existingTramwajeGrid.Visibility = Visibility.Collapsed;
                newTramwajeGrid.Visibility = Visibility.Visible;
                btnAdd.Content = "Edytuj przystanek";
                btnDelete.IsEnabled = false;

                // Clear all the text boxes before adding a new customer.
                foreach (var child in newTramwajeGrid.Children)
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
            context.Tramwaje.Load();
            context.Trasy.Load();
            tramwajeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tramwajeViewSource")));
            trasyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("trasyViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            tramwajeViewSource.Source = context.Tramwaje.Local;
            trasyViewSource.Source = context.Trasy.Local;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
