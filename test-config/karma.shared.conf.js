var shared = {};
shared.singleRun = false
shared.autoWatch = true
shared.colors = true
shared.growl = true

// If browser does not capture in given timeout [ms], kill it
shared.captureTimeout = 60000;

//shared.defaultLogLevel = LOG_INFO;

shared.defaultReporters = ['progress'];

// Start these browsers, currently available:
// - Chrome
// - ChromeCanary
// - Firefox
// - Opera
// - Safari (only Mac)
// - PhantomJS
// - IE (only Windows)
shared.defaultBrowsers = ['Chrome'];
shared.defaultProxies = {
    '/': 'http://localhost:8000'
};

exports.shared = shared;