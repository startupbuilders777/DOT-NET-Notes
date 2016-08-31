using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

/*
 TO NOT HAVE ERROR DO THIS:

    PM> Add-Migration -Context MvcMovieContext
    PM> Update-Database -Context MvcMovieContext

     */
namespace aspnetcoremvctut.Models
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
            
        }

        public DbSet<Movie> Movie { get; set; }
        
    }
}
