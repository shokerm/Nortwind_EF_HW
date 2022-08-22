using System;
using System.Collections.Generic;

namespace Nortwind_EF_HW.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
    }
}
