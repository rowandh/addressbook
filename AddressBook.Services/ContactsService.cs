using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using AddressBook.DataAccess;
using AddressBook.DataAccess.Models;
using AddressBook.DataAccess.Search;
using static AddressBook.Services.ObjectExtensions;

namespace AddressBook.Services
{
    public class ContactsService
    {
        private readonly string nameOrConnectionString;

        public ContactsService(string nameOrConnectionString)
        {
            this.nameOrConnectionString = nameOrConnectionString;
        }

        public void CreateContact(CreateEditContact contact, string uploadPathBase)
        {
            using (var context = new AddressBookContext(nameOrConnectionString))
            {
                if (contact.File != null && contact.File.ContentLength > 0)
                {
                    contact.Image = Guid.NewGuid() + Path.GetExtension(contact.File.FileName);                    
                    contact.File.SaveAs(Path.Combine(uploadPathBase, contact.Image));
                }

                var mapped = new DataAccess.Models.Contact
                {
                    Id = contact.Id,
                    FullName = contact.FullName,
                    LastName = contact.LastName,
                    Title = contact.Title,
                    Email = contact.Email,
                    OfficeId = contact.OfficeId,
                    DepartmentId = contact.DepartmentId,
                    OrganisationId = contact.OrganisationId,
                    Image = contact.Image,
                    WebLinks = contact.WebLinks
                        .Where(w => !string.IsNullOrWhiteSpace(w.Url))
                        .Select(w => new WebLink {  Id = w.Id, ContactId = w.ContactId, WebLinkTypeId = w.WebLinkTypeId})
                        .ToList(),
                    PhoneNumbers = contact.PhoneNumbers
                        .Where(p => !string.IsNullOrWhiteSpace(p.PhoneNumber))
                        .Select(p => new ContactPhoneNumber {  Id = p.Id, ContactId = p.ContactId, PhoneNumberTypeId = p.PhoneNumberTypeId})
                        .ToList()
                };

                context.Contacts.Add(mapped);
                context.SaveChanges();

                // Update the lucene index
                var luceneUpdate = new CreateUpdateLuceneContact(mapped);
                luceneUpdate.Execute();
            }
        }

        public void UpdateContact(CreateEditContact contact, string uploadPath)
        {
            var mappedContact = new DataAccess.Models.Contact
            {
                Id = contact.Id,
                FullName = contact.FullName,
                LastName = contact.LastName,
                Email = contact.Email,
                Title = contact.Title,
                DepartmentId = contact.DepartmentId,
                OfficeId = contact.OfficeId,
                OrganisationId = contact.OrganisationId,
                Image = contact.Image,
                PhoneNumbers = contact.PhoneNumbers
                    .Where(p => !string.IsNullOrWhiteSpace(p.PhoneNumber))
                    .Select(p => new ContactPhoneNumber {  Id = p.Id, ContactId = p.ContactId, PhoneNumber = p.PhoneNumber, PhoneNumberTypeId = p.PhoneNumberTypeId})
                    .ToList(),
                WebLinks = contact.WebLinks
                    .Where(w => !string.IsNullOrWhiteSpace(w.Url))
                    .Select(w => new WebLink {  Id = w.Id, ContactId = w.ContactId, Url = w.Url, WebLinkTypeId = w.WebLinkTypeId})
                    .ToList()
            };

            using (var context = new AddressBookContext(nameOrConnectionString))
            {
                var originalContact = context
                    .Contacts
                    .Include(c => c.PhoneNumbers)
                    .Include(c => c.WebLinks)
                    .FirstOrDefault(c => c.Id == mappedContact.Id);

                if (originalContact == null)
                    throw new Exception($"No contact with id {mappedContact.Id} exists");

                var originalPhoneNumbers = originalContact
                    .PhoneNumbers
                    .ToList();

                var deletedPhoneNumbers = originalPhoneNumbers
                    .Except(mappedContact.PhoneNumbers, c => c.Id)
                    .ToList();

                var addedPhoneNumbers = mappedContact
                    .PhoneNumbers
                    .Where(p => p.Id == 0)
                    .ToList();

                var modifiedPhoneNumbers = mappedContact
                    .PhoneNumbers
                    .Except(deletedPhoneNumbers, c => c.Id)
                    .Except(addedPhoneNumbers, c => c.Id)
                    .ToList();

                foreach (var delete in deletedPhoneNumbers)
                {
                    context.Entry(delete).State = EntityState.Deleted;
                }

                foreach (var added in addedPhoneNumbers)
                {
                    context.Entry(added).State = EntityState.Added;
                }

                // Get existing items to update
                foreach (var modified in modifiedPhoneNumbers)
                {
                    var existing = context.PhoneNumbers.Find(modified.Id);
                    if (existing == null)
                        continue;

                    context.Entry(existing).CurrentValues.SetValues(modified);
                }

                var originalWebLinks = originalContact
                    .WebLinks
                    .ToList();

                var deletedWebLinks = originalWebLinks
                    .Except(mappedContact.WebLinks, c => c.Id)
                    .ToList();

                var addedWebLinks = mappedContact.WebLinks
                    .Where(c => c.Id == 0)
                    .ToList();

                var modifiedWebLinks = mappedContact.WebLinks
                    .Except(deletedWebLinks, c => c.Id)
                    .Except(addedWebLinks, c => c.Id)
                    .ToList();

                foreach (var delete in deletedWebLinks)
                {
                    context.Entry(delete).State = EntityState.Deleted;
                }

                foreach (var added in addedWebLinks)
                {
                    context.Entry(added).State = EntityState.Added;
                }

                // Get existing items to update
                foreach (var modified in modifiedWebLinks)
                {
                    var existing = context.WebLinks.Find(modified.Id);
                    if (existing == null)
                        continue;

                    context.Entry(existing).CurrentValues.SetValues(modified);
                }

                // If we have an image attached
                if (contact.File != null && contact.File.ContentLength > 0)
                {
                    var uploadedImageName = Guid.NewGuid() + Path.GetExtension(contact.File.FileName);

                    mappedContact.Image = uploadedImageName;

                    contact.File.SaveAs(Path.Combine(uploadPath, uploadedImageName));

                    // Delete the original
                    DeleteImage(originalContact.Image, uploadPath);
                }

                // If no image is attached it means it should be deleted (if possible)
                if (string.IsNullOrWhiteSpace(contact.Image))
                {
                    // Delete the original
                    DeleteImage(originalContact.Image, uploadPath);
                }

                // Update the other properties with the new values
                context.Entry(originalContact).CurrentValues.SetValues(mappedContact);

                context.SaveChanges();

                // Update the lucene index
                var luceneUpdate = new CreateUpdateLuceneContact(mappedContact);
                luceneUpdate.Execute();
            }
        }

        private static void DeleteImage(string image, string uploadPath)
        {
            if (image == null)
                return;

            var path = Path.Combine(uploadPath, image);
            if (File.Exists(path))
                File.Delete(path);
        }

        public ContactViewModel GetContactDetails(int id)
        {
            using (var context = new AddressBookContext(nameOrConnectionString))
            {
                return context
                    .Contacts
                    .Include(c => c.PhoneNumbers.Select(w => w.PhoneNumberType))
                    .Include(c => c.Organisation)
                    .Include(c => c.Department)
                    .Include(c => c.Office)
                    .Include(c => c.WebLinks.Select(w => w.WebLinkType))
                    .Select(MapContact)
                    .FirstOrDefault(contact => contact.Id == id);
            }
        }

        public PagedContactQueryResult GetContacts(int page = 1, int pageSize = 50)
        {
            using (var context = new AddressBookContext(nameOrConnectionString))
            {
                var contacts = context.Contacts
                    .OrderBy(c => c.LastName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include(c => c.PhoneNumbers.Select(w => w.PhoneNumberType))
                    .Include(c => c.Organisation)
                    .Include(c => c.Department)
                    .Include(c => c.Office)
                    .Include(c => c.WebLinks.Select(w => w.WebLinkType))
                    .Select(MapContact)
                    .ToList();

                var count = context.Contacts.Count();

                return new PagedContactQueryResult(page, pageSize, count, contacts);
            }
        }

        public PagedContactQueryResult SearchContacts(string searchTerm, int page = 1, int pageSize = 50)
        {
            var luceneSearcher = new SingleTermLuceneSearcher();

            using (var context = new AddressBookContext(nameOrConnectionString))
            {
                var luceneResults = luceneSearcher.Search(searchTerm).Results;

                var contacts = luceneResults
                    .OrderBy(r => r.LastName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(record => context.Contacts
                        .Include(c => c.PhoneNumbers.Select(p => p.PhoneNumberType))
                        .Include(c => c.Organisation)
                        .Include(c => c.Department)
                        .Include(c => c.Office)
                        .Include(c => c.WebLinks.Select(w => w.WebLinkType))
                        .FirstOrDefault(c => c.Id == record.Id))
                    .Where(record => record != null)
                    .Select(MapContact)
                    .ToList();

                var count = luceneResults.Count();

                return new PagedContactQueryResult(page, pageSize, count, contacts);
            }
        }

        public PagedContactQueryResult GetContactsByStateAndLastName(string state, string lastName, int page,
            int pageSize)
        {
            using (var context = new AddressBookContext(nameOrConnectionString))
            {
                var contacts = context.Contacts
                    .Where(c => c.Office.Address.State == state)
                    .Where(c => c.LastName.StartsWith(lastName))
                    .Include(c => c.PhoneNumbers.Select(w => w.PhoneNumberType))
                    .Include(c => c.Organisation)
                    .Include(c => c.Department)
                    .Include(c => c.Office.Address)
                    .Include(c => c.WebLinks.Select(w => w.WebLinkType))
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.Office.Address.State)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(MapContact)
                    .ToList();

                var count = context.Contacts
                    .Where(c => c.Office.Address.State == state)
                    .Count(c => c.LastName.StartsWith(lastName));

                return new PagedContactQueryResult(page, pageSize, count, contacts);
            }
        }

        private ContactViewModel MapContact(DataAccess.Models.Contact c)
        {
            return new ContactViewModel
            {
                Id = c.Id,
                FullName = c.FullName,
                Title = c.Title,
                Organisation = c.Organisation.Name,
                Department = c.Department.Name,
                Location = c.Office.Name,
                PhoneNumbers = c.PhoneNumbers.Select(p => new ContactPhoneNumberViewModel
                {
                    PhoneNumber = p.PhoneNumber,
                    PhoneNumberType = p.PhoneNumberType.Name
                }),
                Email = c.Email,
                WebLinks = c.WebLinks.Select(p => new WebLinkViewModel
                {
                    Url = p.Url,
                    Type = p.WebLinkType.Name
                }),
                ImageUrl =
                        c.Image == null || c.Image.Trim().Length == 0
                            ? null
                            : "/uploads/" + c.Image
            };
        }
    }
}
