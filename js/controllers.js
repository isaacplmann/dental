function ResultListCtrl($scope, Result) {
    $scope.results = Result.query();
    $scope.orderProp = 'Id';
    $scope.sort = function (prop) {
        if ($scope.orderProp.indexOf(prop) == 0) {
            $scope.orderProp = "-" + prop;
        } else {
            $scope.orderProp = prop;
        }
    }
}

function ResultDetailCtrl($scope, $routeParams, $location, Result) {
    $scope.result = Result.get({ resultId: $routeParams.resultId });
    $scope.save = function () {
        var ok = Result.save({ resultId: $scope.result.Id }, $scope.result, function (res) {
            if (res) { $location.path("/results"); }
        });
    };
    $scope.delete = function () {
        console.log($scope.result);
        var ok = Result.remove({ resultId: $scope.result.Id }, function (res) {
            if (res) { $location.path("/results"); }
        });
    }
}


