﻿using System.ComponentModel.DataAnnotations;

namespace CategoryAPI.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public string? Notes { get; set; }
        public IList<Item> Items { get; set; }
    }
}
