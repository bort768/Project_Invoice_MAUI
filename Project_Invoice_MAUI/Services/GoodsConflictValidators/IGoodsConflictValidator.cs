using Project_Invoice_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.Services.GoodsConflictValidators
{
    public interface IGoodsConflictValidator
    {
        Task<Goods> GetConflictingGoods(Goods goods);
    }
}
