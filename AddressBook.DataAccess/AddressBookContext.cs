using System.Data.Entity;
using AddressBook.DataAccess.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AddressBook.DataAccess
{
    public class AddressBookContext : IdentityDbContext<ApplicationUser>
    {
        public AddressBookContext()
            : base("name=AddressBookContext")
        {
        }

        public AddressBookContext(string connectionString)
            : base(connectionString)
        {
        }

        public static AddressBookContext Create()
        {
            return new AddressBookContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<IdentityRole>()
        //        .ToTable("Roles")
        //        .Property(c => c.Name)
        //        .HasMaxLength(128)
        //        .IsRequired();

        //    modelBuilder.Entity<ApplicationUser>()
        //        .ToTable("Users")
        //        .Property(c => c.UserName)
        //        .HasMaxLength(128)
        //        .IsRequired();

        //    modelBuilder.Entity<IdentityUserRole>()
        //        .ToTable("UserRoles");

        //    modelBuilder.Entity<IdentityUserClaim>()
        //        .ToTable("UserClaims");

        //    modelBuilder.Entity<IdentityUserLogin>()
        //        .ToTable("UserLogins");
        //}

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<ContactEmailAddress> EmailAddresses { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public virtual DbSet<ContactPhoneNumber> PhoneNumbers { get; set; }
        public virtual DbSet<WebLink> WebLinks { get; set; }
        public virtual DbSet<WebLinkType> WebLinkTypes { get; set; }
        public virtual DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
    }
}
