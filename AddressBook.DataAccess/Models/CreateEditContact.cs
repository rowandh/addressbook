using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.DataAccess.Models
{
    public class CreateEditContact
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Title { get; set; }

        public int OrganisationId { get; set; }

        public int DepartmentId { get; set; }

        public int OfficeId { get; set; }

        public string Image { get; set; }

        public string Email { get; set; }

        public IEnumerable<CreateEditPhoneNumberViewModel> PhoneNumbers { get; set; }

        public IEnumerable<CreateEditWebLinkViewModel> WebLinks { get; set; }

        public HttpPostedFileBase File { get; set; }
    }

    public class CreateEditPhoneNumberViewModel
    {
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int PhoneNumberTypeId { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class CreateEditWebLinkViewModel
    {
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int WebLinkTypeId { get; set; }

        public string Url { get; set; }
    }
}