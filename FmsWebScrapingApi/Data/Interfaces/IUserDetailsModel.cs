using System.ComponentModel.DataAnnotations;
using FmsWebScrapingApi.Data.Models;

namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IUserDetailsModel
    {
        public int Id { get; set; }

        public string Cnh { get; set; }

        [StringLength(16, MinimumLength = 10)]
        public string Phone { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<IAddressModel> Address { get; set; }
        public IUserModel User { get; set; }
    }
}
