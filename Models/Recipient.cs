using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JProject.Models
{
    public class Recipient
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PassportNumber { get; set; }
        public string PersonalNumber { get; set; }
        public string PhoneNumber { get; set; }

    }
}
