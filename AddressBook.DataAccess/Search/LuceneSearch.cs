using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using AddressBook.DataAccess.Models;
using Lucene.Net.Analysis;
using Lucene.Net.Store;

namespace AddressBook.DataAccess.Search
{
    public static class LuceneSearch
    {
        private static FSDirectory _directoryTemp;

        public static FSDirectory LuceneDirectory
        {
            get
            {
                var lucene = HostingEnvironment.MapPath("~/lucene_index");

                return _directoryTemp ?? (_directoryTemp = FSDirectory.Open(new DirectoryInfo(lucene).CreateIfNotExists()));
            }
        }

        private static ISet<string> _stopWords;

        public static ISet<string> StopWords
        {
            get
            {
                if (_stopWords != null) return _stopWords;
                
                            string[] strArray = 
                  {
                    "a",
                    "an",
                    "and",
                    "are",
                    "as",
                    "at",
                    "be",
                    "but",
                    "by",
                    "for",
                    "if",
                    "in",
                    "into",
                    "is",
                    "no",
                    "not",
                    "of",
                    "on",
                    "or",
                    "such",
                    "that",
                    "the",
                    "their",
                    "then",
                    "there",
                    "these",
                    "they",
                    "this",
                    "to",
                    "was",
                    "will",
                    "with"
                  };

                _stopWords = new CharArraySet(strArray, false);

                return _stopWords;
            }
        }

        public static ContactDocument MapContactToDocument(Contact contact)
        {
            var cd = new ContactDocument
            {
                Id = contact.Id,
                FullName = contact.FullName,
                LastName = contact.LastName,
                Title = contact.Title,
                Email = contact.Email,
                OrganisationName = contact.Organisation != null ? contact.Organisation.Name : "",
                OfficeName = contact.Office != null? contact.Office.Name : "",
                State = (contact.Office !=null && contact.Office.Address != null) ? contact.Office.Address.State : "",
                DepartmentName = contact.Department != null? contact.Department.Name  :"",
                PhoneNumbers = contact.PhoneNumbers != null? contact.PhoneNumbers.Select(p => p.PhoneNumber) : Enumerable.Empty<string>(),                
            };

            var phones = string.Join(" ", cd.PhoneNumbers);

            cd.All = string.Join(" ", cd.FullName, cd.Title, cd.Email, cd.OrganisationName, cd.OfficeName, cd.State,
                cd.DepartmentName, phones);

            return cd;
        }
    }
}
