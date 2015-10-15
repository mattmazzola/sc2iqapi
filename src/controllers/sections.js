const Firebase = require('firebase');
import FirebaseTokenGenerator from 'firebase-token-generator';
import Guid from 'guid';
import moment from 'moment';

// TODO: Move to configuration file
const firebaseUrl = "https://sc2iq.firebaseio.com/";
const firebaseSecret = 'xbpht3WzBto3lWyXgE9qCDJzMnr22aImn8LwYrUk';

const tokenGenerator = new FirebaseTokenGenerator(firebaseSecret);
const now = new Date();
now.setDate(now.getDate()+1)
const expiration = now.getTime();
console.log(`expiration: ${expiration}`);

const token = tokenGenerator.createToken({uid: "sc2iqapi"}, { expires: expiration });

const firebase = new Firebase(firebaseUrl);
firebase.authWithCustomToken(token, function(error, authData) {
  if(error) {
    console.log(`Error authenticating with firebase: `, error);
    return;
  }

  console.log('Firebase Authentication Successful: ', authData);
});

const controller = {
  basePath: '/api',

  getName: '/sections',
  *get(next) {

    const object = {
      id: Guid.raw(),
      name: 'Test Object'
    };

    firebase.child('sc2iqap').push(object);
    // const reference = 'https://www.bing.com';

    yield next;

    this.body = { object };
  }
}

export default controller;
