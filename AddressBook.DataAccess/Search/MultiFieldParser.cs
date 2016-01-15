using System;
using System.Linq;
using Lucene.Net.Linq.Mapping;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace AddressBook.DataAccess.Search
{
    public class MultiFieldParser<T> : MultiFieldQueryParser
    {
        public MultiFieldParser(Version matchVersion, IDocumentMapper<T> mapper) 
            : base(matchVersion, mapper.IndexedProperties.ToArray(), mapper.Analyzer)
        {
        }

        public override Query Parse(string query)
        {
            var phrase = string.Format("\"{0}\"", query);

            // Split terms on spaces or hyphens, then wildcard search all terms
            var terms = query.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim() + "*")
                .ToList();

            // Add an exact match phrase search
            terms.Add(phrase);
            
            query = string.Join(" ", terms);

            try
            {
                return base.Parse(query);
            }
            catch (ParseException e)
            {
                return base.Parse("");
            }
            catch (Exception e)
            {
                return base.Parse("");
            }
        }
    }
}