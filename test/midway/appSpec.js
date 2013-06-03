//
// test/midway/appSpec.js
//
describe("Midway: Testing Modules", function () {
    describe("App Module:", function () {

        var module;
        beforeEach(function () {
            module = angular.module("osudental");
        });

        it("should be registered", function () {
            expect(module).not.toBe(null);
        });

        describe("Dependencies:", function () {

            var deps;
            var hasModule = function (m) {
                return deps.indexOf(m) >= 0;
            };
            beforeEach(function () {
                deps = module.value('osudental').requires;
            }); 

            //you can also test the module's dependencies
            it("should have ngCookies as a dependency", function () {
                expect(hasModule('ngCookies')).toBe(true);
            });

            it("should have osudentalFilters as a dependency", function () {
                expect(hasModule('osudentalFilters')).toBe(true);
            });

            it("should have ngGrid as a dependency", function () {
                expect(hasModule('ngGrid')).toBe(true);
            });

            it("should have osudentalServices as a dependency", function () {
                expect(hasModule('osudentalServices')).toBe(true);
            });
        });
    });
});