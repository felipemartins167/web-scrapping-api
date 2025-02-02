﻿namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IRoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
