namespace Project_Invoice_MAUI.Models
{
    public class DocumentNumbering
    {

        public bool Broken_By_Mounth { get; set; }
        public bool Broken_By_Year { get; set; }

        public DocumentNumbering(bool broken_By_Mounth, bool broken_By_Year)
        {
            Broken_By_Mounth = broken_By_Mounth;
            Broken_By_Year = broken_By_Year;
        }
    }
}
