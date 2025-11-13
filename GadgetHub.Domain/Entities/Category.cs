using GadgetHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GadgetHub.Domain.Entities
{
    public class Category :BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation property - EF Core will automatically detect this as one-to-many
        public ICollection<Product> Products { get; set; } = new List<Product>();

        // Constructors
        public Category() { } // For EF Core

        public Category(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description;
            LastModifiedDate = DateTime.UtcNow;
        }
    }
}
