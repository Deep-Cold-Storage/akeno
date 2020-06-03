using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace akeno
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateMovieList();

        }

    

        private static async Task<Movie> GetJsonHttpClient(string uri, HttpClient httpClient)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Trace.WriteLine(responseBody);

                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                var myString = root.GetProperty("results")[0];
                Trace.WriteLine(myString);

               return new Movie { Title = myString.GetProperty("title").ToString() };
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

        private void UpdateMovieList()
        {
            using (var db = new DatabaseContext())
            {
                // Create
                //Trace.WriteLine("Inserting a new movie!");
                //db.Add(new Movie { Title = "Bee Movie " });
                //db.SaveChanges();

                // Read
                //Trace.WriteLine("Querying for a movies!");
                //var movie = db.Movies.First();

                // Update
                //Trace.WriteLine("Updating the movie and adding a movie!");
                //movie.Title = "Bee Movie 2";
                //db.SaveChanges();

                moviesListBox.ItemsSource = db.Movies.ToList();
            }
        }

        private async void MovieAddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
     
                if (movieTitleForm.Text == "")
                {
                    return;
                }

                HttpClient client = new HttpClient();
                var movie = await GetJsonHttpClient("https://api.themoviedb.org/3/search/movie?api_key=f08ca94e9e62a564b46cdff1046fce3a&query=" + movieTitleForm.Text, client) as Movie;

                Trace.WriteLine(movie.Title);

                db.Add(movie);
                db.SaveChanges();
                movieTitleForm.Text = "";

                UpdateMovieList();


            }
        }

        private void movieDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
                var movie = moviesListBox.SelectedItem as Movie;
                if (movie == null)
                {
                    return;
                }

                Trace.WriteLine(movie.Title);

                db.Remove(movie);
                db.SaveChanges();
                UpdateMovieList();
            }
        }
    }
}
