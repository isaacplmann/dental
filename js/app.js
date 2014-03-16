var TESTING = false,TESTINGUSER = false;

angular.module('osudental', ['osudentalFilters', 'osudentalServices', 'ngGrid', 'ngCookies', 'ui.directives', 'ui.state'])
    .directive('nuwuEditable', function () {
        var inputTemplate = '<input ng-show="isEditMode" class="input-block-level" type="text" />';
        var textareaTemplate = '<textarea ng-show="isEditMode" class="input-block-level" rows="10" cols="10"></textarea>';
        var selectTemplate = '<select ng-show="isEditMode" class="input-block-level" ></select>';
        var previewTemplate = '<div class="nuwu-editable-preview" ng-hide="isEditMode"></div>';
        return{
            restrict: 'E',
            compile: function (tElement, tAttrs, transclude) {
                var editElement = {};
                var newPreviewTemplate = $(previewTemplate).html("{{" + tAttrs.ngModel + "}}");
                if (tAttrs.type == "textarea") {
                    var editTemplate = $(textareaTemplate).attr("ng-model", tAttrs.ngModel)
                                                    .attr("name", tAttrs.ngModel)
                                                    .attr("disabled", tAttrs.hasOwnProperty('disabled')?"":null)
                                                    .attr("required", tAttrs.hasOwnProperty('required') ? "" : null);
                } else if (tAttrs.type == "select") {
                    var editTemplate = $(selectTemplate).attr("ng-model", tAttrs.ngModel)
                                                    .attr("name", tAttrs.ngModel)
                                                    .attr("ng-options", "obj.value as obj.label for obj in " + tAttrs.selectOptions)
                                                    .attr("disabled", tAttrs.hasOwnProperty('disabled')?"":null)
                                                    .attr("required", tAttrs.hasOwnProperty('required') ? "" : null);
                    newPreviewTemplate = $(newPreviewTemplate).html("{{" + tAttrs.selectOptions + "[" + tAttrs.ngModel + "-1].label}}");
                } else {
                    var editTemplate = $(inputTemplate).attr("ng-model", tAttrs.ngModel)
                                                    .attr("name", tAttrs.ngModel)
                                                    .attr("type", tAttrs.type)
                                                    .attr("disabled", tAttrs.hasOwnProperty('disabled')?"":null)
                                                    .attr("required", tAttrs.hasOwnProperty('required') ? "" : null);
                    if (tAttrs.type == "checkbox") {
                        editTemplate = $(editTemplate).removeClass("input-block-level");
                        newPreviewTemplate = "<span class='nuwu-editable-preview' ng-hide='isEditMode'><i class='icon-save {{" + tAttrs.ngModel + "| checkmarkicon}}'></i></span>";
                    }
                }
                if (tAttrs.hasOwnProperty('disabled')) {
                    editTemplate = "";
                    newPreviewTemplate = $(newPreviewTemplate).removeAttr("ng-hide");
                }

                editElement = angular.element(editTemplate);
                tElement.append(editElement);
                var previewElement = angular.element(newPreviewTemplate);
                tElement.append(previewElement);

                return function (scope, element, attrs) {
                }
            }
        }
    })
    .config(['$routeProvider', '$locationProvider', '$httpProvider','$stateProvider', function ($routeProvider, $locationProvider, $httpProvider,$stateProvider) {

        var access = routingConfig.accessLevels;

        //$routeProvider.
        //    when('/', { templateUrl: 'tpl/client-view.html', controller: ClientDetailCtrl, access: access.user }).
        //    when('/clients', { templateUrl: 'tpl/client-list.html', controller: PagingGridCtrl, access: access.admin }).
        //    when('/clients/:clientId', { templateUrl: 'tpl/client-detail.html', controller: ClientDetailCtrl, access: access.admin }).
        //    when('/results', { templateUrl: 'tpl/result-list.html', controller: PagingGridCtrl, access: access.user }).
        //    when('/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl, access: access.admin }).
        //    when('/equipment', { templateUrl: 'tpl/equipment-list.html', controller: PagingGridCtrl, access: access.user }).
        //    when('/equipment/:equipmentId', { templateUrl: 'tpl/equipment-detail.html', controller: EquipDetailCtrl, access: access.admin }).
        //    when('/orders', { templateUrl: 'tpl/order-list.html', controller: PagingGridCtrl, access: access.user }).
        //    when('/orders/:orderId', { templateUrl: 'tpl/order-detail.html', controller: OrderDetailCtrl, access: access.admin }).
        //    when('/clients/:clientId/results', { templateUrl: 'tpl/client-result-list.html', controller: PagingGridCtrl, access: access.admin }).
        //    when('/clients/:clientId/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl, access: access.admin }).
        //    when('/clients/:clientId/equipment', { templateUrl: 'tpl/client-equipment-list.html', controller: PagingGridCtrl, access: access.admin }).
        //    when('/clients/:clientId/equipment/:equipmentId', { templateUrl: 'tpl/equipment-detail.html', controller: EquipDetailCtrl, access: access.admin }).
        //    when('/clients/:clientId/orders', { templateUrl: 'tpl/client-order-list.html', controller: PagingGridCtrl, access: access.admin }).
        //    when('/clients/:clientId/orders/:orderId', { templateUrl: 'tpl/order-detail.html', controller: OrderDetailCtrl, access: access.admin });
        //            otherwise({ redirectTo: '/' });

        /****************
         * view areas
         toolbar@ - put state-specific buttons here
         primary@ - top-level view, corresponds to main navbar
         clientdetail@client.detail - nested view, corresponds to client detail nav
         */

        $stateProvider
            .state("contactinfo", {
                url: '/',
                views: {
                    "primary@": {
                        templateUrl: 'tpl/client-view.html',
                        controller: 'ClientDetailCtrl',
                    }
                },
                data: {
                    access: access.user,
                    isEditable: true
                }
            })
            .state("clients", {
                url: '/clients',
                views: {
                    "toolbar@": {
                        template: '<a href="#/clients/0" class="btn btn-info">Add a Client</a>'
                    },
                    primary: {
                        templateUrl: 'tpl/client-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.admin,
                }
            })
            .state("clients.detail", {
                url: '/:clientId',
                views: {
                    "primary@": {
                        templateUrl: 'tpl/client-navbar.html',
                        controller: 'ClientNavBarCtrl',
                    },
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/client-detail.html',
                        controller: 'ClientDetailCtrl',
                    }
                },
                data: {
                    access: access.admin,
                    isEditable: true
                }
            })
            .state("clients.detail.results", {
                url: '/results',
                views: {
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/client-result-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.admin,
                }
            })
            .state("clients.detail.results.detail", {
                url: '/:resultId',
                views: {
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/result-detail.html',
                        controller: 'ResultDetailCtrl'
                    }
                },
                data: {
                    access: access.admin,
                    isEditable: true,
                    activeClientNav: 'results'
                }
            })
            .state("clients.detail.equipment", {
                url: '/equipment',
                views: {
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/client-equipment-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.admin,
                }
            })
            .state("clients.detail.equipment.detail", {
                url: '/:equipmentId',
                views: {
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/equipment-detail.html',
                        controller: 'EquipDetailCtrl'
                    }
                },
                data: {
                    access: access.admin,
                    isEditable: true,
                    activeClientNav: 'equipment',
                }
            })
            .state("clients.detail.orders", {
                url: '/orders',
                views: {
                    "toolbar@": {
                        template: '<a href="#/orders/0" class="btn btn-info">Place an Order</a>'
                    },
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/client-order-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.admin,
                }
            })
            .state("clients.detail.orders.detail", {
                url: '/:orderId',
                views: {
                    "clientdetail@clients.detail": {
                        templateUrl: 'tpl/order-detail.html',
                        controller: 'OrderDetailCtrl'
                    }
                },
                data: {
                    access: access.admin,
                    activeClientNav: 'orders',
                    isEditable: true
                }
            })
            .state("results", {
                url: '/results',
                views: {
                    "toolbar@": {
                        template: '<a ng-show="userRole==4" href="#/results/0" class="btn btn-info">Add a Result</a>'
                    },
                    primary: {
                        templateUrl: 'tpl/result-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.user,
                }
            })
            .state("results.detail", {
                url: '/:resultId',
                views: {
                    "primary@": {
                        templateUrl: 'tpl/result-detail.html',
                        controller: 'ResultDetailCtrl'
                    }
                },
                data: {
                    access: access.admin,
                    isEditMode: true
                }
            })
            .state("equipment", {
                url: '/equipment',
                views: {
                    "toolbar@": {
                        template: '<a ng-show="userRole==4" href="#/equipment/0" class="btn btn-info">Add Equipment</a>'
                    },
                    primary: {
                        templateUrl: 'tpl/equipment-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.user,
                }
            })
            .state("equipment.detail", {
                url: '/:equipmentId',
                views: {
                    "primary@": {
                        templateUrl: 'tpl/equipment-detail.html',
                        controller: 'EquipDetailCtrl'
                    }
                },
                data: {
                    access: access.admin,
                    isEditable: true
                }
            })
            .state("orders", {
                url: '/orders',
                views: {
                    "toolbar@": {
                        template: '<a href="#/orders/0" class="btn btn-info">Place an Order</a>'
                    },
                    primary: {
                        templateUrl: 'tpl/order-list.html',
                        controller: 'PagingGridCtrl',
                    }
                },
                data: {
                    access: access.user,
                }
            })
            .state("orders.detail", {
                url: '/:orderId',
                views: {
                    "primary@": {
                        templateUrl: 'tpl/order-detail.html',
                        controller: 'OrderDetailCtrl'
                    }
                },
                data: {
                    access: access.user,
                    isEditable: true
                }
            });

        var interceptor = ['$location', '$q', function ($location, $q) {
            function success(response) {
                return response;
            }

            function error(response) {
                if (response.status === 401) {
                    //$rootScope.userRole = routingConfig.userRoles.public;
                    window.location.href = '/Login.aspx';
                    //$location.path('/login');
                    return $q.reject(response);
                }
                else {
                    return $q.reject(response);
                }
            }

            return function (promise) {
                return promise.then(success, error);
            }
        }];

        $httpProvider.responseInterceptors.push(interceptor);
    }])
    .run(function ($rootScope, $location, $cookieStore, $route, $state, $stateParams) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        if (TESTING) {
            $rootScope.userRole = 4;
        } else if (TESTINGUSER) {
            $rootScope.userRole = 2;
        } else {
            $rootScope.userRole = $cookieStore.get('userRole') ||
                                  routingConfig.userRoles.public;
            //$cookieStore.remove('userRole');
        }

        $rootScope.$on("$stateChangeStart", function (event, next, current) {
            if (!next || !(next.data.access & $rootScope.userRole)) {
                if ($rootScope.userRole === routingConfig.userRoles.user ||
                   $rootScope.userRole === routingConfig.userRoles.admin) {
                    $location.path('/');
                }
                else {
                    $rootScope.userRole = routingConfig.userRoles.public;
                    window.location.href = '/Login.aspx';
                    //$location.path('/login');
                }
            }
        });
        $rootScope.isUser = function () {
            return $rootScope.userRole == 2;
        }
        $rootScope.isAdmin = function () {
            return $rootScope.userRole == 4;
        }
    });

