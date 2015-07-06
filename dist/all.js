'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

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

var _iteratorNormalCompletion = true;
var _didIteratorError = false;
var _iteratorError = undefined;

try {
  for (var _iterator = _fs2['default'].readdirSync(controllersPath)[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
    var fileName = _step.value;

    var controllerName = fileName.substring(0, fileName.length - 3);
    var controller = require(controllersPath + '/' + fileName);

    console.log('Register Controller: ' + controllerName);

    for (var property in controller) {
      var httpMethod = property; /** TODO: Validate */
      var path = '/api/' + controllerName;
      var handler = controller[property];

      console.log(property + ': ' + path);

      router[httpMethod](path, handler);
    }
  }
} catch (err) {
  _didIteratorError = true;
  _iteratorError = err;
} finally {
  try {
    if (!_iteratorNormalCompletion && _iterator['return']) {
      _iterator['return']();
    }
  } finally {
    if (_didIteratorError) {
      throw _iteratorError;
    }
  }
}

app.init = function () {
  var port = process.env.PORT || 44360;
  app.listen(port);
  console.log('Listening on port: ' + port);
};

exports['default'] = app;
module.exports = exports['default'];
//# sourceMappingURL=all.js.map