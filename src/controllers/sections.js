const controller = {
  basePath: '/api',

  getName: '/sections',
  *get(next) {
    yield next;
    this.body = 'GET: assignments from imported controller';
  }
}

export default controller;
