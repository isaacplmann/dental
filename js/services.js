angular.module('osudentalServices', ['ngResource']).
    factory('Client', function ($resource) {
        return $resource('api/client/:clientId', { clientId: "0" }, {
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
    }).
    factory('ClientResult', function ($resource) {
        return $resource('api/client/:clientId/result/:listController:resultId',
            {
                clientId: "@clientId",
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
    }).
    factory('Equipment', function ($resource) {
        return $resource('api/equipment/:listController:equipmentId',
            {
                equipmentId: "@equipmentId",
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
    }).
    factory('ClientEquipment', function ($resource) {
        return $resource('api/client/:clientId/equipment/:listController:equipmentId',
            {
                clientId: "@clientId",
                equipmentId: "@equipmentId",
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
    }).
    factory('Order', function ($resource) {
        return $resource('api/order/:listController:orderId',
            {
                equipmentId: "@orderId",
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
    }).
    factory('ClientOrder', function ($resource) {
        return $resource('api/client/:clientId/order/:listController:orderId',
            {
                clientId: "@clientId",
                equipmentId: "@orderId",
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

