using Faregosoft.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Faregosoft.Api.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await SeedUsersAync();
            await SeedProductsAync();
            await SeedCustomersAync();
        }

        private async Task SeedUsersAync()
        {
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User { FirstName = "Juan", LastName = "Reyes", Email = "juan@yopmail.com", Password = "123456", IsActive = true });
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedProductsAync()
        {
            if (!_context.Products.Any())
            {
                User user = await _context.Users.FirstOrDefaultAsync();
                _context.Products.Add(new Product { User = user, Name = "iPad", Description = "Lorem Ipsum...", Price = 879.65M, Inventory = 12, IsActive = true }) ;
                _context.Products.Add(new Product { User = user, Name = "iPhone", Description = "Lorem Ipsum...", Price = 1200.00M, Inventory = 36, IsActive = true });
                _context.Products.Add(new Product { User = user, Name = "iWatch", Description = "Lorem Ipsum...", Price = 660.00M, Inventory = 24, IsActive = true });
                _context.Products.Add(new Product { User = user, Name = "iMac", Description = "Lorem Ipsum...", Price = 2400.99M, Inventory = 6, IsActive = true });
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedCustomersAync()
        {
            if (!_context.Customers.Any())
            {
                User user = await _context.Users.FirstOrDefaultAsync();
                _context.Customers.Add(new Customer { User = user, FirstName = "Juan", LastName = "Reyes", Email = "juan@yopmail.com", Phonenumber = "303030", Address = "Calle Luna Calle Sol", IsActive = true });
                _context.Customers.Add(new Customer { User = user, FirstName = "Fausto", LastName = "Reyes", Email = "fausto@yopmail.com", Phonenumber = "404040", Address = "Calle Luna Calle Sol", IsActive = true });
                _context.Customers.Add(new Customer { User = user, FirstName = "Juan", LastName = "Zuluaga", Email = "zulu@yopmail.com", Phonenumber = "505050", Address = "Calle Luna Calle Sol", IsActive = true });
                await _context.SaveChangesAsync();
            }
        }
    }
}
