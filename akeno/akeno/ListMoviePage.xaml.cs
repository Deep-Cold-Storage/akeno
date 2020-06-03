using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace akeno
{
    /// <summary>
    /// Interaction logic for ListMoviePage.xaml
    /// </summary>
    public partial class ListMoviePage : Page
    {
        public ListMoviePage()
        {
            InitializeComponent();

            UpdateMovieList();

        }

        private void UpdateMovieList()
        {
            using (var db = new DatabaseContext())
            {
                moviesListBox.ItemsSource = db.Movies.ToList();
            }
        }

        private void NavigationAddMovie_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddMoviePage());

        }

        private void MovieDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
                var movie = moviesListBox.SelectedItem as Movie;
                if (movie == null)
                {
                    return;
                }

                db.Remove(movie);
                db.SaveChanges();

                UpdateMovieList();
            }
        }
    }
}
