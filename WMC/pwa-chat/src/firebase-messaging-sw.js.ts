import firebase from 'firebase/compat/app';

importScripts("https://www.gstatic.com/firebasejs/10.7.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/10.7.0/firebase-messaging-compat.js");

firebase.initializeApp({
  apiKey: '',
  authDomain: 'pwa-chat-6e3c8.firebaseapp.com',
  projectId: 'pwa-chat-6e3c8',
  storageBucket: 'pwa-chat-6e3c8.firebasestorage.app',
  messagingSenderId: '525181723385',
  appId: '1:525181723385:web:0d8bd8613edc4a343a7828',
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage(function (payload) {
  (self as any).registration.showNotification(payload.notification.title, {
    body: payload.notification.body,
  });
})
