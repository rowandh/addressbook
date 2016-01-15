using System.Collections.Generic;

namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// A physical location where a contact can be based
    /// </summary>
    public class Office : Entity
    {
        public string Name { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }
    }
}