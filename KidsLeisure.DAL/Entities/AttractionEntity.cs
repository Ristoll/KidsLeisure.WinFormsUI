﻿using KidsLeisure.DAL.Interfaces;
namespace KidsLeisure.DAL.Entities
{
    public class AttractionEntity : IItemEntity
    {
        public int AttractionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DisplayName => $"{Name} - {Price} грн";
        public int GetId() => AttractionId;
    }
}