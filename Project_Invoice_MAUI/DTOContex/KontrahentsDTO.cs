using Project_Invoice_MAUI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.DTOContex
{
    public class KontrahentsDTO
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string BankAccount_Name { get; set; }
        public string Account_Number { get; set; }
        //public int Company_ID { get; set; }


        //lenistwo
        public string Full_Name { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }

        //można dać w inną klase
        public string Street { get; set; }
        public string House_Number { get; set; }
        public string ZIP_Code { get; set; }
        public string Town { get; set; }
    }
}

