using Microsoft.AspNetCore.Identity;

namespace FitnessCalculationService.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string ?Address { get; set; }

        public string Email { get; set; }

    }
}
