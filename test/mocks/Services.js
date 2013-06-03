if (typeof (mocks) === 'undefined') mocks = {};
var clients = [
    { "Id": 2539, "IsActive": true, "TypeId": 1, "Name": " JON BOWIE DMD PC - FAIRHOPE", "Address": "KAREN/JON BOWIE DMD", "Address2": "4728 AIRPORT BLVD", "City": "MOBILE", "State": "AL", "Zip": "36608", "Phone": "251-990-3646", "Extension": "", "DateAdded": "2005-10-17T00:00:00", "DateDropped": null, "Fax": "251-990-2558", "ReferBy": "", "Certificate": true, "LifeMember": 0, "Email": "JNBOWIE@GMAIL.COM", "Graduate": true, "AlumniAnnual": "", "AlumniID": "1/3/2012  ", "DDSFirstName": "", "DDSLastName": "" },
    { "Id": 1299, "IsActive": true, "TypeId": 1, "Name": " T. BELL DENTAL AFFILIATES, PLLC", "Address": "SHEILA", "Address2": "P.O. BOX 428", "City": "SOUTH SHORE", "State": "KY", "Zip": "41175", "Phone": "606-932-3181", "Extension": "", "DateAdded": "1997-04-08T00:00:00", "DateDropped": null, "Fax": "606-932-6691", "ReferBy": "", "Certificate": true, "LifeMember": 0, "Email": "", "Graduate": true, "AlumniAnnual": "", "AlumniID": "08/04/11  ", "DDSFirstName": "THOMAS", "DDSLastName": "BELL" },
    { "Id": 1298, "IsActive": true, "TypeId": 1, "Name": " T. BELL DENTAL AFFILIATES, PLLC", "Address": "SHEILA", "Address2": "P.O. BOX 428", "City": "SOUTH SHORE", "State": "KY", "Zip": "41175", "Phone": "606-932-3181", "Extension": "", "DateAdded": "1997-04-08T00:00:00", "DateDropped": null, "Fax": "606-932-6691", "ReferBy": "", "Certificate": true, "LifeMember": 0, "Email": "", "Graduate": true, "AlumniAnnual": "", "AlumniID": "08/04/11  ", "DDSFirstName": "THOMAS", "DDSLastName": "BELL" },
];
var equipment = [
    { "Id": 3592, "ClientId": 2539, "Type": 1, "IsActive": true },
    { "Id": 1072, "ClientId": 1299, "Type": 1, "IsActive": true },
];
var results = [
    { "Id": 649598, "ClientId": 1299, "TestDate": "2012-02-27T00:00:00", "EnterDate": "2012-02-28T00:00:00", "TestResult": true, "EquipId": "A1", "Reference": "" },
    { "Id": 647623, "ClientId": 1299, "TestDate": "2012-02-20T00:00:00", "EnterDate": "2012-02-22T00:00:00", "TestResult": true, "EquipId": "A1", "Reference": "" },
    { "Id": 645546, "ClientId": 1299, "TestDate": "2012-02-13T00:00:00", "EnterDate": "2012-02-15T00:00:00", "TestResult": true, "EquipId": "A1", "Reference": "" },
    { "Id": 642066, "ClientId": 1299, "TestDate": "2012-02-06T00:00:00", "EnterDate": "2012-02-07T00:00:00", "TestResult": true, "EquipId": "A1", "Reference": "" },
    { "Id": 640748, "ClientId": 1299, "TestDate": "2012-01-30T00:00:00", "EnterDate": "2012-02-02T00:00:00", "TestResult": true, "EquipId": "A1", "Reference": "" },
    { "Id": 637964, "ClientId": 1299, "TestDate": "2012-01-23T00:00:00", "EnterDate": "2012-01-25T00:00:00", "TestResult": true, "EquipId": "A1", "Reference": "" },
];
var orders = [
    { "Id": 11867, "ClientId": 2539, "DateReceived": "2008-02-08T00:00:00", "AmountDue": 225.0, "PaymentType": "VISA", "PlacedBy": "KAREN", "CheckNumber": "6045", "TakenBy": "BRIAN", "Lot": "552511", "OrderType": "50 STRIPS", "Units": 1 },
    { "Id": 13603, "ClientId": 2539, "DateReceived": "2009-01-21T00:00:00", "AmountDue": 225.0, "PaymentType": "VISA", "PlacedBy": "KAREN", "CheckNumber": "5421", "TakenBy": "KAREN", "Lot": "567191", "OrderType": "50 STRIPS", "Units": 1 },
    { "Id": 15442, "ClientId": 2539, "DateReceived": "2010-01-13T00:00:00", "AmountDue": 225.0, "PaymentType": "VISA", "PlacedBy": "KAREN", "CheckNumber": "5421", "TakenBy": "GEORGE/kd", "Lot": "580202", "OrderType": "50 STRIPS", "Units": 1 },
    { "Id": 17324, "ClientId": 2539, "DateReceived": "2011-01-05T00:00:00", "AmountDue": 225.0, "PaymentType": "VISA", "PlacedBy": "KAREN", "CheckNumber": "5421", "TakenBy": "GEORGE/lc", "Lot": "583282", "OrderType": "50 STRIPS", "Units": 1 },
    { "Id": 19262, "ClientId": 2539, "DateReceived": "2012-01-03T00:00:00", "AmountDue": 225.0, "PaymentType": "VISA", "PlacedBy": "KAREN", "CheckNumber": "5421", "TakenBy": "LAURA", "Lot": "506791", "OrderType": "50 STRIPS", "Units": 1 },
];

var Service = function (itemData,expectedParams) {
    var items;

    this.init = function () {
        items = JSON.parse(JSON.stringify(itemData));
    }
    this.init();

    this.query = function (params) {
        for (i = 0; expectedParams && i < expectedParams.length; i++) {
            expect(params[expectedParams[i]]).toBeDefined();
        }
        return items;
    }

    this.get = function (params) {
        return items[0];
    }

    this.save = function (params, data) {
        items[0] = data;
    }
    this.remove = function (params) {
        items.shift();
    }
};

mocks.Client = new Service(clients);
mocks.Equipment = new Service(equipment);
mocks.ClientEquipment = new Service(equipment,["clientId"]);
mocks.Result = new Service(results);
mocks.ClientResult = new Service(results, ["clientId"]);
mocks.Order = new Service(orders);
mocks.ClientOrder = new Service(orders, ["clientId"]);
