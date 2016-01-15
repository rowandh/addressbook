using AddressBook.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace AddressBook
{
	public partial class Startup
	{
	    public void ConfigureAuth(IAppBuilder app)
	    {
            app.CreatePerOwinContext(AddressBookContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

			// Enable the application to use a cookie to store information for the signed in user
	        app.UseCookieAuthentication(new CookieAuthenticationOptions
	        {
	            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
	            LoginPath = new PathString("/Account/Login")
	        });

			// Use a cookie to temporarily store information about a user logging in with a third party login provider
	        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
	    }
	}
}