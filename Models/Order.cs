using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lista10.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string NIP { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string HomeNumber { get; set; }

        public string LocalNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Invalid ZIP Code format.")]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Points must be non-negative.")]
        public int Points { get; set; }

        [Required]
        public string DeliveryMethod { get; set; }

        public string DeliveryPoint { get; set; }

        public string PaymentMethod { get; set; }

        public List<CartItem> OrderDetails { get; set; }

        public bool IsConfirmed { get; set; }

        public decimal Price { get; set; }

        public DateTime ExpiryMagazineDate { get; set; }

        public DateTime CollectionDate { get; set; }
    }
}
