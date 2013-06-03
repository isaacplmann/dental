// Karma configuration
// Generated on Mon May 27 2013 14:23:22 GMT+0100 (Morocco Daylight Time)


// base path, that will be used to resolve files and exclude
basePath = '../';


// list of files / patterns to load in the browser
files = [
  // testing framework
  JASMINE,
  JASMINE_ADAPTER,

  // Libraries
  "./js/jquery-1.9.1.min.js",
  "./js/angular.min.js",

  // App code
  "./js/*.js",

  // Midway testing module
  "./test/lib/ngMidwayTester.js",

  // Midway tests
  "./test/midway/*.js",
];


// list of files to exclude
exclude = [
];


// web server port
port = 9876;


// cli runner port
runnerPort = 9100;

logLevel = LOG_INFO;

shared = require(__dirname + "/karma.shared.conf.js").shared
colors = shared.colors;
singleRun = shared.singleRun;
autoWatch = shared.autoWatch;
browsers = shared.defaultBrowsers;
reporters = shared.defaultReporters;
proxies = shared.defaultProxies;
captureTimeout = shared.captureTimeout;
