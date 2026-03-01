const express = require('express');
const admin = require('firebase-admin');
const cors = require('cors');
const app = express();
app.use(express.json());
app.use(cors());
admin.initializeApp({ credential: admin.credential.cert(require('./serviceAccountKey.json')) });
let tokens = [];
app.post('/register', (req, res) => {
  tokens.push(req.body.token);
  res.send('Token gespeichert');
});
app.post('/send', async (req, res) => {
  const payload = { notification: { title: 'Neue Nachricht', body: req.body.message } };
  for (const token of tokens) {
    await admin.messaging().sendToDevice(token, payload);
  }
  res.send('Push gesendet');
});
app.listen(3000, () => console.log('Server läuft'));
