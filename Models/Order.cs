using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JProject
{
    public class Order
    {
        [Key]
        public int id {  get; set; }
        [Required]
        [ForeignKey("Recipient")]
        public int RecipientId { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal ShippingPrice {  get; set; }
        [Required]
        public DateOnly DeliveryDate { get; set; }
        public string? Comment { get; set; }
    }
}

