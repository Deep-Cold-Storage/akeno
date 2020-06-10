using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AddMoviePage.
    /// </summary>
    public partial class AddMoviePage : Page
    {
        public AddMoviePage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Interaction logic for MovieAddButton_Click. 
        /// Save selected Movie to local database and navigate to ListMoviePage.
        /// </summary>
        private void MovieAddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
                var movie = moviesListBox.SelectedItem as Movie;

                // Check if any movie is selected.
                if (movie == null)
                {
                    return;
                }

                db.Add(movie);
                db.SaveChanges();

                this.NavigationService.Navigate(new ListMoviePage());

            }
        }

        /// <summary>
        /// Interaction logic for MovieSearchButton_ClickAsync. 
        /// Search API by movieTitleForm text. Update listBox with Movie results found.
        /// </summary>
        private async void MovieSearchButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (movieTitleForm.Text == "")
            {
                return;
            }

            HttpClient client = new HttpClient();
            List<Movie> movies = await ApiClient.GetJsonHttpClient(movieTitleForm.Text, client) as List<Movie>;

            moviesListBox.ItemsSource = movies;
        }
    }
}
