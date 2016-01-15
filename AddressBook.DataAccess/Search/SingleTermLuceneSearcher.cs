using System.Linq;
using Lucene.Net.Linq;
using Lucene.Net.Util;

namespace AddressBook.DataAccess.Search
{
    public class SingleTermLuceneSearcher : ILuceneSearcher
    {
        public SearchResult<ContactDocument> Search(string searchQuery)
        {
            using (var provider = new LuceneDataProvider(LuceneSearch.LuceneDirectory, Version.LUCENE_30))
            {
                provider.Settings.EnableMultipleEntities = false;

                var queryParser = provider.CreateQueryParser<ContactDocument>();
                queryParser.DefaultSearchProperty = "All";
                queryParser.DefaultOperator = Lucene.Net.QueryParsers.QueryParser.Operator.AND;

                var query = queryParser.Parse(searchQuery);

                var queryable = provider.AsQueryable<ContactDocument>();

                var results = queryable.Where(query);

                return new SearchResult<ContactDocument>
                {
                    SearchTerm = searchQuery,
                    Results = results.ToList()
                };
            }
        }
    }
}