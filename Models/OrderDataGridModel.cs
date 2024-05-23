using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JProject.Models
{
    public class OrderDataGridModel
    {
        public int Id { get; set; }
        public string RecipientName { get; set; }
        public string CategoryName { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingPrice { get; set; }
        public string Comment { get; set; }
    }
}


