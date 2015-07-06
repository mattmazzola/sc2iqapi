let assignmentsController = {
  * get(next) {
    yield next;
    this.body = 'GET: sections';
  }
};

export default assignmentsController;