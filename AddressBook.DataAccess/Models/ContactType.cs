namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// The type of phone number, eg. home, mobile, etc.
    /// </summary>
    public class ContactType : Entity
    {
        public string Name { get; set; }
    }
}