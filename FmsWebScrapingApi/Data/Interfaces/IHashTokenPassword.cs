namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IHashTokenPassword
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public int UserId { get; set; }
    }
}
