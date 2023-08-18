using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using UserManagement.API.DataAccess.Entities;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Xml.Linq;

namespace UserManagement.API.DataAccess
{
    public class UserManagmentSeeder
    {
        private readonly UserManagementContext _dbContext;
        private readonly UserManager<User> _userManager;

        public UserManagmentSeeder(UserManagementContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _dbContext.Database.EnsureCreated();
            string url = "https://jsonplaceholder.typicode.com/users";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        var users = JsonSerializer.Deserialize<UserResponse[]>(jsonContent);

                        foreach (var user in users)
                        {
                            User userExist = await _userManager.FindByEmailAsync(user.email);

                            if (userExist == null)
                            {
                                var newUser = MapToUser(user);
                                var result = await _userManager.CreateAsync(newUser, "P@ssw0rd!");
                                if (result != IdentityResult.Success)
                                {
                                    throw new InvalidOperationException("Could not create new user in Seeder");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static User MapToUser(UserResponse response)
        {
            return new User
            {
                Name = response.name,
                UserName = response.username,
                Email = response.email,
                Address = new Address
                {
                    Id = 0,
                    Street = response.address.street,
                    Suite = response.address.suite,
                    City = response.address.city,
                    Zipcode = response.address.zipcode,
                    GeoLat = response.address.geo.lat,
                    GeoLng = response.address.geo.lng
                },
                PhoneNumber = response.phone,
                Website = response.website,
                Company = new Company
                {
                    Id = 0,
                    Name = response.company.name,
                    CatchPhrase = response.company.catchPhrase,
                    Bs = response.company.bs
                }
            };
        }

    }

    public class UserResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public AddressResponse address { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public CompanyResponse company { get; set; }
    }

    public class AddressResponse
    {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public GeoResponse geo { get; set; }
    }

    public class GeoResponse
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class CompanyResponse
    {
        public string name { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }
    }
}
