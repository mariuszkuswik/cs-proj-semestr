﻿using System;
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
using System.Text.RegularExpressions;

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
        int mode;

        public TrasyWindow()
        {
            InitializeComponent();
        }
        private void NextCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingPrzystankiNaTrasieGrid.IsEnabled)
            {
                przystankiNaTrasieViewSource.View.MoveCurrentToNext();
                if (przystankiNaTrasieViewSource.View.IsCurrentAfterLast)
                {
                    przystankiNaTrasieViewSource.View.MoveCurrentToPrevious();
                }
                return;
            }
            
            trasyViewSource.View.MoveCurrentToNext();
            if (trasyViewSource.View.IsCurrentAfterLast)
            {
                trasyViewSource.View.MoveCurrentToPrevious();
            }
        }
        private void PreviousCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingPrzystankiNaTrasieGrid.IsEnabled)
            {
                przystankiNaTrasieViewSource.View.MoveCurrentToPrevious();
                if (przystankiNaTrasieViewSource.View.IsCurrentBeforeFirst)
                {
                    przystankiNaTrasieViewSource.View.MoveCurrentToNext();
                }
                return;
            }

            trasyViewSource.View.MoveCurrentToPrevious();
            if (trasyViewSource.View.IsCurrentBeforeFirst)
            {
                trasyViewSource.View.MoveCurrentToNext();
            }
        }
        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingPrzystankiNaTrasieGrid.IsEnabled)
            {
                var selectedPrzystanekNaTrasie = przystankiNaTrasieViewSource.View.CurrentItem as Przystanki_na_trasie;

                if (selectedPrzystanekNaTrasie == null)
                {
                    MessageBox.Show("Nie wybrano żadnego przystanku");
                    return;
                }

                var przystanek = (from c in context.Przystanki_na_trasie
                             where c.id_trasy == selectedPrzystanekNaTrasie.id_trasy &&
                             c.id_przystanku == selectedPrzystanekNaTrasie.id_przystanku
                             select c).FirstOrDefault();

                if (przystanek != null)
                {
                    context.Przystanki_na_trasie.Remove(przystanek);
                }
                context.SaveChanges();
                przystankiNaTrasieViewSource.View.Refresh();
                return;
            }

            var selectedTrasa = trasyViewSource.View.CurrentItem as Trasy;

            if (selectedTrasa == null)
            {
                MessageBox.Show("Nie wybrano żadnej trasy");
                return;
            }

            var trasa = (from c in context.Trasy
                          where c.id == selectedTrasa.id
                          select c).FirstOrDefault();

            if (trasa != null)
            {
                foreach (var przystanekNaTrasie in context.Przystanki_na_trasie)
                {
                    if (przystanekNaTrasie.id_trasy == trasa.id)
                    {
                        MessageBox.Show("Trasa nie jest pusta!");
                        return;
                    }
                }
                context.Trasy.Remove(trasa);
            }
            context.SaveChanges();
            trasyViewSource.View.Refresh();
        }
        private void CommitCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingPrzystankiNaTrasieGrid.IsEnabled)
            {
                return;
            }

            if (newTrasyGrid.IsVisible)
            {

                Trasy newTrasa = new Trasy
                {
                    id = Utils.getNextId(context.Trasy),
                    nazwa = addNazwaTextBox.Text,
                };

                if (newTrasa.nazwa.Length <= 0)
                {
                    MessageBox.Show("Nazwa nie może być pusta");
                    return;
                }

                context.Trasy.Local.Add(newTrasa);
                trasyViewSource.View.Refresh();
                trasyViewSource.View.MoveCurrentTo(newTrasa);


                newTrasyGrid.Visibility = Visibility.Collapsed;
                existingTrasyGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = true;
            }
            else
            {
                Trasy currentTrasa = (Trasy)trasyViewSource.View.CurrentItem;

                if (currentTrasa == null)
                {
                    MessageBox.Show("Należy wybrać trasę");
                    return;
                }

                if (currentTrasa.nazwa.Length <= 0)
                {
                    MessageBox.Show("Nazwa nie może być pusta");
                    return;
                }

            }

            context.SaveChanges();
        }
        private void SwitchCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingTrasyGrid.Visibility == Visibility.Collapsed)
            {
                existingTrasyGrid.Visibility = Visibility.Visible;
                newTrasyGrid.Visibility = Visibility.Collapsed;
                btnDelete.IsEnabled = true;
            }
            else
            {
                existingTrasyGrid.Visibility = Visibility.Collapsed;
                newTrasyGrid.Visibility = Visibility.Visible;
                btnDelete.IsEnabled = false;

                // Clear all the text boxes before adding a new customer.
                foreach (var child in newTrasyGrid.Children)
                {
                    var tb = child as TextBox;
                    if (tb != null)
                    {
                        tb.Text = "";
                    }
                }
            }
        }

        private void ModeCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (existingPrzystankiNaTrasieGrid.IsEnabled)
            {
                existingPrzystankiNaTrasieGrid.IsEnabled = false;
                przystankiDataGrid.IsEnabled = false;

                existingTrasyGrid.IsEnabled = true;
                newTrasyGrid.IsEnabled = true;
                trasyDataGrid.IsEnabled = true;
            } else
            {
                existingPrzystankiNaTrasieGrid.IsEnabled = true;
                przystankiDataGrid.IsEnabled = true;

                existingTrasyGrid.IsEnabled = false;
                newTrasyGrid.IsEnabled = false;
                trasyDataGrid.IsEnabled = false;
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            context.Przystanki_na_trasie.Load();
            przystankiNaTrasieViewSource = ((CollectionViewSource)(this.FindResource("przystankiNaTrasieViewSource")));
            przystankiNaTrasieViewSource.Source = context.Przystanki_na_trasie.Local;

            context.Przystanki.Load();
            przystankiViewSource = ((CollectionViewSource)(this.FindResource("przystankiViewSource")));
            przystankiViewSource.Source = context.Przystanki.Local;

            context.Trasy.Load();
            trasyViewSource = ((CollectionViewSource)(this.FindResource("trasyViewSource")));
            trasyViewSource.Source = context.Trasy.Local;
            

            przystankiNaTrasieViewSource.Filter += new FilterEventHandler(FilterByPrzystanekId);

            przystankiNaTrasieViewSource.SortDescriptions.Add(new SortDescription("numer_na_trasie", ListSortDirection.Ascending));
        }

        private void FilterByPrzystanekId(object sender, FilterEventArgs e)
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

        private void TrasyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            przystankiNaTrasieViewSource.View.Refresh();
        }

        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void Numer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void existingPrzystanekNaTrasieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(existingPrzystanekNaTrasieComboBox.SelectedValue == null)
            {
                return;
            }
            var currentPrzystanekNaTrasie = przystankiNaTrasieViewSource.View.CurrentItem as Przystanki_na_trasie;
            var currentIdPrzystanku = Int32.Parse(existingPrzystanekNaTrasieComboBox.SelectedValue.ToString());
            var currentPrzystanek = (from c in context.Przystanki
                              where c.id == currentIdPrzystanku
                                     select c).FirstOrDefault();

            currentPrzystanekNaTrasie.Przystanki = currentPrzystanek;
            
            przystankiNaTrasieViewSource.View.Refresh();
        }
    }
}
