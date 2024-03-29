﻿using System;
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
    /// Interaction logic for ListMoviePage. Update moviesListBox on startup.
    /// </summary>
    public partial class ListMoviePage : Page
    {
        public ListMoviePage()
        {
            InitializeComponent();

            UpdateMovieList();
        }

        /// <summary>
        /// Update movieListBox from local database. Filters output by movieTitleForm search bar.
        /// </summary>
        private void UpdateMovieList()
        {
            using (var db = new DatabaseContext())
            {
                if (movieTitleForm.Text == "")
                {
                    moviesListBox.ItemsSource = db.Movies.ToList();
                } 
                else
                {
                    moviesListBox.ItemsSource = db.Movies.ToList().Where(o => o.Title.Contains(movieTitleForm.Text));
                }        
            }
        }

        /// <summary>
        /// Interaction logic for NavigationAddMovie_Click. Navigates to AddMoviePage.
        /// </summary>
        /// <param name="sender">An object that owns the event handler.</param>
        /// <param name="e">Args from interaction event.</param>
        private void NavigationAddMovie_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddMoviePage());

        }

        /// <summary>
        /// Interaction logic for MovieDeleteButton_Click. Delete Movie object from local database.
        /// Refresh listBox with new content.
        /// </summary>
        /// <param name="sender">An object that owns the event handler.</param>
        /// <param name="e">Args from interaction event.</param>
        private void MovieDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DatabaseContext())
            {
                var movie = moviesListBox.SelectedItem as Movie;
                
                // Check if any list item is selected.
                if (movie == null)
                {
                    return;
                }

                db.Remove(movie);
                db.SaveChanges();

                UpdateMovieList();
            }
        }

        /// <summary>
        /// Interaction logic for movieTitleForm.
        /// Refresh listBox with filtered content.
        /// </summary>
        /// <param name="sender">An object that owns the event handler.</param>
        /// <param name="e">Args from interaction event.</param>
        private void movieTitleForm_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMovieList();
        }
    }
}
