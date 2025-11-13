using GadgetHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Infrastructure.Data;

public class MyDBcontext : DbContext
{
    public MyDBcontext(DbContextOptions<MyDBcontext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ⚠️ Prevent cascade delete
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ Indexes
        modelBuilder.Entity<Product>().HasIndex(p => p.Name);
        modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

        // ✅ Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Laptops", Description = "High-performance laptops for work and gaming" },
            new Category { Id = 2, Name = "Smartphones", Description = "Latest smartphones with advanced features" },
            new Category { Id = 3, Name = "Tablets", Description = "Portable tablets for entertainment and work" },
            new Category { Id = 4, Name = "Accessories", Description = "Gadget accessories and peripherals" },
            new Category { Id = 5, Name = "Gaming", Description = "Gaming consoles and accessories" },
            new Category { Id = 6, Name = "Wearables", Description = "Smart watches and fitness trackers" },
            new Category { Id = 7, Name = "Audio", Description = "Headphones, speakers, and audio equipment" },
            new Category { Id = 8, Name = "Cameras", Description = "Digital cameras and photography gear" }
        );

        // ✅ Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "MacBook Pro 16\"", Description = "Apple MacBook Pro with M3 Pro chip, 16GB RAM, 1TB SSD", Price = 2499.99m, CategoryId = 1 },
            new Product { Id = 2, Name = "Dell XPS 13", Description = "Dell XPS 13 with Intel i7, 16GB RAM, 512GB SSD", Price = 1299.99m, CategoryId = 1 },
            new Product { Id = 3, Name = "HP Spectre x360", Description = "HP Spectre x360 2-in-1 laptop, 13.5\" OLED display", Price = 1399.99m, CategoryId = 1 },
            new Product { Id = 4, Name = "Lenovo ThinkPad X1", Description = "Business laptop with Intel i5, 16GB RAM, 512GB SSD", Price = 1499.99m, CategoryId = 1 },

            new Product { Id = 5, Name = "iPhone 15 Pro", Description = "Apple iPhone 15 Pro with A17 Pro chip, 256GB", Price = 999.99m, CategoryId = 2 },
            new Product { Id = 6, Name = "Samsung Galaxy S24", Description = "Samsung Galaxy S24 with Snapdragon 8 Gen 3, 256GB", Price = 899.99m, CategoryId = 2 },
            new Product { Id = 7, Name = "Google Pixel 8 Pro", Description = "Google Pixel 8 Pro with Tensor G3, 256GB", Price = 999.99m, CategoryId = 2 },
            new Product { Id = 8, Name = "OnePlus 12", Description = "OnePlus 12 with Snapdragon 8 Gen 3, 256GB", Price = 799.99m, CategoryId = 2 },

            new Product { Id = 9, Name = "iPad Air", Description = "Apple iPad Air with M1 chip, 64GB, Wi-Fi", Price = 599.99m, CategoryId = 3 },
            new Product { Id = 10, Name = "Samsung Galaxy Tab S9", Description = "Samsung Galaxy Tab S9, 256GB, 5G capable", Price = 849.99m, CategoryId = 3 },
            new Product { Id = 11, Name = "Microsoft Surface Pro 9", Description = "Surface Pro 9 with Intel i5, 8GB RAM, 256GB SSD", Price = 999.99m, CategoryId = 3 },

            new Product { Id = 12, Name = "Wireless Earbuds", Description = "Noise cancelling wireless earbuds with 30hr battery", Price = 199.99m, CategoryId = 4 },
            new Product { Id = 13, Name = "USB-C Hub", Description = "7-in-1 USB-C Hub with HDMI, USB, Ethernet ports", Price = 89.99m, CategoryId = 4 },
            new Product { Id = 14, Name = "Laptop Stand", Description = "Aluminum laptop stand adjustable for ergonomic use", Price = 49.99m, CategoryId = 4 },
            new Product { Id = 15, Name = "Phone Case", Description = "Protective phone case with shock absorption", Price = 29.99m, CategoryId = 4 },

            new Product { Id = 16, Name = "PlayStation 5", Description = "Sony PlayStation 5 Console with DualSense controller", Price = 499.99m, CategoryId = 5 },
            new Product { Id = 17, Name = "Xbox Series X", Description = "Microsoft Xbox Series X 1TB Console", Price = 499.99m, CategoryId = 5 },
            new Product { Id = 18, Name = "Nintendo Switch OLED", Description = "Nintendo Switch OLED Model with 7-inch screen", Price = 349.99m, CategoryId = 5 },
            new Product { Id = 19, Name = "Gaming Headset", Description = "Wireless gaming headset with 7.1 surround sound", Price = 129.99m, CategoryId = 5 },

            new Product { Id = 20, Name = "Apple Watch Series 9", Description = "Apple Watch Series 9 GPS, 45mm, Aluminum", Price = 399.99m, CategoryId = 6 },
            new Product { Id = 21, Name = "Samsung Galaxy Watch 6", Description = "Samsung Galaxy Watch 6 Classic, 47mm", Price = 369.99m, CategoryId = 6 },
            new Product { Id = 22, Name = "Fitbit Charge 6", Description = "Fitbit Charge 6 Fitness Tracker with GPS", Price = 159.99m, CategoryId = 6 },

            new Product { Id = 23, Name = "Sony WH-1000XM5", Description = "Sony WH-1000XM5 Wireless Noise Cancelling Headphones", Price = 399.99m, CategoryId = 7 },
            new Product { Id = 24, Name = "Bose SoundLink Flex", Description = "Bose SoundLink Flex Bluetooth Speaker", Price = 149.99m, CategoryId = 7 },

            new Product { Id = 25, Name = "Canon EOS R5", Description = "Canon EOS R5 Mirrorless Camera, 45MP", Price = 3899.99m, CategoryId = 8 },
            new Product { Id = 26, Name = "Sony A7 IV", Description = "Sony Alpha A7 IV Mirrorless Camera, 33MP", Price = 2499.99m, CategoryId = 8 },
            new Product { Id = 27, Name = "GoPro Hero 12", Description = "GoPro Hero 12 Action Camera with 5.3K video", Price = 399.99m, CategoryId = 8 }
        );
    }
}