function ResultListCtrl($scope, Result) {
    $scope.sortInfo = { fields: ['TestDate'], directions: ['DESC'] }
    $scope.pagingOptions = {
        pageSizes: [50, 100, 200],
        pageSize: 50,
        totalServerItems: 1600,
        currentPage: 1
    };
    $scope.filterOptions = {
        startDate: null,
        endDate: null
    };

    $scope.results = [];//Result.query({ page: $scope.pagingOptions.currentPage, pageSize: $scope.pagingOptions.pageSize, sortColumn: $scope.sortInfo.fields[0] });
    $scope.selectedItems = [];
    $scope.totalServerItems = 0;

    $scope.setPagingData = function (data, page, pageSize) {
        $scope.pagingOptions.totalServerItems = data.length;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.getPagedDataAsync = function () {
        setTimeout(function () {
            $scope.results = Result.query({
                page: $scope.pagingOptions.currentPage,
                pageSize: $scope.pagingOptions.pageSize,
                sortColumn: $scope.sortInfo.fields[0],
                direction: $scope.sortInfo.directions[0],
                startDate: $scope.filterOptions.startDate,
                endDate: $scope.filterOptions.endDate,
            }, function (largeLoad) {
                //$scope.setPagingData(largeLoad, page, pageSize);
            });
        }, 100);
    };
    $scope.getPagedDataAsync();

    $scope.$watch('pagingOptions.currentPage', function () {
        $scope.getPagedDataAsync();
    }, true);
    $scope.resetPaging = function () {
        if ($scope.pagingOptions.currentPage == 1) {
            $scope.getPagedDataAsync();
        } else {
            $scope.pagingOptions.currentPage = 1;
        }
    }
    $scope.$watch('pagingOptions.pageSize', function () {
        $scope.resetPaging();
    }, true);
    $scope.$watch('sortInfo.fields', function () {
        $scope.resetPaging();
    }, true);
    $scope.$watch('sortInfo.directions', function () {
        $scope.resetPaging();
    }, true);
    $scope.$watch('filterOptions', function () {
        $scope.resetPaging();
    }, true);

    $scope.$watch('selectedItems', function () {
        if ($scope.selectedItems.length > 0) {
            $location.path("/results/" + $scope.selectedItems[0].Id);
        }
    }, true);

    $scope.gridOptions = {
        data: 'results',
        columnDefs: [{ field: 'TestDate', displayName: 'Test Date', cellFilter:'date' },
            { field: 'EnterDate', displayName: 'Enter Date', cellFilter: 'date' },
            { field: 'TestResult', displayName: 'Test Result', cellFilter: 'checkmark' },
            { field: 'EquipId', displayName: 'Equip ID' },
            { field: 'Reference', displayName: 'Reference' },
        ],
        enablePaging: true,
        showFooter: true,
        pagingOptions: $scope.pagingOptions,
        //filterOptions: $scope.filterOptions,
        sortInfo: $scope.sortInfo,
        useExternalSorting: true,
        selectedItems: $scope.selectedItems,
//        totalServerItems:  //0 //Result.count()
    }
    Result.count(function (data) { console.log(data["0"]); $scope.gridOptions.totalServerItems = data["0"]; });
    console.log(Result.count());

    //.success(function (data) {
    //    setTimeout(function () {
    //        $scope.gridOptions.totalServerItems = data;
    //        $scope.$apply();
    //    }, 100);
    //});
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

function ClientListCtrl($scope, $location, Client) {
    $scope.filterOptions = {
        filterText: "",
        //useExternalFilter: true
    };
    $scope.sortInfo = { fields: ['Name'], directions: ['ASC'] }
    $scope.pagingOptions = {
        pageSizes: [50, 100, 200],
        pageSize: 50,
        totalServerItems: 1600,
        currentPage: 1
    };

    $scope.clients = [];//Client.query({ page: $scope.pagingOptions.currentPage, pageSize: $scope.pagingOptions.pageSize, sortColumn: $scope.sortInfo.fields[0] });
    $scope.selectedItems = [];

    $scope.setPagingData = function (data, page, pageSize) {
        $scope.pagingOptions.totalServerItems = data.length;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.getPagedDataAsync = function (pageSize, page) {
        setTimeout(function () {
            $scope.clients = Client.query({page:page,pageSize:pageSize,sortColumn:$scope.sortInfo.fields[0],direction:$scope.sortInfo.directions[0]},function (largeLoad) {
                //$scope.setPagingData(largeLoad, page, pageSize);
            });
        }, 100);
    };
    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.sortInfo.fields[0], $scope.sortInfo.directions[0]);

    $scope.$watch('pagingOptions.currentPage', function () {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.sortInfo.fields[0], $scope.sortInfo.directions[0]);
    }, true);
    $scope.resetPaging = function () {
        if ($scope.pagingOptions.currentPage == 1) {
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.sortInfo.fields[0], $scope.sortInfo.directions[0]);
        } else {
            $scope.pagingOptions.currentPage = 1;
        }
    }
    $scope.$watch('pagingOptions.pageSize', function () {
        $scope.resetPaging();
    }, true);
    $scope.$watch('sortInfo.fields', function () {
        $scope.resetPaging();
    }, true);
    $scope.$watch('sortInfo.directions', function () {
        $scope.resetPaging();
    }, true);

    $scope.$watch('selectedItems', function () {
        if ($scope.selectedItems.length > 0) {
            $location.path("/clients/"+$scope.selectedItems[0].Id);
        }
    }, true);

    $scope.gridOptions = {
        data: 'clients',
        columnDefs: [{ field: 'Name', displayName: 'Name' }, { field: 'Address2', displayName: 'Address' }],
        enablePaging: true,
        showFooter: true,
        pagingOptions: $scope.pagingOptions,
        //filterOptions: $scope.filterOptions,
        sortInfo: $scope.sortInfo,
        useExternalSorting: true,
        selectedItems:$scope.selectedItems
    }
}

function ClientDetailCtrl($scope, $routeParams, $location, Client) {
    $scope.client = Client.get({ clientId: $routeParams.clientId });
    $scope.save = function () {
        var ok = Client.save({ clientId: $scope.client.Id }, $scope.client, function (res) {
            if (res) { $location.path("/clients"); }
        });
    };
    $scope.delete = function () {
        console.log($scope.client);
        var ok = Client.remove({ clientId: $scope.client.Id }, function (res) {
            if (res) { $location.path("/clients"); }
        });
    }
}


