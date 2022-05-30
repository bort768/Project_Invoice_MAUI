using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SaveFileHelper;
using Project_Invoice_MAUI.Singleton;
//using System.Collections.ObjectModel;

namespace Project_Invoice_MAUI.ViewModels

/* Niescalona zmiana z projektu „Project_Invoice_MAUI (net6.0-windows10.0.19041.0)”
Przed:
{
    
    public partial class InvoiceViewModel : BaseViewModel
Po:
{

    public partial class InvoiceViewModel : BaseViewModel
*/

/* Niescalona zmiana z projektu „Project_Invoice_MAUI (net6.0-maccatalyst)”
Przed:
{
    
    public partial class InvoiceViewModel : BaseViewModel
Po:
{

    public partial class InvoiceViewModel : BaseViewModel
*/

/* Niescalona zmiana z projektu „Project_Invoice_MAUI (net6.0-ios)”
Przed:
{
    
    public partial class InvoiceViewModel : BaseViewModel
Po:
{

    public partial class InvoiceViewModel : BaseViewModel
*/
{

    public partial class InvoiceViewModel : BaseViewModel
    {
        Firma firma = Firma.GetInstance();

        ISaveToFile MetodOfSave;

        //public CommandBase SubmitInvoiceCommand { get; set; }
        //public CommandBase AddGoodToListCommand { get; set; }
        //public CommandBase RemoveGoodFromListCommand { get; set; }
        //public CommandBase EditGoodToListCommand { get; set; }

        public ObservableCollection<Goods> List_Of_Added_Goods { get; set; } = new();
        public ObservableCollection<Kontrahent> List_Of_Kontrahents { get; set; }
        public ObservableCollection<Goods> Add_Remove_Goods_List { get; set; }

        [ObservableProperty]
        private Kontrahent _selected_Kontrahent;


        public CompanyData companyData { get; set; }

        public static int Invoice_Number = 1;

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                var SelectedItem = from x in List_Of_Added_Goods where (x.IsSelected == true) select x;
                Selected_Goods = (Goods)SelectedItem;
                OnPropertyChanged();
            }
        }


        public string Town { get; set; }


        private string _selected_Item;

        public string Selected_Item
        {
            get
            {
                return _selected_Item;
            }
            set
            {
                int index = 0;
                foreach (var goods in firma.goods)
                {
                    if (goods.Product_Name == value)
                    {
                        SaveToLocal(index);
                        break;
                    }
                    index++;
                }
                _selected_Item = value;
                OnPropertyChanged();
            }
        }

        private int _quantity = 1;
        public string Quantity
        {
            get
            {
                return _quantity.ToString();
            }
            set
            {
                if (value != null && value != "")
                {
                    _quantity = Convert.ToInt32(value!);
                    if (Selected_Goods != null)
                    {
                        Selected_Goods.Quantity = _quantity;
                        Selected_Goods.Sum = (_quantity * Selected_Goods.Price_Brutto);
                    }

                }

                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private Goods _selected_Goods;


        public Kontrahent kontrahent { get; set; }


        private DateTime _dataWystawienia = DateTime.Now;
        public DateTime DataWystawienia
        {
            get
            {
                return _dataWystawienia;
            }
            set
            {
                _dataWystawienia = value;
                Wypisanie_Invoice_Format();
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private DateTime _dataDostawy = DateTime.Now;



        [ObservableProperty]
        private string _invoice_Format;





        public InvoiceViewModel()
        {
            if (firma.CompanyData != null)
            {
                Town = firma.CompanyData.Town;
            }
            else
            {
                //MessageBox.Show("Dane o firmie nie zostały załadowane", "Bład", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Wypisanie_Invoice_Format();

            if (firma.CompanyData != null)
            {
                companyData = firma.CompanyData;
            }

            List_Of_Added_Goods = new();

            //dodawanie i towrzenie listy kontrahentów do comobobox
            List_Of_Kontrahents = new();
            foreach (var kontrahent in firma.kontrahents!)
            {
                List_Of_Kontrahents.Add(kontrahent);
            }

            Add_Remove_Goods_List = new();

            if (firma.goods != null)
            {
                //Add_Remove_Goods_List = firma.goods;
                foreach (var goods in firma.goods)
                {
                    Add_Remove_Goods_List.Add(goods);
                }
            }





        }


        #region Commands
        [ICommand]
        public void AddGoodToListCommand(Goods goods)
        {
            Add_Goods_To_List(goods);
        }

        [ICommand]
        public void EditGoodToListCommand(Goods goods)
        {
            Edit_Goods_To_List(goods);
        } // zmien tu trochę

        [ICommand]
        public void RemoveGoodFromListCommand(Goods goods)
        {
            DeleteGoods(goods);
        }

        [ICommand]
        public void SubmitInvoiceCommand()
        {
            MetodOfSave = new SaveAsPdf(firma.DocumentNumbering, Selected_Kontrahent, firma.CompanyData!, firma.BossData, firma.BankAccount,
                List_Of_Added_Goods.ToList(), DataWystawienia, Invoice_Format, Invoice_Number);
            MetodOfSave.SaveToFile();
            Invoice_Number++;
            Wypisanie_Invoice_Format();
        }

        #endregion



        #region Metody
        private void DeleteGoods(Goods goods_To_Remove)
        {
            //MessageBox.Show($"{Product_Code.ToString}", "Testowo", MessageBoxButton.OK, MessageBoxImage.Information);

            List_Of_Added_Goods.Remove(goods_To_Remove);
        }

        private void Edit_Goods_To_List(Goods goods_To_Edit)
        {
            var find_Item = List_Of_Added_Goods.Where(r => r.Product_Code == goods_To_Edit.Product_Code);
            foreach (var item in find_Item)
            {
                List_Of_Added_Goods.Remove(item);
            }


        }

        private void Add_Goods_To_List(Goods goods_To_Add)
        {

            if (Selected_Goods != null)
            {
                Selected_Goods.Quantity = _quantity;
                Selected_Goods.Sum = (_quantity * Selected_Goods.Price_Brutto);
                List_Of_Added_Goods.Add(goods_To_Add);
            }
            else
            {
                //MessageBox.Show($"Nie wybrano towaru lub usługi", "Błąd", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }



        private void SaveToLocal(int index)
        {
            Selected_Goods = firma.goods[index];
        }

        private void Wypisanie_Invoice_Format()
        {
            if (firma.DocumentNumbering != null)
            {
                if (firma.DocumentNumbering.Broken_By_Year && firma.DocumentNumbering.Broken_By_Mounth)
                {
                    Invoice_Format = $"FS {Invoice_Number}/{DataWystawienia.ToString("MM")}/{DataWystawienia.ToString("yyyy")}";
                }
                else if (firma.DocumentNumbering.Broken_By_Year)
                {
                    Invoice_Format = $"FS {Invoice_Number}/{DataWystawienia.ToString("yyyy")}";
                }
                else if (firma.DocumentNumbering.Broken_By_Mounth)
                {
                    Invoice_Format = $"FS {Invoice_Number}/{DataWystawienia.ToString("MM")}";
                }
                else
                {
                    Invoice_Format = $"FS {Invoice_Number}";
                }
            }
            else
            {
                Invoice_Format = $"FS {Invoice_Number}";
            }

        }
        #endregion
    }
}
