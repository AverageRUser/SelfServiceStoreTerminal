
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCompApp.Models;

namespace TradeCompApp.Database
{
    public class DatabaseService : DbContext
    {
        //Вариант 2 Entity Framework
        /*
           public DbSet<Product> Products { get; set; }
           public DbSet<Category> Categories { get; set; }
           public DbSet<TechSpec> TechSpecs { get; set; }

           protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           {

               optionsBuilder.UseMySql(
                  ,
                    new MySqlServerVersion(new Version(8, 0, 31))
               );
           }
           protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               // Настройка имен таблиц (если они отличаются от названий классов)
               modelBuilder.Entity<TechSpec>().ToTable("TechSpecs");

               // Настройка отношений
               modelBuilder.Entity<Product>()
                   .HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

               modelBuilder.Entity<TechSpec>()
                   .HasOne(t => t.Product)
                   .WithMany(p => p.Specs)
                   .HasForeignKey(t => t.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);


           }
         */
        //Вариант 1 MySQLConnector

    


        public DatabaseService()
        {
          
        }
        public async Task<string> GetConnectionString()
        {
            return await SecureStorage.GetAsync("mysql_connection");
        }
        public async Task<List<Product>> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new MySqlConnection(await GetConnectionString()))
            {
                connection.Open();

                // Запрос для получения продуктов
                var productCommand = new MySqlCommand(
                    "SELECT Id, Name, Price, ImageUrl ,CategoryId FROM Products", connection);

                using (var reader = productCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product
                        {
                            ProductId = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Price = reader.GetDecimal("Price"),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ?
                            null : reader.GetString("ImageUrl"),
                            CategoryId = reader.GetInt32("CategoryId")
                        };

                        products.Add(product);
                    }
                }

                // Для каждого продукта загружаем спецификации
                foreach (var product in products)
                {
                    product.Specs = GetTechSpecsForProduct(product.ProductId, connection);
                }
                
            }

            return products;
        }
        public async Task<List<ProductService>> GetProductServices(int categoryId)
        {
            var productServices = new List<ProductService>();
            using (var connection = new MySqlConnection(await GetConnectionString()))
            {
                connection.Open();
                var specCommand = new MySqlCommand(
                "SELECT Id, Name, Price FROM Services WHERE CategoryId = @Id", connection);
                specCommand.Parameters.AddWithValue("@Id", categoryId);

                using (var reader = specCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productServices.Add(new ProductService
                        {
                             Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Price = reader.GetInt32("Price"),
                            CategoryId = categoryId
                        });
                    }
                }
            }
            return productServices;
        }
        private List<TechSpec> GetTechSpecsForProduct(int productId, MySqlConnection connection)
        {
            var specs = new List<TechSpec>();

            var specCommand = new MySqlCommand("SELECT Id, SpecName, SpecValue, Unit FROM TechSpecs WHERE ProductId = @Id", connection);
            specCommand.Parameters.AddWithValue("@Id", productId);

            using (var reader = specCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    specs.Add(new TechSpec
                    {
                        TechSpecId = reader.GetInt32("Id"),
                        Name = reader.GetString("SpecName"),
                        Value = reader.GetString("SpecValue"),
                        Unit = reader.IsDBNull(reader.GetOrdinal("Unit")) ?
                            null : reader.GetString("Unit"),
                        ProductId = productId
                    });
                }
            }

            return specs;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = new List<Category>();

            using (var connection = new MySqlConnection(await GetConnectionString()))
            using (var command = new MySqlCommand("SELECT Id, Name, ImageUrl FROM categories", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ?
                            null : reader.GetString("ImageUrl")
                        });
                    }
                }
            }

            return categories;
        }
        public async Task<Receipt> GetReceipt()
        {
            var receipt = new Receipt();
        
         
            using (var connection = new MySqlConnection(await GetConnectionString()))
            {
                using (var commands = new MySqlCommand("INSERT receipt (EmployeeId, MarketId) VALUES (1,1)", connection))
                {
                    connection.Open();
                    commands.ExecuteNonQuery();
                  
                }
                using (var command = new MySqlCommand("select r.Id,r.CreatedAt, m.ИНН as ИНН, ККМ, ЭКЛЗ, Адрес, ФИО, Должность from receipt as r " +
                    "left join market as m on r.MarketId = m.Id " +
                    "left join employee as e on r.EmployeeId = e.Id " +
                    "ORDER BY r.CreatedAt DESC LIMIT 1;", connection))
                {
                    //connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            receipt = new Receipt
                            {
                                Id = reader.GetInt32("Id"),
                                CreatedAt = reader.GetDateTime("CreatedAt"),
                                Address = reader.GetString("Адрес"),
                                EKLZ = reader.GetString("ЭКЛЗ"),
                                INN = reader.GetString("ИНН"),
                                KKM = reader.GetString("ККМ"),
                                FIO = reader.GetString("ФИО"),
                                Position = reader.GetString("Должность")
                            };
                        }
                    }
                }
            }
            return receipt;
        }
    }
}
