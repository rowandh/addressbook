using System.Collections.Generic;

namespace AddressBook.DataAccess.Search
{
    public class SearchResult<T>
    {
        public string SearchTerm { get; set; }

        public List<T> Results { get; set; }

        public int Hits { get; set; }
    }
}