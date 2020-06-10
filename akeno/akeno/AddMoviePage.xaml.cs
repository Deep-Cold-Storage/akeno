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
        /// Async create List of Movie objects from MovieDB API by searchQuery.
        /// </summary>
        /// <returns>
        /// Return List of Movie objects with metadata.
        /// <returns>
        private async Task<List<Movie>> GetJsonHttpClient(string searchQuery, HttpClient httpClient)
        {
            var API_KEY = "f08ca94e9e62a564b46cdff1046fce3a";

            try
            {
                var response = await httpClient.GetAsync("https://api.themoviedb.org/3/search/movie?api_key=" + API_KEY + "&query=" + searchQuery);
                response.EnsureSuccessStatusCode();

                using var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = jsonDoc.RootElement;

                // Create placeholder List of Movie objects.
                List<Movie> movies = new List<Movie> { };

                // Limit amount of results to 5.
                var len = root.GetProperty("results").GetArrayLength() - 1;
                if (len > 5)
                {
                    len = 5;
                }

                // Create Movie objects from API results.
                for (int i = 1; i <= len; i++)
                {
                    var movieResult = root.GetProperty("results")[i];
                    
                    movies.Add(new Movie { Title = movieResult.GetProperty("title").ToString(),
                        PosterPath = "https://image.tmdb.org/t/p/w500/" + movieResult.GetProperty("poster_path").ToString(),
                        Description = movieResult.GetProperty("overview").ToString(),
                        ReleaseDate = "Released: " + movieResult.GetProperty("release_date").ToString().Split("-")[0]
                    });
                }

                return movies;
            }
            catch (HttpRequestException)
            {
                return new List<Movie> { }; ;
            }
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
            List<Movie> movies = await GetJsonHttpClient(movieTitleForm.Text, client) as List<Movie>;

            moviesListBox.ItemsSource = movies;
        }
    }
}
