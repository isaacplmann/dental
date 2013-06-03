basePath = '../';

files = [
  // testing framework
  JASMINE,
  JASMINE_ADAPTER,

  // Libraries
  "./js/jquery-1.9.1.min.js",
  "./js/angular.min.js",
  "./js/angular-*.js",

  // App code
  "./js/*.js",

  // Test specific code
  './test/lib/angular-mocks.js',
  './test/mocks/*.js',

  //Test-Specs
  './test/unit/**/*.js'
];

port = 9201;
runnerPort = 9301;
captureTimeout = 60000;

shared = require(__dirname + "/karma.shared.conf.js").shared
growl = shared.colors;
colors = shared.colors;
singleRun = shared.singleRun;
autoWatch = shared.autoWatch;
browsers = shared.defaultBrowsers;
reporters = shared.defaultReporters;