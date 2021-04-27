using Faregosoft.Api2.Data.Entities;
using Faregosoft.Api2.Enums;
using Faregosoft.Api2.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Faregosoft.Api2.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Juan", "Zuluaga", "juan@yopmail.com", "322 311 4620", UserType.Admin);
            await SeedProductsAync();
            await SeedCustomersAync();
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string firstName, string lastName,string email, string phone, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
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
                for (int i = 0; i < 100; i++)
                {
                    _context.Customers.Add(new Customer { User = user, FirstName = $"Nombres {i}", LastName = $"Apellidos  {i}", Email = $"cliente{i}@yopmail.com", Phonenumber = $" {i}{i}{i}{i}{i}{i}{i}", Address = $"Dirección {i}", IsActive = true });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
