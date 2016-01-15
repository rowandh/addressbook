using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.DataAccess.Models;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System.Data.Entity;
using System.IO;
using Lucene.Net.Linq;
using Lucene.Net.Store;

namespace AddressBook.DataAccess.Search
{
    /// <summary>
    /// Indexes the address book database for Lucene
    /// </summary>
    public class AddressBookLuceneIndexer
    {
        private readonly string connectionString;

        public AddressBookLuceneIndexer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InitializeIndex()
        {
            var context = new AddressBookContext(connectionString);
            var contacts = context.Contacts
                .Include(c => c.Organisation)
                .Include(c => c.Department)
                .Include(c => c.Office.Address)
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses);
            
            using (var provider = new LuceneDataProvider(LuceneSearch.LuceneDirectory, Lucene.Net.Util.Version.LUCENE_30))
            {
                provider.Settings.EnableMultipleEntities = false;

                using (var session = provider.OpenSession<ContactDocument>())
                {
                    var docs = contacts.Select(LuceneSearch.MapContactToDocument).ToArray();
                    
                    session.DeleteAll();
                    session.Add(docs);
                }
            }            
        }
    }
}
