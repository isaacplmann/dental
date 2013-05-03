angular.module('osudental', ['osudentalFilters','osudentalServices','ngGrid']).
  config(['$routeProvider', function ($routeProvider) {
      $routeProvider.
          when('/clients', { templateUrl: 'tpl/client-list.html', controller: ClientListCtrl }).
          when('/clients/:clientId', { templateUrl: 'tpl/client-detail.html', controller: ClientDetailCtrl }).
          when('/results', { templateUrl: 'tpl/result-list.html', controller: ResultListCtrl }).
          when('/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl }).
          when('/clients/:clientId/results', { templateUrl: 'tpl/result-list.html', controller: ResultListCtrl }).
          when('/clients/:clientId/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl }).
          otherwise({ redirectTo: '/results' });
  }]);
