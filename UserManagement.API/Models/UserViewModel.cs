
using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public AddressViewModel Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public CompanyViewModel Company { get; set; }
    }

    public class AddressViewModel
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string GeoLat { get; set; }
        public string GeoLng { get; set; }
    }

    public class CompanyViewModel
    {
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        public string Bs { get; set; }
    }

    public class RegisterUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$",
        ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^(https?://)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*/?$",
        ErrorMessage = "Invalid website URL")]
        public string Website { get; set; }

        public AddressViewModel Address { get; set; }

        public CompanyViewModel Company { get; set; }
    }
}
