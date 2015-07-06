import polyfill from "babel/polyfill";

import fs from 'fs';
import koa from 'koa';
import bodyParser from 'koa-bodyparser';
import Router from 'koa-router';
import logger from 'koa-logger';

var app = koa();
var router = Router();

app.use(function* responseTime(next) {
  var start = new Date;
  yield next;
  var ms = new Date - start;
  this.set('X-Response-Time', ms + 'ms');
});
app.use(router.routes());
app.use(router.allowedMethods());
app.use(logger());

let controllersPath = __dirname + '/controllers';

router.get('/api/assignments', function* (next) {
  yield next;
  this.body = 'GET: assignments';
});

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

app.init = () => {
  let port = process.env.PORT || 44360;
  app.listen(port);
  console.log(`Running: ${process.title} ${process.version}`);
  console.log(`Listening on port: ${port}`);
};

export default app;

app.init();