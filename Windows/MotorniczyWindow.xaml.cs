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
    /// Logika interakcji dla klasy MotorniczyWindow.xaml.
    /// Pozwala przeprowadzać operacje CRUD na tabeli motorniczy.
    /// </summary>
    public partial class MotorniczyWindow : Window
    {
        csprojEntities context = new csprojEntities();
        CollectionViewSource motorniczyViewSource;
        CollectionViewSource tramwajeViewSource;
        public MotorniczyWindow()
        {
            InitializeComponent();
        }

        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            motorniczyViewSource.View.MoveCurrentToNext();
            if (motorniczyViewSource.View.IsCurrentAfterLast)
            {
                motorniczyViewSource.View.MoveCurrentToPrevious();
            }
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            motorniczyViewSource.View.MoveCurrentToPrevious();
            if (motorniczyViewSource.View.IsCurrentBeforeFirst)
            {
                motorniczyViewSource.View.MoveCurrentToNext();
            }
        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedMotorniczy = motorniczyViewSource.View.CurrentItem as Motorniczy;

            if (selectedMotorniczy == null)
            {
                MessageBox.Show("Nie wybrano żadnego motorniczego");
                return;
            }

            var motorniczy = (from c in context.Motorniczy
                              where c.id == selectedMotorniczy.id
                              select c).FirstOrDefault();

            if (motorniczy != null)
            {
                context.Motorniczy.Remove(motorniczy);
            }
            context.SaveChanges();
            motorniczyViewSource.View.Refresh();
        }

        private void CommitCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newMotorniczyGrid.IsVisible)
            {

                if (addMotorniczyComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Należy wybrać tramwaj");
                    return;
                }

                Motorniczy newMotorniczy = new Motorniczy
                {
                    id = Utils.getNextId(context.Motorniczy),
                    imie = addImieTextBox.Text,
                    nazwisko = addNazwiskoTextBox.Text,
                    id_tramwaju = Int32.Parse(addMotorniczyComboBox.SelectedValue.ToString())
                };

                if (newMotorniczy.imie.Length <= 0)
                {
                    MessageBox.Show("Imię nie może być puste");
                    return;
                }

                if (newMotorniczy.nazwisko.Length <= 0)
                {
                    MessageBox.Show("Nazwisko nie może być puste");
                    return;
                }

                context.Motorniczy.Local.Add(newMotorniczy);
                motorniczyViewSource.View.Refresh();
                motorniczyViewSource.View.MoveCurrentTo(newMotorniczy);


                newMotorniczyGrid.Visibility = Visibility.Collapsed;
                existingMotorniczyGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = true;
            }
            else
            {
                Motorniczy currentMotorniczy = (Motorniczy)motorniczyViewSource.View.CurrentItem;

                if (currentMotorniczy == null)
                {
                    MessageBox.Show("Należy wybrać motorniczego");
                    return;
                }

                if (currentMotorniczy.imie.Length <= 0)
                {
                    MessageBox.Show("Imię nie może być puste");
                    return;
                }

                if (currentMotorniczy.nazwisko.Length <= 0)
                {
                    MessageBox.Show("Nazwisko nie może być puste");
                    return;
                }
                var tramwaj = (from c in context.Tramwaje
                                         where c.id == currentMotorniczy.id_tramwaju
                                         select c).FirstOrDefault();
                currentMotorniczy.Tramwaje = tramwaj;
            }

            context.SaveChanges();
            motorniczyViewSource.View.Refresh();

        }
        private void SwitchCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingMotorniczyGrid.Visibility == Visibility.Collapsed)
            {
                existingMotorniczyGrid.Visibility = Visibility.Visible;
                newMotorniczyGrid.Visibility = Visibility.Collapsed;
                btnAdd.Content = "Dodaj przystanek";
                btnDelete.IsEnabled = true;
            }
            else
            {
                existingMotorniczyGrid.Visibility = Visibility.Collapsed;
                newMotorniczyGrid.Visibility = Visibility.Visible;
                btnAdd.Content = "Edytuj przystanek";
                btnDelete.IsEnabled = false;

                // Clear all the text boxes before adding a new customer.
                foreach (var child in newMotorniczyGrid.Children)
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
            context.Motorniczy.Load();
            context.Tramwaje.Load();
            motorniczyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("motorniczyViewSource")));
            tramwajeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tramwajeViewSource")));
            motorniczyViewSource.Source = context.Motorniczy.Local;
            tramwajeViewSource.Source = context.Tramwaje.Local;
        }

    }
}
