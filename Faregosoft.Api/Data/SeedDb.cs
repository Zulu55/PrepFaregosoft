using Faregosoft.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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
                Random random = new Random();
                for (int i = 0; i < 100; i++)
                {
                    _context.Products.Add(new Product { User = user, Name = $"Producto: {i}", Description = $"Producto: {i}", Price = random.Next(1, 100), Inventory = random.Next(1, 100), IsActive = true });
                }
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
