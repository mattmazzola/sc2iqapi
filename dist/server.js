'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

// auto init if this app is not being initialized by another module (i.e. using require('./app').init();)
// if (!module.parent) {
//   try {
//     app.init();
//   }
//   catch (err) {
//     console.error(err);
//     process.exit(1);
//   }
// }

var _babelPolyfill = require('babel/polyfill');

var _babelPolyfill2 = _interopRequireDefault(_babelPolyfill);

var _fs = require('fs');

var _fs2 = _interopRequireDefault(_fs);

var _koa = require('koa');

var _koa2 = _interopRequireDefault(_koa);

var _koaBodyparser = require('koa-bodyparser');

var _koaBodyparser2 = _interopRequireDefault(_koaBodyparser);

var _koaRouter = require('koa-router');

var _koaRouter2 = _interopRequireDefault(_koaRouter);

var _koaLogger = require('koa-logger');

var _koaLogger2 = _interopRequireDefault(_koaLogger);

require('babel/register');
var app = require('./server');

var app = (0, _koa2['default'])();
var router = (0, _koaRouter2['default'])();

app.use(regeneratorRuntime.mark(function responseTime(next) {
  var start, ms;
  return regeneratorRuntime.wrap(function responseTime$(context$1$0) {
    while (1) switch (context$1$0.prev = context$1$0.next) {
      case 0:
        start = new Date();
        context$1$0.next = 3;
        return next;

      case 3:
        ms = new Date() - start;

        this.set('X-Response-Time', ms + 'ms');

      case 5:
      case 'end':
        return context$1$0.stop();
    }
  }, responseTime, this);
}));
app.use(router.routes());
app.use(router.allowedMethods());
app.use((0, _koaLogger2['default'])());

var controllersPath = __dirname + '/controllers';

router.get('/api/assignments', regeneratorRuntime.mark(function callee$0$0(next) {
  return regeneratorRuntime.wrap(function callee$0$0$(context$1$0) {
    while (1) switch (context$1$0.prev = context$1$0.next) {
      case 0:
        context$1$0.next = 2;
        return next;

      case 2:
        this.body = 'GET: assignments';

      case 3:
      case 'end':
        return context$1$0.stop();
    }
  }, callee$0$0, this);
}));

// for (let fileName of fs.readdirSync(controllersPath)) {
//   let controllerName = fileName.substring(0, fileName.length - 3);
//   let controller = require(controllersPath + '/' + fileName);

//   console.log(`Register Controller: ${controllerName}`);

//   for(let property in controller) {
//     let httpMethod = property; /** TODO: Validate */
//     let path = `/api/${controllerName}`;
//     let handler = controller[property];

//     console.log(`${property}: ${path}`);

//     router[httpMethod](path, handler);
//   }
// }

app.init = function () {
  var port = process.env.PORT || 44360;
  app.listen(port);
  console.log('Running: ' + process.title + ' ' + process.version);
  console.log('Listening on port: ' + port);
};

exports['default'] = app;

app.init();
var sectionsController = {
  get: regeneratorRuntime.mark(function get(next) {
    return regeneratorRuntime.wrap(function get$(context$1$0) {
      while (1) switch (context$1$0.prev = context$1$0.next) {
        case 0:
          context$1$0.next = 2;
          return next;

        case 2:
          this.body = 'GET: assignments';

        case 3:
        case 'end':
          return context$1$0.stop();
      }
    }, get, this);
  })
};

exports['default'] = sectionsController;

var assignmentsController = {
  get: regeneratorRuntime.mark(function get(next) {
    return regeneratorRuntime.wrap(function get$(context$1$0) {
      while (1) switch (context$1$0.prev = context$1$0.next) {
        case 0:
          context$1$0.next = 2;
          return next;

        case 2:
          this.body = 'GET: sections';

        case 3:
        case 'end':
          return context$1$0.stop();
      }
    }, get, this);
  })
};

exports['default'] = assignmentsController;
module.exports = exports['default'];
//# sourceMappingURL=server.js.map