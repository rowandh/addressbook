using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AddressBook.DataAccess.Models;

namespace AddressBook.Models
{
    public class CreateEditContactViewModel : CreateEditContact
    {
        public List<WebLinkType> WebLinkTypes { get; set; }

        public List<PhoneNumberType> PhoneNumberTypes { get; set; }

        public SelectList DepartmentIdSelect { get; set; }

        public SelectList OfficeIdSelect { get; set; }

        public SelectList OrganisationIdSelect { get; set; }
    }
}