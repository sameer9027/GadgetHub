using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GadgetHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RoleId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Description", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "High-performance laptops for work and gaming", null, "Laptops" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest smartphones with advanced features", null, "Smartphones" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Portable tablets for entertainment and work", null, "Tablets" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gadget accessories and peripherals", null, "Accessories" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaming consoles and accessories", null, "Gaming" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smart watches and fitness trackers", null, "Wearables" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Headphones, speakers, and audio equipment", null, "Audio" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digital cameras and photography gear", null, "Cameras" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "LastModifiedDate", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple MacBook Pro with M3 Pro chip, 16GB RAM, 1TB SSD", null, "MacBook Pro 16\"", 2499.99m },
                    { 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dell XPS 13 with Intel i7, 16GB RAM, 512GB SSD", null, "Dell XPS 13", 1299.99m },
                    { 3, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HP Spectre x360 2-in-1 laptop, 13.5\" OLED display", null, "HP Spectre x360", 1399.99m },
                    { 4, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business laptop with Intel i5, 16GB RAM, 512GB SSD", null, "Lenovo ThinkPad X1", 1499.99m },
                    { 5, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple iPhone 15 Pro with A17 Pro chip, 256GB", null, "iPhone 15 Pro", 999.99m },
                    { 6, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samsung Galaxy S24 with Snapdragon 8 Gen 3, 256GB", null, "Samsung Galaxy S24", 899.99m },
                    { 7, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Pixel 8 Pro with Tensor G3, 256GB", null, "Google Pixel 8 Pro", 999.99m },
                    { 8, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "OnePlus 12 with Snapdragon 8 Gen 3, 256GB", null, "OnePlus 12", 799.99m },
                    { 9, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple iPad Air with M1 chip, 64GB, Wi-Fi", null, "iPad Air", 599.99m },
                    { 10, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samsung Galaxy Tab S9, 256GB, 5G capable", null, "Samsung Galaxy Tab S9", 849.99m },
                    { 11, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Surface Pro 9 with Intel i5, 8GB RAM, 256GB SSD", null, "Microsoft Surface Pro 9", 999.99m },
                    { 12, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Noise cancelling wireless earbuds with 30hr battery", null, "Wireless Earbuds", 199.99m },
                    { 13, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "7-in-1 USB-C Hub with HDMI, USB, Ethernet ports", null, "USB-C Hub", 89.99m },
                    { 14, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aluminum laptop stand adjustable for ergonomic use", null, "Laptop Stand", 49.99m },
                    { 15, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Protective phone case with shock absorption", null, "Phone Case", 29.99m },
                    { 16, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sony PlayStation 5 Console with DualSense controller", null, "PlayStation 5", 499.99m },
                    { 17, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Xbox Series X 1TB Console", null, "Xbox Series X", 499.99m },
                    { 18, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nintendo Switch OLED Model with 7-inch screen", null, "Nintendo Switch OLED", 349.99m },
                    { 19, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wireless gaming headset with 7.1 surround sound", null, "Gaming Headset", 129.99m },
                    { 20, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple Watch Series 9 GPS, 45mm, Aluminum", null, "Apple Watch Series 9", 399.99m },
                    { 21, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samsung Galaxy Watch 6 Classic, 47mm", null, "Samsung Galaxy Watch 6", 369.99m },
                    { 22, 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fitbit Charge 6 Fitness Tracker with GPS", null, "Fitbit Charge 6", 159.99m },
                    { 23, 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sony WH-1000XM5 Wireless Noise Cancelling Headphones", null, "Sony WH-1000XM5", 399.99m },
                    { 24, 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bose SoundLink Flex Bluetooth Speaker", null, "Bose SoundLink Flex", 149.99m },
                    { 25, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Canon EOS R5 Mirrorless Camera, 45MP", null, "Canon EOS R5", 3899.99m },
                    { 26, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sony Alpha A7 IV Mirrorless Camera, 33MP", null, "Sony A7 IV", 2499.99m },
                    { 27, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GoPro Hero 12 Action Camera with 5.3K video", null, "GoPro Hero 12", 399.99m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "RoleId", "RoleId1", "Username" },
                values: new object[] { 1, "$2a$11$AfzNtxfc7C0c94A4CHdu5.KqcQLNIUfeR4HzV9WmE0Qo2r9qTcRzq", 1, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId1",
                table: "Users",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
