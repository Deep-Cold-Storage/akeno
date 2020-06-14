using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace akeno
{
    public class ApiClient
    {
        /// <summary>
        /// Async create List of Movie objects from MovieDB API by searchQuery.
        /// </summary>
        /// <param name="searchQuery">The query for the external API endpoint.</param>
        /// <param name="httpClient">Base class for sending and receiving HTTP requests.</param>
        /// <returns>
        /// Return List of Movie objects with metadata.
        /// <returns>
        public static async Task<List<Movie>> GetJsonHttpClient(string searchQuery, HttpClient httpClient)
        {
            var API_KEY = "f08ca94e9e62a564b46cdff1046fce3a";

            try
            {
                var response = await httpClient.GetAsync("https://api.themoviedb.org/3/search/movie?api_key=" + API_KEY + "&query=" + Uri.EscapeUriString(searchQuery));
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

                    movies.Add(new Movie
                    {
                        Title = movieResult.GetProperty("title").ToString(),
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
    }
}
