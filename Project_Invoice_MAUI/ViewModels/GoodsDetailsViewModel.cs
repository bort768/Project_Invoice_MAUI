using Project_Invoice_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.ViewModels
{
    [QueryProperty("Goods", "Goods")]
    public partial class GoodsDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Goods goods;
    }
}
