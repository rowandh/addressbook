namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// An organisational department
    /// </summary>
    public class Department : Entity
    {
        public string Name { get; set; }

        public int OrganisationId { get; set; }

        public virtual Organisation Organisation { get; set; }
    }
}