basePath = '../';

files = [
  ANGULAR_SCENARIO,
  ANGULAR_SCENARIO_ADAPTER,
  // testing framework
  //JASMINE,
  //JASMINE_ADAPTER,

  // Libraries
  "./js/jquery-1.9.1.min.js",
  "./js/angular.min.js",

  // App code
  "./js/*.js",

  './test/e2e/**/*.js'
];

port = 9202;
runnerPort = 9303;
captureTimeout = 10000;

shared = require(__dirname + "/karma.shared.conf.js").shared
growl = shared.colors;
colors = shared.colors;
singleRun = shared.singleRun;
autoWatch = shared.autoWatch;
browsers = shared.defaultBrowsers;
reporters = shared.defaultReporters;
proxies = {
    '/': 'http://localhost:51171/'
};