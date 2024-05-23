using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JProject.Models
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal LimitNumber { get; set; }
        [Required]
        public decimal TaxRate { get; set; }
    }
}
 

