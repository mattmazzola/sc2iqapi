let controller = {
  * get(next) {
    yield next;
    this.body = 'GET: assignments';
  }
};

export default controller;