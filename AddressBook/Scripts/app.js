ko.subscribable.fn.subscribeChanged = function (callback) {
    var savedValue = this.peek();
    return this.subscribe(function (latestValue) {
        var oldValue = savedValue;
        savedValue = latestValue;
        callback(latestValue, oldValue);
    });
};

var ContactsSearchViewModel = function() {
    var self = this;

    self.contacts = ko.observableArray([]);

    self.groupedContacts = ko.computed(function() {
        var rows = [],
            current = [];
        rows.push(current);

        for (var i = 0; i < self.contacts().length; i += 1) {
            current.push(self.contacts()[i]);

            if (!((i + 1) % 6)) {
                current = [];
                rows.push(current);
            }
        }

        return rows;
    });

    // This term is bound in two places - the search box, which changes the URL, which changes the search 
    // box...
    // KO doesn't update the bindings if the value hasn't changed, otherwise we would enter an infinite loop
    self.searchTerm = ko.observable();

    self.searchTerm.extend({ rateLimit: { timeout: 500, method: "notifyWhenChangesStop" } });

    self.searchTerm.subscribe(function (term) {
        // Replace certain special chars
        var trimmed = $.map(term.split(" "), function(item) {
                    return $.trim(item.replace(/([\!\-\/])/g, " "));
            })
            .filter(function (el) { return el.length > 0; })
            .join(" ");

        location.hash = trimmed ? 'search/' + trimmed : '';
    });

    self.selectedContactId = ko.observable();

    self.getIconClass = function(type) {
        switch (type) {
        case "Landline":
            return "mdi-communication-phone";

        case "Mobile":
            return "mdi-communication-stay-current-portrait";

        case "Fax":
            return "mdi-action-print";

        default:
            return "mdi-communication-phone";
        }
    };

    self.contactsLoading = ko.observable(false);

    self.searchParams = ko.observable({
        url: '/api/Contacts',
        params: {
            page: 1,
            pageSize: 12
        }
    });

    self.total = 0;

    function loadContacts(searchParams) {

        var refreshSource = ko.unwrap(searchParams);

        self.contactsLoading(true);

        $.ajax({
                url: refreshSource.url,
                data: refreshSource.params
            })
            .done(function(data) {
                var contacts = self.contacts();
                var newContacts = data.contacts || [];
                self.total = data.total;

                for (var i = 0; i < newContacts.length; i++) {
                    contacts.push(new Contact(newContacts[i]));
                }

                self.contacts.valueHasMutated();
            })
            .always(function() {
                self.contactsLoading(false);
            });
    };

    self.searchParams.subscribe(loadContacts);

    self.getNextPage = function() {
        // This is susceptible to repeatedly reloading the page once the end of the 
        // infinite scroll is reached, but the API currently doesn't return total results so
        // there's no way around this
        var refreshSource = self.searchParams();

        // Check if we've loaded all the contacts already
        if (self.total === self.contacts().length)
            return;

        refreshSource.params["page"] += 1;

        self.searchParams(refreshSource);
    };

    function clearContacts() {
        // Clear contacts
        self.contacts.removeAll();
    }

    $.sammy(function() {
        this.get('#search/:searchTerm', function () {
            var term = $.trim(this.params.searchTerm);

            clearContacts();

            var refreshSource = self.searchParams();

            refreshSource.url = '/api/Contacts/Search';
            refreshSource.params["page"] = 1;
            refreshSource.params["searchTerm"] = term;

            self.searchParams(refreshSource);
            self.searchTerm(term);
        });

        this.get('#state/:state/lastname/:lastName', function () {
            var state = $.trim(this.params.state);
            var lastName = $.trim(this.params.lastName);

            clearContacts();

            var refreshSource = self.searchParams();

            refreshSource.url = '/api/Contacts/State/' + state + '/LastName/' + lastName;
            refreshSource.params = {
                page: 1,
                pageSize: refreshSource.params.pageSize
            };

            self.searchParams(refreshSource);
        });

        this.get('#edit/:id', function () {
            clearContacts();
        });

        this.get('/', function () {

            clearContacts();

            var refreshSource = self.searchParams();

            refreshSource.url = '/api/Contacts';
            refreshSource.params = {
                page: 1,
                pageSize: refreshSource.params.pageSize
            };

            self.searchParams(refreshSource);
        });
    }).run();
};

function Contact(data) {
    this.id = data.id;
    this.fullName = data.fullName;
    this.title = data.title;
    this.department = data.department;
    this.location = data.location;
    this.organisation = data.organisation;
    this.imageUrl = data.imageUrl;
    this.phoneNumbers = $.map(data.phoneNumbers, function (item) {
        return new PhoneNumber(item);
    });
    this.extension = data.extension;
    this.email = data.email;
    this.webLinks = $.map(data.webLinks, function (item) {
        return new WebLink(item);
    });
};

function WebLink(data) {
    this.url = data.url;
    this.type = data.type;
}

function PhoneNumber(data) {
    this.phoneNumber = data.phoneNumber;
    this.phoneNumberType = data.phoneNumberType;
}