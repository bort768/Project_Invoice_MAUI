using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Project_Invoice_MAUI.DTOs
{
    public class CompanyDataDTO
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        //public int ID { get; set; }
        public string Full_Name { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string Street { get; set; }
        public string House_Number { get; set; }
        public string ZIP_Code { get; set; }
        public string Town { get; set; }
    }
}

