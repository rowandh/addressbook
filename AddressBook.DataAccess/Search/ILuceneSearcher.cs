namespace AddressBook.DataAccess.Search
{
    public interface ILuceneSearcher
    {
        SearchResult<ContactDocument> Search(string searchQuery);
    }
}