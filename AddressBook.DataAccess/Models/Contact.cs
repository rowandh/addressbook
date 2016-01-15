using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// An organisational contact
    /// </summary>
    public class Contact : Entity
    {
        public Contact()
        {
            PhoneNumbers = new List<ContactPhoneNumber>();
            WebLinks = new List<WebLink>();
        }

        [Required]
        [Display(Name = "Name")]
        public string FullName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public int? OrganisationId { get; set; }

        // Storing both the organisation and the department on the Contact means we can't
        // select only departments that belong to a particular organisation, but that doesn't
        // matter for this application.
        public virtual Organisation Organisation { get; set; }
        
        public int? DepartmentId { get; set; }

        /// <summary>
        /// The department in which the contact is based
        /// </summary>
        public virtual Department Department { get; set; }

        public int OfficeId { get; set; }

        /// <summary>
        /// The office in which the contact is based
        /// </summary>
        public virtual Office Office { get; set; }

        /// <summary>
        /// A contact can have multiple email addresses, but an email address can only have one contact
        /// </summary>
        public virtual ICollection<ContactEmailAddress> EmailAddresses { get; set; }

        /// <summary>
        /// Phone numbers belonging to the person
        /// </summary>
        public virtual ICollection<ContactPhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Website links belonging to the person
        /// </summary>
        public virtual ICollection<WebLink> WebLinks { get; set; }
    }
}
