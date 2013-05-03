angular.module('osudentalServices', ['ngResource']).
    factory('Client', function ($resource) {
        return $resource('api/client/:clientId', {}, {
            query: { method: 'GET', isArray: true }
        });
    }).
    factory('Result', function ($resource) {
        return $resource('api/result/:listController:resultId',
            {
                resultId: "@resultId",
                listController: "@listController"
            },
            {
                query: { method: 'GET', isArray: true },
                count: {
                    method: "GET",
                    params: {
                        listController: "count"
                    }
                }
        });
    });

