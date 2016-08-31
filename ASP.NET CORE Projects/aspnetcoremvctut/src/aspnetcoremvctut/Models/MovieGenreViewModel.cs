using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

//Adding Search by Genre
/*
 The move-genre view model will contain:

a list of movies
a SelectList containing the list of genres. This will allow the user to select a genre from the list.
movieGenre, which contains the selected genre
     */
namespace MvcMovie.Models
{
    public class MovieGenreViewModel
    {
        public List<Movie> movies;
        public SelectList genres;
        public string movieGenre { get; set; }
    }
}