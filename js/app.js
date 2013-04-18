angular.module('osudental', ['osudentalFilters','osudentalServices']).
  config(['$routeProvider', function ($routeProvider) {
      $routeProvider.
          when('/results', { templateUrl: 'tpl/result-list.html', controller: ResultListCtrl }).
          when('/results/:resultId', { templateUrl: 'tpl/result-detail.html', controller: ResultDetailCtrl }).
          otherwise({ redirectTo: '/results' });
  }]);