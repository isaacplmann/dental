function PagingGridCtrl($scope, $location, $timeout) {
    $scope.PagingGrid = {};
    $scope.PagingGrid.Service = function(){};
    $scope.PagingGrid.getPagedDataAsync = function () {
        $timeout(function () {
            var queryOptions = {
                page: $scope.PagingGrid.pagingOptions.currentPage,
                pageSize: $scope.PagingGrid.pagingOptions.pageSize,
                sortColumn: $scope.PagingGrid.sortInfo.fields[0],
                direction: $scope.PagingGrid.sortInfo.directions[0],
            };
            for (var i = 0; i < $scope.PagingGrid.filterOptions.filters.length; i++) {
                queryOptions[$scope.PagingGrid.filterOptions.filters[i]] = $scope.PagingGrid.filterOptions[$scope.PagingGrid.filterOptions.filters[i]];
            }
            $scope.PagingGrid.items = $scope.PagingGrid.Service.query(queryOptions, function (largeLoad) {
                //$scope.setPagingData(largeLoad, page, pageSize);
            });
        }, 10);
    };
    $scope.PagingGrid.sortInfo = { fields: ['ID'], directions: ['ASC'] }
    $scope.PagingGrid.filterOptions = {
        filters: []
    };
    $scope.PagingGrid.pagingOptions = {
        pageSizes: [50, 100, 200],
        pageSize: 50,
        totalServerItems: 1600,
        currentPage: 1
    };
    $scope.PagingGrid.setPagingData = function (data, page, pageSize) {
        $scope.PagingGrid.pagingOptions.totalServerItems = data.length;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.PagingGrid.resetPaging = function () {
        if ($scope.PagingGrid.pagingOptions.currentPage == 1) {
            $scope.PagingGrid.getPagedDataAsync();
        } else {
            $scope.PagingGrid.pagingOptions.currentPage = 1;
        }
    };
    $scope.PagingGrid.items = [];
    $scope.PagingGrid.selectedItems = [];
    $scope.PagingGrid.totalServerItems = 0;
    $scope.PagingGrid.getPagedDataAsync();
    $scope.$watch('PagingGrid.pagingOptions.currentPage', function () {
        $scope.PagingGrid.getPagedDataAsync();
    }, true);
    $scope.$watch('PagingGrid.pagingOptions.pageSize', function () {
        $scope.PagingGrid.resetPaging();
    }, true);
    $scope.$watch('PagingGrid.sortInfo.fields', function () {
        $scope.PagingGrid.resetPaging();
    }, true);
    $scope.$watch('PagingGrid.sortInfo.directions', function () {
        $scope.PagingGrid.resetPaging();
    }, true);
    $scope.$watch('PagingGrid.filterOptions', function () {
        $scope.PagingGrid.resetPaging();
    }, true);
    $scope.$watch('PagingGrid.selectedItems', function () {
        if ($scope.PagingGrid.selectedItems.length > 0) {
            $location.path($location.path() + "/" + $scope.PagingGrid.selectedItems[0].Id);
        }
    }, true);

    $scope.PagingGrid.gridOptions = {
        data: 'PagingGrid.items',
        columnDefs: [{ field: 'Id', displayName: 'ID' }],
        enablePaging: true,
        showFooter: true,
        pagingOptions: $scope.PagingGrid.pagingOptions,
        //filterOptions: $scope.filterOptions,
        selectedItems: $scope.PagingGrid.selectedItems,
        sortInfo: $scope.sortInfo,
        useExternalSorting: true,
        //        totalServerItems:  //0 //Result.count()
    }
};
function DetailCtrl($scope, $routeParams, $location, Service, paramName, itemName) {
    $scope.get = function () {
        params = {};
        params[paramName] = $routeParams[paramName];
        $scope[itemName] = Service.get(params);
    };
    $scope.get();
    $scope.create = function () {
        var ok = Service.save({}, $scope[itemName], function (res) {
            if (res) {
                var arr = $location.path().split("/");
                arr.pop();
                $location.path(arr.join("/"));
            }
        });
    };
    $scope.save = function () {
        params = {};
        params[paramName] = $scope[itemName].Id;
        var ok = Service.save(params, $scope[itemName], function (res) {
            if (res) {
                var arr = $location.path().split("/");
                arr.pop();
                $location.path(arr.join("/"));
            }
        });
    };
    $scope.delete = function () {
        params = {};
        params[paramName] = $scope[itemName].Id;
        var ok = Service.remove(params, function (res) {
            if (res) {
                var arr = $location.path().split("/");
                arr.pop();
                $location.path(arr.join("/"));
            }
        });
    }
}

function NavBarCtrl($scope, $route) {
    console.log("userRole",$scope.userRole);
    $scope.$route = $route;
    $scope.links = [{
        uri: '#/a',
        name: 'A',
        type: 'a'
    }, {
        uri: '#/b',
        name: 'B',
        type: 'b'
    }, {
        uri: '#/c',
        name: 'C',
        type: 'c'
    }, {
        uri: '#/a/detail',
        name: 'A Detail',
        type: 'a'
    }];
}

function ResultListCtrl($scope, Result) {
    $scope.PagingGrid.Service = Result;
    $scope.PagingGrid.sortInfo.fields = ['TestDate'];
    $scope.PagingGrid.sortInfo.directions = ['DESC'];
    $scope.PagingGrid.filterOptions = {
        filters: ['startDate', 'endDate'],
        startDate: null,
        endDate: null
    };

    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'TestDate', displayName: 'Test Date', cellFilter: 'date' },
        { field: 'EnterDate', displayName: 'Enter Date', cellFilter: 'date' },
        { field: 'TestResult', displayName: 'Test Result', cellFilter: 'checkmark' },
        { field: 'EquipId', displayName: 'Equip ID' },
        { field: 'Reference', displayName: 'Reference' },
    ];

    //Result.count(function (data) {
    //    $scope.gridOptions.totalServerItems = digitArrayToInteger(data);
    //});

    //.success(function (data) {
    //    setTimeout(function () {
    //        $scope.gridOptions.totalServerItems = data;
    //        $scope.$apply();
    //    }, 100);
    //});
}
function ClientResultListCtrl($scope, $routeParams, ClientResult, Result) {
    var clientId = $routeParams.clientId;
    if (!clientId) {
        $scope.PagingGrid.Service = Result;
        $scope.PagingGrid.filterOptions = {
            filters: ['startDate', 'endDate'],
            startDate: null,
            endDate: null,
        };
    } else {
        $scope.PagingGrid.Service = ClientResult;
        $scope.PagingGrid.filterOptions = {
            filters: ['startDate', 'endDate', 'clientId'],
            startDate: null,
            endDate: null,
            clientId: $routeParams.clientId,
        };
    }
    $scope.PagingGrid.sortInfo.fields = ['TestDate'];
    $scope.PagingGrid.sortInfo.directions = ['DESC'];
    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'TestDate', displayName: 'Test Date', cellFilter: 'date' },
        { field: 'EnterDate', displayName: 'Enter Date', cellFilter: 'date' },
        { field: 'TestResult', displayName: 'Test Result', cellFilter: 'checkmark' },
        { field: 'EquipId', displayName: 'Equip ID' },
        { field: 'Reference', displayName: 'Reference' },
    ];
}

function digitArrayToInteger(arr) {
    var len = arr.length;
    var sum = 0;
    for (i = 0; arr[i];i++) {
        sum = 10 * sum + 1*arr[i];
    }
    return sum;
}

function ResultDetailCtrl($scope, $routeParams, $location, Result) {
    DetailCtrl($scope, $routeParams, $location, Result, "resultId", "result");
}

function ClientListCtrl($scope, $location, Client) {
    $scope.PagingGrid.Service = Client;
    $scope.PagingGrid.sortInfo.fields = ['Name'];
    $scope.PagingGrid.sortInfo.directions = ['ASC'];
    $scope.PagingGrid.gridOptions.columnDefs = [
        { field: 'Id', displayName: 'Client Id', width: 70 },
        { field: 'Name', displayName: 'Practice' },
        { field: 'Address2', displayName: 'Address' },
        { field: 'DateAdded', displayName: 'Date Added', cellFilter: 'date' }
    ];
}

function ClientDetailCtrl($scope, $routeParams, $location, Client) {
    DetailCtrl($scope, $routeParams, $location, Client, "clientId", "client");
    $scope.isEditMode = false;
    $scope.clienttypes = [1,2,3,4,5,6,7,8,9];
    $scope.clienttypelabels = {
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
}


function EquipListCtrl($scope, Equipment) {
    $scope.PagingGrid.Service = Equipment;
    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'Id', displayName: 'Equip ID' },
            { field: 'Type', displayName: 'Equipment Type' },
            { field: 'IsActive', displayName: 'Is Active', cellFilter: 'checkmark' },
    ];
}
function ClientEquipListCtrl($scope, $routeParams, ClientEquipment, Equipment) {
    var clientId = $routeParams.clientId;
    if (!clientId) {
        $scope.PagingGrid.Service = Equipment;
    } else {
        $scope.PagingGrid.Service = ClientEquipment;
        $scope.PagingGrid.filterOptions = {
            filters: ['clientId'],
            clientId: $routeParams.clientId,
        };
    }
    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'Id', displayName: 'Equip ID' },
            { field: 'Type', displayName: 'Equipment Type' },
            { field: 'IsActive', displayName: 'Is Active', cellFilter: 'checkmark' },
    ];
}

function EquipDetailCtrl($scope, $routeParams, $location, Equipment) {
    DetailCtrl($scope, $routeParams, $location, Equipment, "equipmentId", "equipment");
}

function OrderListCtrl($rootScope, $scope, Order) {
    $scope.PagingGrid.Service = Order;
    $scope.PagingGrid.sortInfo.fields = ['DateReceived'];
    $scope.PagingGrid.sortInfo.directions = ['DESC'];
    if ($rootScope.userRole == 4) {
        $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'Id', displayName: 'Order ID' },
            { field: 'ClientId', displayName: 'Client Id' },
            { field: 'OrderType', displayName: 'Order Type' },
            { field: 'Units', displayName: 'Units' },
            { field: 'AmountDue', displayName: 'Amount' },
            { field: 'PlacedBy', displayName: 'Placed By' },
            { field: 'DateReceived', displayName: 'Date Received', cellFilter: 'date' },
            { field: 'PaymentType', displayName: 'Payment Method' },
            { field: 'CheckNumber', displayName: 'Check Number' },
            { field: 'TakenBy', displayName: 'Taken By' },
            { field: 'Lot' },
        ];
    } else {
        $scope.PagingGrid.gridOptions.columnDefs = [
            { field: 'OrderType', displayName: 'Order Type' },
            { field: 'Units', displayName: 'Units' },
            { field: 'AmountDue', displayName: 'Amount' },
            { field: 'PlacedBy', displayName: 'Placed By' },
            { field: 'DateReceived', displayName: 'Date Received', cellFilter: 'date' },
            { field: 'PaymentType', displayName: 'Payment Method' },
            { field: 'CheckNumber', displayName: 'Check Number' },
        ];
    }
}
function ClientOrderListCtrl($scope, $routeParams, ClientOrder, Order) {
    var clientId = $routeParams.clientId;
    if (!clientId) {
        $scope.PagingGrid.Service = Order;
    } else {
        $scope.PagingGrid.Service = ClientOrder;
        $scope.PagingGrid.filterOptions = {
            filters: ['clientId'],
            clientId: $routeParams.clientId,
        };
    }
    $scope.PagingGrid.sortInfo.fields = ['DateReceived'];
    $scope.PagingGrid.sortInfo.directions = ['DESC'];
    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'Id', displayName: 'Order ID' },
            { field: 'ClientId', displayName: 'Client Id' },
            { field: 'OrderType', displayName: 'Order Type' },
            { field: 'Units', displayName: 'Units' },
            { field: 'AmountDue', displayName: 'Amount' },
            { field: 'PlacedBy', displayName: 'Placed By' },
            { field: 'DateReceived', displayName: 'Date Received', cellFilter: 'date' },
            { field: 'PaymentType', displayName: 'Payment Method' },
            { field: 'CheckNumber', displayName: 'Check Number' },
            { field: 'TakenBy', displayName: 'Taken By' },
            { field: 'Lot' },
    ];
}

function OrderDetailCtrl($scope, $routeParams, $location, Order) {
    DetailCtrl($scope, $routeParams, $location, Order, "orderId", "order");
}

