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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AddMoviePage : Page
    {
        public AddMoviePage()
        {
            InitializeComponent();
        }


        private static async Task<List<Movie>> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                var response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Trace.WriteLine(responseBody);

                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                List<Movie> movies = new List<Movie> { };

                var len = root.GetProperty("results").GetArrayLength() - 1;

                if (len > 5)
                {
                    len = 5;
                }


                for (int i = 1; i <= len; i++)
                {
                    var myString = root.GetProperty("results")[i];
                    movies.Add(new Movie { Title = myString.GetProperty("title").ToString(), PosterPath = "https://image.tmdb.org/t/p/w500/" + myString.GetProperty("poster_path").ToString(), Description = myString.GetProperty("overview").ToString(), ReleaseDate = myString.GetProperty("release_date").ToString() });
                }

                return movies;
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }

        private void MovieAddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {

                var movie = moviesListBox.SelectedItem as Movie;

                if (movie == null)
                {
                    return;
                }

                db.Add(movie);
                db.SaveChanges();

                this.NavigationService.Navigate(new ListMoviePage());

            }
        }

        private async void movieSearchButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (movieTitleForm.Text == "")
            {
                return;
            }

            HttpClient client = new HttpClient();
            List<Movie> movies = await GetJsonHttpClient("https://api.themoviedb.org/3/search/movie?api_key=f08ca94e9e62a564b46cdff1046fce3a&query=" + movieTitleForm.Text, client) as List<Movie>;

            moviesListBox.ItemsSource = movies;
        }
    }
}
