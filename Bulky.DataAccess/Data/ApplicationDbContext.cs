using Bulky.Models;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories
        {
            get; set;
        }

        public DbSet<Product> Products
        {
            get; set;
        }

        public DbSet<OrderDetails> OrderDetails
        {
            get; set;
        }

        public DbSet<OrderHeader> OrderHeaders
        {
            get; set;
        }

        public DbSet<ShoppingCart> ShoppintCarts
        {
            get; set;
        }
        public DbSet<Company> Companies
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "aayush", DisplayOrder = 1 },
                new Category { Id = 2, Name = "stha", DisplayOrder = 2 },
                new Category { Id = 3, Name = "avi", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Company>().HasData(
    new Company
    {
        Id = 1,
        Name = "TechNova Solutions",
        StreetAddress = "123 Main Street",
        City = "Kathmandu",
        state = "Bagmati",
        PostalCode = "44600",
        phoneNumber = 9811111111
    },
    new Company
    {
        Id = 2,
        Name = "Everest IT Pvt. Ltd.",
        StreetAddress = "45 Lakeside Road",
        City = "Pokhara",
        state = "Gandaki",
        PostalCode = "33700",
        phoneNumber = 9822222222
    },
    new Company
    {
        Id = 3,
        Name = "Himalayan Traders",
        StreetAddress = "78 Putalisadak",
        City = "Kathmandu",
        state = "Bagmati",
        PostalCode = "44605",
        phoneNumber = 9833333333
    },
    new Company
    {
        Id = 4,
        Name = "Future Soft Nepal",
        StreetAddress = "12 Birtamode Chowk",
        City = "Birtamode",
        state = "Koshi",
        PostalCode = "57204",
        phoneNumber = 9844444444
    }
);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Dune",
                    Description = "A sweeping science fiction epic set on the desert planet Arrakis, following Paul Atreides as he navigates politics, prophecy, and survival.",
                    ISBN = "9780441013593",
                    Author = "Frank Herbert",
                    ListPrice = 45,
                    Price = 40,
                    Price50 = 35,
                    Price100 = 30,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Clean Code",
                    Description = "A handbook of agile software craftsmanship, covering principles and practices for writing maintainable, readable code.",
                    ISBN = "9780132350884",
                    Author = "Robert C. Martin",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 45,
                    Price100 = 40,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "The Hobbit",
                    Description = "A reluctant hobbit, Bilbo Baggins, sets out on an unexpected journey to help a group of dwarves reclaim their homeland from a dragon.",
                    ISBN = "9780547928227",
                    Author = "J.R.R. Tolkien",
                    ListPrice = 35,
                    Price = 30,
                    Price50 = 27,
                    Price100 = 24,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Atomic Habits",
                    Description = "A practical guide to building good habits and breaking bad ones, using small, incremental changes.",
                    ISBN = "9780735211292",
                    Author = "James Clear",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 24,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "1984",
                    Description = "A dystopian novel depicting a totalitarian regime that enforces absolute control through surveillance and propaganda.",
                    ISBN = "9780451524935",
                    Author = "George Orwell",
                    ListPrice = 25,
                    Price = 22,
                    Price50 = 19,
                    Price100 = 16,
                    CategoryId = 2,
                    ImageUrl = ""
                }
            );
        }
    }
}
