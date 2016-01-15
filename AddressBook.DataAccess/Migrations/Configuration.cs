using System.Collections.Generic;
using AddressBook.DataAccess.Models;

namespace AddressBook.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AddressBook.DataAccess.AddressBookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AddressBook.DataAccess.AddressBookContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );


            context.Offices.AddOrUpdate(
                p => p.Name,
                    new Office { Id = 1, Name = "Test Office", Address = new Address { StreetAddress = "1 Pitt Street", City = "Sydney", PostCode = "2000", State = "NSW" } }
                );

            context.Organisations.AddOrUpdate(
                p => p.Name,
                new Organisation { Id = 1, Name = "Test Organisation" }
                );

            context.Departments.AddOrUpdate(
                p => p.Name,
                new Department { Id = 1, Name = "IT", OrganisationId = 1 }
                );

            var mobileType = new PhoneNumberType { Id = 1, Name = "Mobile" };

            context.PhoneNumberTypes.AddOrUpdate(
                p => p.Id,
                mobileType);

            var phoneExtension1 = new ContactPhoneNumber { Id = 1, PhoneNumber = "4311", PhoneNumberType = mobileType };

            context.PhoneNumbers.AddOrUpdate(
                p => p.PhoneNumber,
                phoneExtension1
                );

            var contact1 = new Contact
            {
                Id = 1,
                FullName = "Test",
                LastName = "Person",
                OfficeId = 1,
                DepartmentId = 1,                
                OrganisationId = 1,
                PhoneNumbers = new List<ContactPhoneNumber>()
            };
            
            contact1.PhoneNumbers.Add(phoneExtension1);

            context.Contacts.AddOrUpdate(
                p => p.Id,
                contact1
                );
        }
    }
}
