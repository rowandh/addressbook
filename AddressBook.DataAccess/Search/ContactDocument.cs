using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Linq.Mapping;

namespace AddressBook.DataAccess.Search
{
    /// <summary>
    /// The contact document used by Lucene for indexing
    /// </summary>
    public class ContactDocument
    {
        [Field(IndexMode.NotAnalyzed, Key = true)]        
        public int Id { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(LowercaseWhitespaceAnalyzer), CaseSensitive = false)]
        public string FullName { get; set; }

        // Need to store for search ordering
        [Field(IndexMode.Analyzed, Store = StoreMode.Yes,Analyzer = typeof(LowercaseWhitespaceAnalyzer), CaseSensitive = false)]
        public string LastName { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(KeywordAnalyzer), CaseSensitive = false)]
        public string State { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(KeywordAnalyzer), CaseSensitive = false)]
        public string Email { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(LowercaseWhitespaceAnalyzer), CaseSensitive = false)]
        public string Title { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(KeywordAnalyzer))]
        public IEnumerable<string> PhoneNumbers { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(KeywordAnalyzer), CaseSensitive = false)]
        public string DepartmentName { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(LowercaseWhitespaceAnalyzer), CaseSensitive = false)]
        public string OfficeName { get; set; }

        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(LowercaseWhitespaceAnalyzer), CaseSensitive = false)]
        public string OrganisationName { get; set; }

        // Concatenated string of all fields. Used to search when a term is not specified
        [Field(IndexMode.Analyzed, Store = StoreMode.No, Analyzer = typeof(LowercaseWhitespaceAnalyzer), CaseSensitive = false)]
        public string All { get; set; }
    }

    /// <summary>
    /// A whitespace analyzer that is case insensitive
    /// </summary>
    public class LowercaseWhitespaceAnalyzer : Analyzer
    {
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            return new LowerCaseFilter(new WhitespaceTokenizer(reader));
        }
    }
}