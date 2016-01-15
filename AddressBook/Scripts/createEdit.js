var PhoneNumber = function(data) {
    var self = this;
    self.id = data ? data.id : 0;
    self.contactId = data ? data.contactId : 0;
    self.phoneNumber = data ? data.phoneNumber : '';
    self.phoneNumberTypeId = data ? data.phoneNumberTypeId : '';
};

var WebLink = function(data) {
    var self = this;

    self.id = data ? data.id : 0;
    self.webLinkTypeId = data ? data.webLinkTypeId : 1;
    self.url = data ? data.url : '';
    self.contactId = data ? data.contactId : 0;
}

var CreateEditContact = function (data) {
    var self = this;

    self.id = ko.observable(data ? data.id : 0);
    self.fullName = ko.observable(data ? data.fullName : '');
    self.lastName = ko.observable(data ? data.lastName : '');
    self.title = ko.observable(data ? data.title : '');
    self.email = data ? data.email : '';

    var numbers = data.phoneNumbers ? $.map(data.phoneNumbers, function (phoneNumber) {            
            return new PhoneNumber(phoneNumber);
        })
        : [];

    self.phoneNumbers = ko.observableArray(numbers);

    var webLinks = data.webLinks ? $.map(data.webLinks, function(webLink) {
            return new WebLink(webLink);
        })
        : [];

    self.webLinks = ko.observableArray(webLinks);
};

var CreateEditContactsViewModel = function(data) {
    var self = this;

    self.contact = ko.observable(new CreateEditContact(data.contacts));

    self.webLinkTypes = data.webLinkTypes;

    self.phoneNumberTypes = data.phoneNumberTypes;

    self.addPhone = function() {
        self.contact().phoneNumbers.push(new PhoneNumber());
    };

    self.removePhone = function(phone) {
        self.contact().phoneNumbers.remove(phone);
    };

    self.addWebLink = function() {
        self.contact().webLinks.push(new WebLink());
    };

    self.removeWebLink = function(webLink) {
        self.contact().webLinks.remove(webLink);
    };

    self.save = function() {
        ko.utils.postJson('/Contacts/Create', self.contact);
    };

    self.addPhone();
    self.addWebLink();
};

var CreateEditContactsParameters = function() {
    var self = this;

    self.contacts = [];
    self.phoneNumberTypes = [];
    self.webLinkTypes = [];
};