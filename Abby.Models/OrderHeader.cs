using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
       
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderCreatedDate { get; set; }

        [Required]
        [DisplayName("Order Total")]
        [DisplayFormat(DataFormatString ="{0:C2}")]
        public double OrderTotal { get; set; }

        [Required]
        [DisplayName("Pick Up Time")]
        public DateTime PickupTime { get; set; }

        [Required]
        [NotMapped]
        [DisplayName("Pick Up Date")]
        public DateTime PickupDate { get; set; }

        public string Status { get; set; }

        public string? Comments { get; set; }

        public string? TransactionId { get; set; }

        [DisplayName("Pick Up Name")]
        [Required]
        public string PickUpName { get; set; }

        [DisplayName("Pick Up Phone Number")]
        [Required]
        public string PickUpPhoneNumber { get; set; }
    }
}
