namespace AddressBook.DataAccess.Models
{
    /// <summary>
    /// An external phone number
    /// </summary>
    public class ContactPhoneNumber : Entity
    {
        public string PhoneNumber { get; set; }

        public int PhoneNumberTypeId { get; set; }

        public virtual PhoneNumberType PhoneNumberType { get; set; }

        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}