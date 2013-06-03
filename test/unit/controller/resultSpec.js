//
// test/unit/controllers/resultSpec.js
//
describe("Unit: Result List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Result", mocks.Result);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(ResultListCtrl, { $scope: listScope });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 6 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(6);
    });
});
describe("Unit: Client Result List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Result", mocks.Result);
        $provide.value("ClientResult", mocks.ClientResult);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(ClientResultListCtrl, { $scope: listScope, $routeParams: {clientId:2539} });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 6 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(6);
    });
});
describe("Unit: Result Detail Control", function () {
    var scope, ctrl;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Result", mocks.Result);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        scope = $rootScope.$new();
        ctrl = $controller(ResultDetailCtrl, { $scope: scope });
    }));

    it('should load 1 item at start', function () {
        expect(scope.result).toEqual(jasmine.any(Object));
    });
    it('should load a valid result object', function () {
        expect(scope.result.Id).toEqual(jasmine.any(Number));
        expect(scope.result.ClientId).toBeDefined();
        expect(scope.result.TestDate).toBeDefined();
        expect(scope.result.EnterDate).toBeDefined();
        expect(scope.result.TestResult).toBeDefined();
        expect(scope.result.EquipId).toBeDefined();
        expect(scope.result.Reference).toBeDefined();
    });
    it('should save an item', function () {
        var newReference = "My New Reference";
        scope.result.TestResult = false;
        scope.result.Reference = newReference;
        scope.save();
        scope.get();
        expect(scope.result.TestResult).toEqual(false);
        expect(scope.result.Reference).toEqual(newReference);
    });
    it('should delete an item correctly', function () {
        var oldId = scope.result.Id;
        scope.delete();
        scope.get();
        expect(scope.result.Id).not.toEqual(oldId);
    });
});
