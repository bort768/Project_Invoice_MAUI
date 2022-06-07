namespace Project_Invoice_MAUI.Models
{
    public class Kontrahent
    {
        public Kontrahent(string bankAccount_Name, string account_Number, CompanyData company)
        {
            BankAccount_Name = bankAccount_Name;
            Account_Number = account_Number;
            Company = company;
        }

        // można dodac numer kontrahenta(index)
        public string BankAccount_Name { get; set; }
        public string Account_Number { get; set; }
        public CompanyData Company { get; set; }
        public int ID { get; set; }
        public bool IsSelected { get; set; }

        public override string ToString()
        {
            return Company.Full_Name;
        }
    }
}
