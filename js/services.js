angular.module('osudentalServices', ['ngResource']).
    factory('Result', function ($resource) {
        return $resource('api/result/:resultId', {}, {
            query: { method: 'GET', isArray: true }
        });
    });