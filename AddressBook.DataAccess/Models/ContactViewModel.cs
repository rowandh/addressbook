using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.DataAccess.Models
{
    public class ContactViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string Organisation { get; set; }

        public string Department { get; set; }

        public string Location { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public IEnumerable<ContactPhoneNumberViewModel> PhoneNumbers { get; set; }

        public IEnumerable<WebLinkViewModel> WebLinks { get; set; }
    }

    public class WebLinkViewModel
    {
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class ContactPhoneNumberViewModel
    {
        public string PhoneNumber { get; set; }

        public string PhoneNumberType { get; set; }
    }
}
