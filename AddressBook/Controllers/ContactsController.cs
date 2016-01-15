using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AddressBook.DataAccess;
using AddressBook.DataAccess.Models;
using AddressBook.DataAccess.Search;
using AddressBook.Models;
using AddressBook.Services;

namespace AddressBook.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private AddressBookContext db = new AddressBookContext();
        private readonly ContactsService service;

        public ContactsController()
        {
            this.service = new ContactsService("AddressBookContext");
        }

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            var contacts = db.Contacts.Include(c => c.Department).Include(c => c.Office);
            return View(await contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            var viewModel = new CreateEditContactViewModel
            {
                WebLinkTypes = db.WebLinkTypes.ToList(),
                PhoneNumberTypes = db.PhoneNumberTypes.ToList(),
                DepartmentIdSelect = new SelectList(db.Departments, "Id", "Name"),
                OfficeIdSelect = new SelectList(db.Offices, "Id", "Name"),
                OrganisationIdSelect = new SelectList(db.Organisations, "Id", "Name")
            };

            return View(viewModel);
        }

        private CreateEditContactViewModel MapToViewModel(Contact contact)
        {
            return new CreateEditContactViewModel
            {
                Id = contact.Id,
                FullName = contact.FullName,
                LastName = contact.LastName,
                Title = contact.Title,
                Email = contact.Email,
                Image = contact.Image,
                DepartmentId = contact.DepartmentId ?? -1,
                OfficeId = contact.OfficeId,
                OrganisationId = contact.OrganisationId ?? -1,
                PhoneNumbers = contact.PhoneNumbers.Select(p => new CreateEditPhoneNumberViewModel
                {
                    Id = p.Id,
                    ContactId = contact.Id,
                    PhoneNumber = p.PhoneNumber,
                    PhoneNumberTypeId = p.PhoneNumberTypeId
                }),
                WebLinks = contact.WebLinks.Select(w => new CreateEditWebLinkViewModel
                {
                    Id = w.Id,
                    ContactId = contact.Id,
                    Url = w.Url,
                    WebLinkTypeId = w.WebLinkTypeId
                }),
                WebLinkTypes = db.WebLinkTypes.ToList(),
                PhoneNumberTypes = db.PhoneNumberTypes.ToList(),
                DepartmentIdSelect = new SelectList(db.Departments, "Id", "Name", contact.DepartmentId),
                OfficeIdSelect = new SelectList(db.Offices, "Id", "Name", contact.OfficeId),
                OrganisationIdSelect = new SelectList(db.Organisations, "Id", "Name", contact.OrganisationId)
            };
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,LastName,Title,Email,PhoneNumbers,WebLinks,Image,File,DepartmentId,OfficeId,OrganisationId")] CreateEditContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                service.CreateContact(contact, HostingEnvironment.MapPath("~/uploads"));

                return RedirectToAction("Index", "Home");
            }

            contact.WebLinkTypes = db.WebLinkTypes.ToList();
            contact.PhoneNumberTypes = db.PhoneNumberTypes.ToList();
            contact.DepartmentIdSelect = new SelectList(db.Departments, "Id", "Name");
            contact.OfficeIdSelect = new SelectList(db.Offices, "Id", "Name");
            contact.OrganisationIdSelect = new SelectList(db.Organisations, "Id", "Name");

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contact = await db.Contacts.FindAsync(id);

            if (contact == null)
            {
                return HttpNotFound();
            }

            var viewModel = MapToViewModel(contact);

            return View(viewModel);
        }


        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,LastName,Title,Email,PhoneNumbers,WebLinks,Image,File,DepartmentId,OfficeId,OrganisationId")] CreateEditContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                service.UpdateContact(contact, HostingEnvironment.MapPath("~/uploads"));                

                return RedirectToAction("Index", "Home");
            }

            //var viewModel = MapToViewModel(contact);

            return View();
        }

        // GET: Contacts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();

            // Update Lucene index
            var luceneUpdate = new DeleteLuceneContact(contact);
            luceneUpdate.Execute();

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
