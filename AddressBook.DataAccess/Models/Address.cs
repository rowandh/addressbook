namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// An address
    /// </summary>
    public class Address : Entity
    {
        public string StreetAddress { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}