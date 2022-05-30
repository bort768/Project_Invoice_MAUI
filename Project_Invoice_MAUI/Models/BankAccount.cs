namespace Project_Invoice_MAUI.Models
{
    public class BankAccount
    {
        public string BankAccount_Name { get; set; }
        public string Account_Number { get; set; }
        public string Currency { get; set; }
        public int Value { get; set; }

        public BankAccount(string bankAccount_Name, string account_Number, string currency, int value)
        {
            BankAccount_Name = bankAccount_Name;
            Account_Number = account_Number;

            Currency = currency;
            Value = value;
        }
    }
}
