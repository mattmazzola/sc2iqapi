let sectionsController = {
  * get(next) {
    yield next;
    this.body = 'GET: assignments';
  }
};

export default sectionsController;