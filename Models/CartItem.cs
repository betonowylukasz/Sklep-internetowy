using System.ComponentModel.DataAnnotations;

namespace lista10.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public Article Article { get; set; }

        public int Quantity { get; set; }
    }
}
