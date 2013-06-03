var TESTING = false,TESTINGUSER = false;

angular.module('osudental', ['osudentalFilters', 'osudentalServices', 'ngGrid', 'ngCookies', 'ui.directives'])
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
                                                    .attr("ng-options", "type as " + tAttrs.selectLabels + "[type] for type in " + tAttrs.selectValues)
                                                    .attr("disabled", tAttrs.hasOwnProperty('disabled')?"":null)
                                                    .attr("required", tAttrs.hasOwnProperty('required') ? "" : null);
                    newPreviewTemplate = $(newPreviewTemplate).html("{{" + tAttrs.selectLabels + "[" + tAttrs.ngModel + "]}}");
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
    .config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

        var access = routingConfig.accessLevels;

        $routeProvider.
            when('/', { templateUrl: 'tpl/client-view.html', controller: ClientDetailCtrl, access: access.user }).
            when('/clients', { templateUrl: 'tpl/client-list.html', controller: PagingGridCtrl, access: access.admin }).
            when('/clients/:clientId', { templateUrl: 'tpl/client-detail.html', controller: ClientDetailCtrl, access: access.admin }).
            when('/results', { templateUrl: 'tpl/result-list.html', controller: PagingGridCtrl, access: access.user }).
            when('/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl, access: access.admin }).
            when('/equipment', { templateUrl: 'tpl/equipment-list.html', controller: PagingGridCtrl, access: access.user }).
            when('/equipment/:equipmentId', { templateUrl: 'tpl/equipment-detail.html', controller: EquipDetailCtrl, access: access.admin }).
            when('/orders', { templateUrl: 'tpl/order-list.html', controller: PagingGridCtrl, access: access.user }).
            when('/orders/:orderId', { templateUrl: 'tpl/order-detail.html', controller: OrderDetailCtrl, access: access.admin }).
            when('/clients/:clientId/results', { templateUrl: 'tpl/client-result-list.html', controller: PagingGridCtrl, access: access.admin }).
            when('/clients/:clientId/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl, access: access.admin }).
            when('/clients/:clientId/equipment', { templateUrl: 'tpl/client-equipment-list.html', controller: PagingGridCtrl, access: access.admin }).
            when('/clients/:clientId/equipment/:equipmentId', { templateUrl: 'tpl/equipment-detail.html', controller: EquipDetailCtrl, access: access.admin }).
            when('/clients/:clientId/orders', { templateUrl: 'tpl/client-order-list.html', controller: PagingGridCtrl, access: access.admin }).
            when('/clients/:clientId/orders/:orderId', { templateUrl: 'tpl/order-detail.html', controller: OrderDetailCtrl, access: access.admin });
//            otherwise({ redirectTo: '/' });

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
    .run(function ($rootScope, $location, $cookieStore) {
        if (TESTING) {
            $rootScope.userRole = 4;
        } else if (TESTINGUSER) {
            $rootScope.userRole = 2;
        } else {
            $rootScope.userRole = $cookieStore.get('userRole') ||
                                  routingConfig.userRoles.public;
            //$cookieStore.remove('userRole');
        }

        //console.log($rootScope.userRole);

        $rootScope.$on("$routeChangeStart", function (event, next, current) {
            if (!next || !(next.access & $rootScope.userRole)) {
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
    });

