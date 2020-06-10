using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace akeno
{
    /// <summary>
    /// Local SQLite Database context from database.db
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=database.db");
    }

    /// <summary>
    /// Movie database model with API provided information.
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PosterPath{ get; set; }
        public string Description { get; set; }
        public string ReleaseDate { get; set; }

    }
}