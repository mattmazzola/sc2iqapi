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
app.use(function *(){
  this.body = 'Hello World';
});
app.use(logger());

app.init = () => {
  let port = process.env.PORT || 44360;
  app.listen(port);
  console.log(`Running: ${process.title} ${process.version}`);
  console.log(`Listening on port: ${port}`);
};

app.init();