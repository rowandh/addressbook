using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AddressBook.DataAccess;
using AddressBook.DataAccess.Models;
using AddressBook.DataAccess.Search;
using AddressBook.Services;

namespace AddressBook.Controllers
{
    [RoutePrefix("api/Contacts")]
    public class ContactsApiController : ApiController
    {
        private readonly ContactsService service;

        public ContactsApiController()
        {
            this.service = new ContactsService("AddressBookContext");
        }

        // GET: api/Contacts
        [Route]
        public PagedContactQueryResult GetContacts(int page = 1, int pageSize = 50)
        {
            return service.GetContacts(page, pageSize);
        }
        
        [Route("State/{state:alpha}/LastName/{lastName:alpha:maxlength(1)}")]
        public PagedContactQueryResult GetStateLastName(string state, string lastName, int page = 1, int pageSize = 50)
        {
            return service.GetContactsByStateAndLastName(state, lastName, page, pageSize);
        }


        [Route("Search")]
        public PagedContactQueryResult GetSearch(string searchTerm, int page = 1, int pageSize = 50)
        {
            return service.SearchContacts(searchTerm, page, pageSize);
        }

        // GET: api/Contacts/5
        [Route("{id}")]
        [ResponseType(typeof(ContactViewModel))]
        public IHttpActionResult GetContact(int id)
        {
            var contact = service.GetContactDetails(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }               
    }
}