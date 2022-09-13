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
    /// Logika interakcji dla klasy MotorniczyWindow.xaml
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
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            motorniczyViewSource.View.MoveCurrentToPrevious();
        }
        private void DeleteCustomerCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            // If existing window is visible, delete the customer and all their orders.
            // In a real application, you should add warnings and allow the user to cancel the operation.
            var selectedMotorniczy = motorniczyViewSource.View.CurrentItem as Motorniczy;

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
        private int getNextId()
        {
            var current_id = 0;
            foreach (var motorniczy in context.Motorniczy)
            {
                if (motorniczy.id > current_id)
                {
                    current_id = motorniczy.id;
                }
            }

            return current_id + 1;
        }

        private void UpdateCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (newMotorniczyGrid.IsVisible)
            {
                // Create a new object because the old one
                // is being tracked by EF now.
                Motorniczy newMotorniczy = new Motorniczy
                {
                    id = getNextId(),
                    imie = addImieTextBox.Text,
                    nazwisko = addNazwiskoTextBox.Text,
                    id_tramwaju = Int32.Parse(addMotorniczyComboBox.SelectedValue.ToString())
                };

                // Perform very basic validation
                //if (newPrzystanek.imie.Length <= 0)
                //{
                //    MessageBox.Show("Nazwa nie może być pusta");
                //    return;
                //}

                // Insert the new customer at correct position:
                int len = context.Motorniczy.Local.Count();

                context.Motorniczy.Local.Add(newMotorniczy);
                motorniczyViewSource.View.Refresh();
                motorniczyViewSource.View.MoveCurrentTo(newMotorniczy);


                newMotorniczyGrid.Visibility = Visibility.Collapsed;
                existingMotorniczyGrid.Visibility = Visibility.Visible;
                btnAdd.Content = "Dodaj przystanek";
                btnDelete.IsEnabled = true;
            }
            else
            {
                Motorniczy currentMotorniczy = (Motorniczy)motorniczyViewSource.View.CurrentItem;

                if (currentMotorniczy.imie.Length <= 0)
                {
                    MessageBox.Show("Nazwa nie może być pusta");
                    return;
                }

            }

            // Save the changes, either for a new customer, a new order
            // or an edit to an existing customer or order.
            context.SaveChanges();

        }
        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
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
            System.Windows.Data.CollectionViewSource motorniczyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("motorniczyViewSource")));
            System.Windows.Data.CollectionViewSource tramwajeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tramwajeViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            motorniczyViewSource.Source = context.Motorniczy.Local;
            tramwajeViewSource.Source = context.Tramwaje.Local;
        }

    }
}
