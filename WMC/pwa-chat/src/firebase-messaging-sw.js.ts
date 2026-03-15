import firebase from 'firebase/compat/app';
import { environment } from './environment/environment.prod';

importScripts("https://www.gstatic.com/firebasejs/10.7.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/10.7.0/firebase-messaging-compat.js");

firebase.initializeApp(environment.firebaseConfig);

const messaging = firebase.messaging();

messaging.onBackgroundMessage(function (payload) {
  (self as any).registration.showNotification(payload.notification.title, {
    body: payload.notification.body,
  });
})
