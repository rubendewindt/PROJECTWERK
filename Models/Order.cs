using System.ComponentModel.DataAnnotations.Schema;

namespace projectwerk.Models
{
        public class Order
        {
            public int Id { get; set; }

            public DateTime OrderPlaced { get; set; }

            public string Name { get; set; } 
            public int Quantity { get; set; }

            [Column(TypeName = "decimal(6, 2)")]
            public decimal Price { get; set; }
    }
  
}
