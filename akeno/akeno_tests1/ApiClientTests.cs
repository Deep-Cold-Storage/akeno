using Microsoft.VisualStudio.TestTools.UnitTesting;
using akeno;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace akeno.Tests
{
    [TestClass()]
    public class ApiClientTests
    {
        [TestMethod()]
        public async Task GetJsonHttpClientTest_BlankSearch()
        {

            var ApiClient = new ApiClient();
            HttpClient client = new HttpClient();

            try
            {
                List<Movie> movies = await ApiClient.GetJsonHttpClient("", client) as List<Movie>;

                _ = movies[10];
               
                Assert.Fail();

            } catch { };
        }

        [TestMethod()]
        public async Task GetJsonHttpClientTest_KnownSearch()
        {

            var ApiClient = new ApiClient();
            HttpClient client = new HttpClient();

            try
            {
                List<Movie> movies = await ApiClient.GetJsonHttpClient("Star Wars ", client) as List<Movie>;

                Movie movie = movies[0];

                Assert.IsNotNull(movie);
            }
            catch 
            {
                Assert.Fail();
            };
        }
    }
}