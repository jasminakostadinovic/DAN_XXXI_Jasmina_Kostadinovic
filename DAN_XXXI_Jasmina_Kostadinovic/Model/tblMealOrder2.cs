using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_XXXI_Jasmina_Kostadinovic.Model
{
    public partial class tblMealOrder
    {
        public tblMealOrder(tblMeal meal, tblOrder order)
        {
            tblMeal = meal;
            tblOrder = order;
        }
    }
}
