//
// test/unit/controllers/equipmentSpec.js
//
describe("Unit: Equipment List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Equipment", mocks.Equipment);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(EquipListCtrl, { $scope: listScope });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 2 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(2);
    });
});
describe("Unit: Client Equipment List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Equipment", mocks.Equipment);
        $provide.value("ClientEquipment", mocks.Equipment);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(ClientEquipListCtrl, { $scope: listScope, $routeParams: { clientId: 2539 } });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 2 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(2);
    });
});

describe("Unit: Equipment Detail Control", function () {
    var scope, ctrl;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Equipment", mocks.Equipment);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        scope = $rootScope.$new();
        ctrl = $controller(EquipDetailCtrl, { $scope: scope, $routeParams: {equipmentId:3592} });
    }));

    it('should load 1 item at start', function () {
        expect(scope.equipment).toEqual(jasmine.any(Object));
    });
    it('should load a valid equipment object', function () {
        expect(scope.equipment.Id).toBeDefined();
        expect(scope.equipment.ClientId).toBeDefined();
        expect(scope.equipment.Type).toBeDefined();
        expect(scope.equipment.IsActive).toBeDefined();
    });
    it('should save an item', function () {
        scope.equipment.IsActive = false;
        scope.equipment.Type = 2;
        scope.save();
        scope.get();
        expect(scope.equipment.IsActive).toEqual(false);
        expect(scope.equipment.Type).toEqual(2);
    });
    it('should delete an item correctly', function () {
        var oldId = scope.equipment.Id;
        scope.delete();
        scope.get();
        expect(scope.equipment.Id).not.toEqual(oldId);
    });
});
