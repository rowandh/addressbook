using System.Collections.Generic;

namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// An organisation within the business
    /// </summary>
    public class Organisation : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}