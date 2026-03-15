importScripts("https://www.gstatic.com/firebasejs/10.7.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/10.7.0/firebase-messaging-compat.js");

firebase.initializeApp({
  apiKey: "AIzaSyDjRrRVRs5bClcmXcVCIAogBi-k0OD3ec0",
  authDomain: "pwa-chat-6e3c8.firebaseapp.com",
  projectId: "pwa-chat-6e3c8",
  storageBucket: "pwa-chat-6e3c8.firebasestorage.app",
  messagingSenderId: "525181723385",
  appId: "1:525181723385:web:0d8bd8613edc4a343a7828"
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage(function (payload) {
  console.log('[firebase-messaging-sw.js] Received background message:', payload);
  const notificationTitle = payload.notification?.title || 'Neue Nachricht';
  const notificationOptions = {
    body: payload.notification?.body || '',
    icon: '/icons/icon-192x192.png',
    badge: '/icons/icon-72x72.png',
  };
  self.registration.showNotification(notificationTitle, notificationOptions);
});
