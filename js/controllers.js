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
    if ($scope.isAdmin()) {
        $scope.$watch('PagingGrid.selectedItems', function () {
            if ($scope.PagingGrid.selectedItems.length > 0) {
                $location.path($location.path() + "/" + $scope.PagingGrid.selectedItems[0].Id);
            }
        }, true);
    }

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
function DetailCtrl($scope, $stateParams, $location, Service, paramName, itemName) {
    $scope.isNewMode = ($stateParams[paramName] == 0);
    $scope.get = function () {
        params = {};
        params[paramName] = $stateParams[paramName];
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
    $scope.edit = function () {
        $scope.isEditMode = true;
    };
    $scope.getEditMode = function () {
        return $scope.isEditMode;
    }
    $scope.save = function () {
        params = {};
        params[paramName] = $scope[itemName].Id;
        var ok = Service.save(params, $scope[itemName], function (res) {
            if (res) {
                var path = $location.path();
                if (path == "/") {
                    $scope.isEditMode = false;
                } else {
                    var arr = path.split("/");
                    arr.pop();
                    $location.path(arr.join("/"));
                }
            }
        });
    };
    $scope.cancel = function () {
        if ($scope.canEditView) {
            $scope.isEditMode = false;
        } else {
            var arr = $location.path().split("/");
            arr.pop();
            $location.path(arr.join("/"));
        }
    };
    $scope.delete = function () {
        if (confirm("Are you sure you want to delete this record?")) {
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
}

function NavBarCtrl($scope, $state, $route, $location) {
    $scope.path = $location.path();
    $scope.$route = $route;
    if ($scope.isUser()) {
        $scope.links = [{
            uri: '#/',
            name: 'Contact Info',
            type: 'contactinfo'
        }, {
            uri: '#/results',
            name: 'Results',
            type: 'results'
        }, {
            uri: '#/orders',
            name: 'Order History',
            type: 'orders'
        }];
    } else if ($scope.isAdmin()) {
        $scope.links = [{
            uri: '#/clients',
            name: 'Clients',
            type: 'clients'
        }, {
            uri: '#/equipment',
            name: 'Equipment',
            type: 'equipment'
        }, {
            uri: '#/results',
            name: 'Results',
            type: 'results'
        }, {
            uri: '#/orders',
            name: 'Orders',
            type: 'orders'
        }];
    }
}
function ClientNavBarCtrl($scope, $state, $location, Client) {
    $scope.path = $location.path();
    $scope.$on("$stateChangeStart", function (event, next, current) {
        $scope.path = $location.path();
    });
    var clientId = $scope.path.split("/")[2];
    $scope.clientData = Client.get({ "clientId": clientId });
    $scope.$state = $state;
    $scope.clientlinks = [{
        uri: '#/clients/'+clientId,
        name: 'Contact Info',
        type: 'contactinfo'
    }, {
        uri: '#/clients/'+clientId+'/equipment',
        name: 'Equipment',
        type: 'equipment'
    }, {
        uri: '#/clients/' + clientId + '/results',
        name: 'Results',
        type: 'results'
    }, {
        uri: '#/clients/' + clientId + '/orders',
        name: 'Orders',
        type: 'orders'
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
function ClientResultListCtrl($scope, $stateParams, ClientResult, Result) {
    var clientId = $stateParams.clientId;
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
            clientId: $stateParams.clientId,
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

function ResultDetailCtrl($scope, $stateParams, $location, Result) {
    DetailCtrl($scope, $stateParams, $location, Result, "resultId", "result");
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

function ClientDetailCtrl($scope, $stateParams, $location, Client) {
    if ($location.path()=="/" && $scope.isAdmin()) {
        $location.path("/clients");
    }

    DetailCtrl($scope, $stateParams, $location, Client, "clientId", "client");
    if ($scope.isUser()) {
        $scope.canEditView = true;
        $scope.isEditMode = false;
    }
    $scope.clienttypes = [
        {value:1,label: "Dental"},
        {value:2,label: "Tattoo"},
        {value:3,label: "Health Dept"},
        {value:4,label: "Corrections"},
        {value:5,label: "Podiatrist"},
        {value:6,label: "Dermatologist"},
        {value:7,label: "Educational"},
        {value:8,label: "In-House"},
        { value: 9, label: "Medical" }
    ];
}


function EquipListCtrl($scope, Equipment) {
    $scope.PagingGrid.Service = Equipment;
    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'Id', displayName: 'Equip ID' },
            { field: 'Type', displayName: 'Equipment Type', cellFilter: 'equipmentType' },
            { field: 'IsActive', displayName: 'Is Active', cellFilter: 'checkmark' },
    ];
}
function ClientEquipListCtrl($scope, $stateParams, ClientEquipment, Equipment) {
    var clientId = $stateParams.clientId;
    if (!clientId) {
        $scope.PagingGrid.Service = Equipment;
    } else {
        $scope.PagingGrid.Service = ClientEquipment;
        $scope.PagingGrid.filterOptions = {
            filters: ['clientId'],
            clientId: $stateParams.clientId,
        };
    }
    $scope.PagingGrid.gridOptions.columnDefs = [{ field: 'Id', displayName: 'Equip ID' },
            { field: 'Type', displayName: 'Equipment Type', cellFilter: 'equipmentType' },
            { field: 'IsActive', displayName: 'Is Active', cellFilter: 'checkmark' },
    ];
}

function EquipDetailCtrl($scope, $stateParams, $location, Equipment) {
    DetailCtrl($scope, $stateParams, $location, Equipment, "equipmentId", "equipment");
    $scope.equipmenttypes = [
        { value: 1, label: "Autoclave" },
        { value: 2, label: "Chemiclave" },
        { value: 3, label: "Other" },
    ];
}

function OrderListCtrl($scope, Order) {
    $scope.PagingGrid.Service = Order;
    $scope.PagingGrid.sortInfo.fields = ['DateReceived'];
    $scope.PagingGrid.sortInfo.directions = ['DESC'];
    if ($scope.isAdmin()) {
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
function ClientOrderListCtrl($scope, $stateParams, ClientOrder, Order) {
    var clientId = $stateParams.clientId;
    if (!clientId) {
        $scope.PagingGrid.Service = Order;
    } else {
        $scope.PagingGrid.Service = ClientOrder;
        $scope.PagingGrid.filterOptions = {
            filters: ['clientId'],
            clientId: $stateParams.clientId,
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

function OrderDetailCtrl($scope, $stateParams, $location, Order) {
    DetailCtrl($scope, $stateParams, $location, Order, "orderId", "order");
}

