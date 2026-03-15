const express = require('express');
const admin = require('firebase-admin');
const cors = require('cors');

const app = express();
app.use(express.json());
app.use(cors({ origin: '*' }));

// Initialize Firebase Admin once at startup
admin.initializeApp({
  credential: admin.credential.cert(require('./serviceAccountKey.json')),
});

let tokens = []; // In-memory store (use a DB for persistence)

app.post('/register', (req, res) => {
  const token = req.body.token;
  if (token && !tokens.includes(token)) {
    tokens.push(token);
    console.log(`Token registriert. Gesamt: ${tokens.length}`);
  }
  res.send('Token gespeichert');
});

app.post('/send', async (req, res) => {
  const message = req.body.message || 'Neue Nachricht';

  if (tokens.length === 0) {
    return res.json({ successCount: 0, failureCount: 0, info: 'Keine registrierten Tokens' });
  }

  const multicastMessage = {
    notification: {
      title: 'Neue Nachricht',
      body: message,
    },
    tokens: tokens, // required field
  };

  try {
    const response = await admin.messaging().sendEachForMulticast(multicastMessage);
    console.log(`Erfolgreich: ${response.successCount}, Fehler: ${response.failureCount}`);

    // Remove invalid tokens
    const failedTokens = [];
    response.responses.forEach((resp, idx) => {
      if (!resp.success) {
        failedTokens.push(tokens[idx]);
        console.log('Fehler für Token:', tokens[idx], resp.error?.message);
      }
    });
    tokens = tokens.filter((t) => !failedTokens.includes(t));

    res.json({
      successCount: response.successCount,
      failureCount: response.failureCount,
    });
  } catch (error) {
    console.error('Fehler beim Senden:', error);
    res.status(500).json({ error: 'Push konnte nicht gesendet werden', detail: error.message });
  }
});

app.listen(3333, () => console.log('Server läuft auf Port 3333'));
