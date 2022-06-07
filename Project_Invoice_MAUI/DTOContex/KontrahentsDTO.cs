using Project_Invoice_MAUI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.DbContexs
{
    public class KontrahentsDTO
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string BankAccount_Name { get; set; }
        public string Account_Number { get; set; }
        public CompanyData Company { get; set; }
    }
}
