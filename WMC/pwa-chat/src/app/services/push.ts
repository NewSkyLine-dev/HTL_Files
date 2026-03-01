import { Injectable } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { firebaseConfig } from '../firebase';

@Injectable({ providedIn: 'root' })
export class PushService {
  messaging: any = null;
  messagingModule: any = null;
  supported = false;

  constructor() {
    const app = initializeApp(firebaseConfig);

    (async () => {
      try {
        const msg = await import('firebase/messaging');
        this.messagingModule = msg;
        const s = await msg.isSupported();
        this.supported = !!s;
        if (s) this.messaging = msg.getMessaging(app);
      } catch (e) {
        this.supported = false;
      }
    })();
  }

  async requestPermission() {
    if (!this.supported) {
      console.warn('Push messaging is not supported in this browser.');
      return;
    }

    const permission = await Notification.requestPermission();

    if (permission === 'granted') {
      const msg = this.messagingModule ?? (await import('firebase/messaging'));
      const token = await msg.getToken(this.messaging, {
        vapidKey: '',
      });

      console.log('Token:', token);
      await fetch('http://localhost:3000/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ token }),
      });
    }
  }

  listen() {
    if (!this.supported) return;

    (async () => {
      const msg = this.messagingModule ?? (await import('firebase/messaging'));
      msg.onMessage(this.messaging, (payload) => {
        alert(payload.notification?.body);
      });
    })();
  }
}
