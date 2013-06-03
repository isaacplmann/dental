angular.module('osudentalFilters', []).filter('checkmarkicon', function () {
    return function (input) {
        //return input ? '\u2713' : '\u2718';
        return input ? 'icon-ok' : 'icon-remove';
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