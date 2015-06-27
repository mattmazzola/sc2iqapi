let controller = {
  * get(next) {
    yield next;
    this.body = 'GET: sections';
  }
};

export default controller;