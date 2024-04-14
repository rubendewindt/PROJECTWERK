using Microsoft.AspNetCore.Identity;

namespace projectwerk.Models
{

        public class User 
    {
            public int Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Address { get; set; }

            public string Phone { get; set; }

            public string Email { get; set; }

            public string Password { get; set; } = null!;


          

        }
    
}
