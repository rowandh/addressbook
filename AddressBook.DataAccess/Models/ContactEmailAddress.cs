namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// An email address
    /// </summary>
    public class ContactEmailAddress : Entity
    {
        public string Email { get; set; }

        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}