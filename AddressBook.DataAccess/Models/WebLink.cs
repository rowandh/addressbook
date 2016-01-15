namespace AddressBook.DataAccess.Models
{
    public class WebLink : Entity
    {
        public string Url { get; set; }

        public int WebLinkTypeId { get; set; }

        public virtual WebLinkType WebLinkType { get; set; }

        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
