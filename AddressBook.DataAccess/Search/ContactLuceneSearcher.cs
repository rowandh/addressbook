using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Mapping;
using Version = Lucene.Net.Util.Version;

namespace AddressBook.DataAccess.Search
{
    public class ContactLuceneSearcher : ILuceneSearcher
    {
        public SearchResult<ContactDocument> Search(string searchQuery)
        {
            using (var provider = new LuceneDataProvider(LuceneSearch.LuceneDirectory, Version.LUCENE_30))
            {
                provider.Settings.EnableMultipleEntities = false;

                var queryParser = new MultiFieldParser<ContactDocument>(Version.LUCENE_30, 
                    new ReflectionDocumentMapper<ContactDocument>(Version.LUCENE_30));
                
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