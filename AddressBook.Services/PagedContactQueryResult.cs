using System.Collections.Generic;
using AddressBook.DataAccess.Models;

namespace AddressBook.Services
{
    public class PagedContactQueryResult
    {
        public PagedContactQueryResult(int page, int pageSize, int total, IEnumerable<ContactViewModel> contacts)
        {
            Page = page;
            PageSize = pageSize;
            Contacts = contacts;
            Total = total;
        }

        public int Page { get; private set; }

        public int PageSize { get; private set; }

        public int Total { get; private set; }

        public IEnumerable<ContactViewModel> Contacts { get; private set; }
    }
}