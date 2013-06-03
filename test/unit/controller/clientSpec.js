//
// test/unit/controllers/clientSpec.js
//
describe("Unit: Client List Control", function () {
    var listScope, listCtrl, $timeout;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Client", mocks.Client);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        listScope = $rootScope.$new();
        pagingCtrl = $controller(PagingGridCtrl, { $scope: listScope });
        listCtrl = $controller(ClientListCtrl, { $scope: listScope });
    }));

    it('should set up a PagingGrid', function () {
        expect(listScope.PagingGrid).toBeDefined();
    });

    it('should load 3 items at start', function () {
        $timeout.flush();
        expect(listScope.PagingGrid.items.length).toEqual(3);
    });
});
describe("Unit: Client Detail Control", function () {
    var scope, ctrl;

    beforeEach(angular.mock.module('osudental', function ($provide) {
        $provide.value("Client", mocks.Client);
    }));

    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        $timeout = _$timeout_;
        scope = $rootScope.$new();
        ctrl = $controller(ClientDetailCtrl, { $scope: scope });
    }));

    it('should load 1 item at start', function () {
        expect(scope.client).toEqual(jasmine.any(Object));
    });
    it('should load a valid client object', function () {
        expect(scope.client.Id).toEqual(jasmine.any(Number));
        expect(scope.client.IsActive).toBeDefined();
        expect(scope.client.TypeId).toBeDefined();
        expect(scope.client.Name).toBeDefined();
        expect(scope.client.Address).toBeDefined();
        expect(scope.client.Address2).toBeDefined();
        expect(scope.client.City).toBeDefined();
        expect(scope.client.State).toBeDefined();
        expect(scope.client.Zip).toBeDefined();
        expect(scope.client.Phone).toBeDefined();
        expect(scope.client.Extension).toBeDefined();
        expect(scope.client.DateAdded).toBeDefined();
        expect(scope.client.DateDropped).toBeDefined();
        expect(scope.client.Fax).toBeDefined();
        expect(scope.client.ReferBy).toBeDefined();
        expect(scope.client.Certificate).toBeDefined();
        expect(scope.client.LifeMember).toBeDefined();
        expect(scope.client.Email).toBeDefined();
        expect(scope.client.Graduate).toBeDefined();
        expect(scope.client.AlumniAnnual).toBeDefined();
        expect(scope.client.AlumniID).toBeDefined();
        expect(scope.client.DDSFirstName).toBeDefined();
        expect(scope.client.DDSLastName).toBeDefined();
    });
    it('should save an item', function () {
        var newname = "My New Name";
        scope.client.IsActive = false;
        scope.client.Name = "My New Name";
        scope.save();
        scope.get();
        expect(scope.client.IsActive).toEqual(false);
        expect(scope.client.Name).toEqual(newname);
    });
    it('should delete an item correctly', function () {
        var oldId = scope.client.Id;
        scope.delete();
        scope.get();
        expect(scope.client.Id).not.toEqual(oldId);
    });
});
