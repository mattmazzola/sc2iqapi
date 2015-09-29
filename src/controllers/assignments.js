import Guid from 'guid';

const azure = require('azure-storage');

const azureTableService = azure.createTableService('sc2iq', 'sxSu1H9mUeJvkrixmdD5EN+GdRx92XcR4tw1gnUl+rpfEim3c7w9ruIy3lO7aEuF3/TWWl5cj6KwMi/bY8cwkw==');
azureTableService.createTableIfNotExists('mytable', function(error, result, response){
    if(!error){
        // Table exists or created
    }
});

const entGen = azure.TableUtilities.entityGenerator;

const controller = {
  basePath: '/api',

  getName: '/assignments',
  *get(next) {
    const guid = Guid.raw();
    const entity = {
      PartitionKey: entGen.String('assignments'),
      RowKey: entGen.String(guid),
      boolValueTrue: entGen.Boolean(true),
      boolValueFalse: entGen.Boolean(false),
      intValue: entGen.Int32(42),
      dateValue: entGen.DateTime(new Date(Date.UTC(2011, 10, 25))),
      complexDateValue: entGen.DateTime(new Date(Date.UTC(2013, 2, 16, 1, 46, 20)))
    };

    azureTableService.insertEntity('mytable', entity, function (error, result, response) {
        if(!error){
            // Entity inserted
        }
    });

    this.body = `GET: assignments from imported controller: ${guid}
      ${JSON.stringify(entity, null, '  ')}
    `;
  },
};

export default controller;
