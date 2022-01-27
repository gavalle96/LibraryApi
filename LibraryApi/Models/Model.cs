using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LibraryApi.Models
{
    public class MyDbContext : DbContext
    {
       public MyDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CheckOut> Checkouts { get; set; }

    }

    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(150)]
        public string LastName { get; set; }
        [Required]
        [StringLength(150)]
        public  string Email { get; set; }
        public string Password { get; set; }
        [Required]
        [StringLength(20)]
        public string RoleName { get; set; }
        public ICollection<CheckOut> CheckOuts { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [Required]
        [StringLength(150)]
        public string Author  { get; set; }
        [Required]
        public int PublishedYear { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public ICollection<CheckOut> CheckOuts { get; set; }
    }

    public class CheckOut
    {
        public int Id { get; set; }
        
        public DateTime RequestDate { get; set; }
        public int Copies { get; set; }
        public bool IsReturned { get; set; }
        public Nullable<DateTime> ReturnDate { get; set; }
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}