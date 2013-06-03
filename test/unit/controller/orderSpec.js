//
// test/unit/controllers/orderSpec.js
//
describe("Unit: Order List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Order", mocks.Order);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(OrderListCtrl, { $scope: listScope });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 5 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(5);
    });
});
describe("Unit: Client Order List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Order", mocks.Order);
        $provide.value("ClientOrder", mocks.Order);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(ClientOrderListCtrl, { $scope: listScope, $routeParams: { clientId: 2539 } });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 5 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(5);
    });
});
describe("Unit: Order Detail Control", function () {
    var scope, ctrl;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Order", mocks.Order);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        scope = $rootScope.$new();
        ctrl = $controller(OrderDetailCtrl, { $scope: scope });
    }));

    it('should load 1 item at start', function () {
        expect(scope.order).toEqual(jasmine.any(Object));
    });
    it('should load a valid order object', function () {
        expect(scope.order.Id).toEqual(jasmine.any(Number));
        expect(scope.order.ClientId).toBeDefined();
        expect(scope.order.DateReceived).toBeDefined();
        expect(scope.order.AmountDue).toBeDefined();
        expect(scope.order.PaymentType).toBeDefined();
        expect(scope.order.PlacedBy).toBeDefined();
        expect(scope.order.CheckNumber).toBeDefined();
        expect(scope.order.TakenBy).toBeDefined();
        expect(scope.order.Lot).toBeDefined();
        expect(scope.order.OrderType).toBeDefined();
        expect(scope.order.Units).toBeDefined();
    });
    it('should save an item', function () {
        var newReference = "My New Reference";
        scope.order.TestResult = false;
        scope.order.Reference = newReference;
        scope.save();
        scope.get();
        expect(scope.order.TestResult).toEqual(false);
        expect(scope.order.Reference).toEqual(newReference);
    });
    it('should delete an item correctly', function () {
        var oldId = scope.order.Id;
        scope.delete();
        scope.get();
        expect(scope.order.Id).not.toEqual(oldId);
    });
});
