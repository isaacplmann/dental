//
// test/e2e/routesSpec.js
//
describe("E2E: Admin Testing Routes", function () {

    beforeEach(function () {
        browser().navigateTo('/index.html');
    });
     
    it('should have a working #/ route', function () {
        browser().navigateTo('#/'); 
        expect(browser().location().path()).toBe("/");
        expect(element('.brand').html()).toContain('Home');
    });
    it('should have a working /clients route', function () {
        browser().navigateTo('#/clients');
        expect(browser().location().path()).toBe("/clients");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /clients/:clientId route', function () {
        browser().navigateTo('#/clients/1');
        expect(browser().location().path()).toBe("/clients/1");
        expect(element('.save').html()).toContain('Save');
    });
    it('should have a working /results route', function () {
        browser().navigateTo('#/results');
        expect(browser().location().path()).toBe("/results");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /results/:resultId route', function () {
        browser().navigateTo('#/results/649195');
        expect(browser().location().path()).toBe("/results/649195");
        expect(element('.save').html()).toContain('Save');
    });
    it('should have a working /equipment route', function () {
        browser().navigateTo('#/equipment');
        expect(browser().location().path()).toBe("/equipment");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /equipment/:equipId route', function () {
        browser().navigateTo('#/equipment/48');
        expect(browser().location().path()).toBe("/equipment/48");
        expect(element('.save').html()).toContain('Save');
    });
    it('should have a working /orders route', function () {
        browser().navigateTo('#/orders');
        expect(browser().location().path()).toBe("/orders");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /orders/:orderId route', function () {
        browser().navigateTo('#/orders/4457');
        expect(browser().location().path()).toBe("/orders/4457");
        expect(element('.save').html()).toContain('Save');
    });
    it('should have a working /clients/:clientId/results route', function () {
        browser().navigateTo('#/clients/2539/results');
        expect(browser().location().path()).toBe("/clients/2539/results");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /clients/:clientId/results/:resultId route', function () {
        browser().navigateTo('#/clients/2539/results/649195');
        expect(browser().location().path()).toBe("/clients/2539/results/649195");
        expect(element('.save').html()).toContain('Save');
    });
    it('should have a working /clients/:clientId/equipment route', function () {
        browser().navigateTo('#/clients/2539/equipment');
        expect(browser().location().path()).toBe("/clients/2539/equipment");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /clients/:clientId/equipment/:equipmentId route', function () {
        browser().navigateTo('#/clients/2539/equipment/3592');
        expect(browser().location().path()).toBe("/clients/2539/equipment/3592");
        expect(element('.save').html()).toContain('Save');
    });
    it('should have a working /clients/:clientId/orders route', function () {
        browser().navigateTo('#/clients/2539/orders');
        expect(browser().location().path()).toBe("/clients/2539/orders");
        expect(element('.container').html()).toContain('gridStyle');
    });
    it('should have a working /clients/:clientId/orders/:orderId route', function () {
        browser().navigateTo('#/clients/2539/orders/11867');
        expect(browser().location().path()).toBe("/clients/2539/orders/11867");
        expect(element('.save').html()).toContain('Save');
    });
});
