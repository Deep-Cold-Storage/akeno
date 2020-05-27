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

        private void movieAddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
     
                if (movieTitleForm.Text == "")
                {
                    return;
                }

                db.Add(new Movie { Title = movieTitleForm.Text });
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
