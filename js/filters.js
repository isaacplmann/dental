angular.module('osudentalFilters', []).filter('checkmarkicon', function () {
    return function (input) {
        //return input ? '\u2713' : '\u2718';
        return input ? 'icon-ok' : 'icon-remove';
    };
}).filter('checkmarkcolor', function () {
    return function (input) {
        return input ? '\u2713' : '\u2718';
    };
}).filter('dateplus7days', function () {
    return function (input) {
        console.log(input);
        var date1 = new Date(input);
        date1.setDate(date1.getDate() + 7);
        console.log(date1.getDate());
        return date1;
    };
}).filter('equipmentType', function () {
    return function (input) {
        var equipmenttypelabels = {
            1: "Autoclave",
            2: "Chemiclave",
            3: "Other",
        };
        return equipmenttypelabels[input];
    };
}).filter('clientType', function () {
    var clienttypelabels = {
        1: "Dental",
        2: "Tattoo",
        3: "Health Dept",
        4: "Corrections",
        5: "Podiatrist",
        6: "Dermatologist",
        7: "Educational",
        8: "In-House",
        9: "Medical",
    };
    return function (input) {
        //return input ? '\u2713' : '\u2718';
        return input ? 'icon-ok' : 'icon-remove';
    };
});