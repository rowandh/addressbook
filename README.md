# Address Book #
Address book is a simple ASP.NET MVC 5 web application for managing contacts.
It includes a full-text search index, allowing contacts to be found
with simple search queries such as their name, location, department or phone extension.

The AddressBook UI has been designed with mobile devices in mind. Views are responsive 
and scale to the device thanks to the Materialize framework.

The main search page uses knockoutjs for data binding. An ASP.NET Web API provides a queryable 
JSON resource.

## Full-Text Search ##
Full-text search is implemented using Lucene.Net. The Lucene index is comprised of ContactDocuments.

A "ContactDocument" model is a flattened version of a contact stored in the address book database.

When creating, updating or deleting a contact, the Lucene index should also be updated, otherwise search
queries will return incorrect results.

The contacts database is re-indexed whenever the application starts. IIS periodically recycles 
App Pools, so this should occur regularly.

## Deployment ##
Make sure the IIS user has full permissions for the ~/lucene_index and ~/uploads folders.

## TODO ##
There's a lot to do to get this to a level I'd be happy with. Some things I can think of,
in no particular order:
* Handle Lucene indexing better. Currently, the entire index is rebuilt every time the app pool is 
recycled.
* Add error checking and transaction binding between lucene and the database. Currently, a record could
be added to the database, but the lucene update could subsequently fail unchecked, making the two inconsistent.
* Add UI for creating or editing offices, departments, organisations, phone number types, or web
link types.
* Image uploads are handled poorly.
* Improve account registration processed. The registration page is hidden, but anyone could find
it and have full control over editing address book entries.
* Clean up javascript code. Use bundling.
* Refactor a bunch of other stuff