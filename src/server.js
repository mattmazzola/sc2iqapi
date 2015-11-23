import koa from 'koa';
import Router from 'koa-router';
import logger from 'koa-logger';
import * as controllers from './controllers/controllers';

var app = koa();
var router = Router();

app.use(router.routes());
app.use(router.allowedMethods());
app.use(logger());

app.use(function* responseTime(next) {
  var start = new Date;
  yield next;
  var ms = new Date - start;
  this.set('X-Response-Time', ms + 'ms');
});

Object.keys(controllers).forEach(controllerName => {
  const controller = controllers[controllerName];
  Object.keys(controller).forEach(key => {
    const value = controller[key];
    if(typeof value === 'function') {
      const path = `${controller.basePath}${controller[key + 'Name']}`;
      console.log(`Registring: ${key}: ${path}`);
      router[key](path, value);
    }
  });
});

app.init = () => {
  let port = process.env.PORT || 44360;
  app.listen(port);
  console.log(`Running: ${process.title} ${process.version}`);
  console.log(`Listening on port: ${port}`);
};

app.init();
