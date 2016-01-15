using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook.DataAccess.Models;
using Lucene.Net.Linq;

namespace AddressBook.DataAccess.Search
{
    public class CreateUpdateLuceneContact
    {
        private readonly Contact contact;

        public CreateUpdateLuceneContact(Contact contact)
        {
            this.contact = contact;
        }

        public void Execute()
        {
            using (var provider = new LuceneDataProvider(LuceneSearch.LuceneDirectory, Lucene.Net.Util.Version.LUCENE_30))
            {
                provider.Settings.EnableMultipleEntities = false;

                using (var session = provider.OpenSession<ContactDocument>())
                {
                    var mapped = LuceneSearch.MapContactToDocument(contact);
                    session.Delete(mapped);
                    session.Add(mapped);
                }
            }    
        }
    }
}
