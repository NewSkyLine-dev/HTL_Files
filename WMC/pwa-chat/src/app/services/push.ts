import { Injectable } from '@angular/core';
import { initializeApp, FirebaseApp } from 'firebase/app';
import { environment } from '../../environment/environment.prod';

@Injectable({ providedIn: 'root' })
export class PushService {
  private messaging: any = null;
  private messagingModule: any = null;
  private supported = false;
  private initPromise: Promise<void>;

  registrationStatus = '';

  constructor() {
    const app: FirebaseApp = initializeApp(environment.firebaseConfig);

    this.initPromise = (async () => {
      try {
        const msg = await import('firebase/messaging');
        this.messagingModule = msg;
        const s = await msg.isSupported();
        this.supported = !!s;
        if (s) this.messaging = msg.getMessaging(app);
      } catch (e) {
        console.error('Firebase messaging init failed:', e);
        this.supported = false;
      }
    })();
  }

  async requestPermission(): Promise<string> {
    await this.initPromise;

    if (!this.supported) {
      this.registrationStatus = 'Push-Benachrichtigungen werden nicht unterstuetzt.';
      console.warn('Push messaging is not supported in this browser.');
      return 'unsupported';
    }

    const permission = await Notification.requestPermission();

    if (permission !== 'granted') {
      this.registrationStatus = 'Benachrichtigungen verweigert.';
      return 'denied';
    }

    try {
      const msg = this.messagingModule ?? (await import('firebase/messaging'));
      const token = await msg.getToken(this.messaging, {
        vapidKey: environment.vapidKey,
      });

      console.log('FCM Token:', token);
      const res = await fetch('http://localhost:3333/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ token }),
      });

      if (res.ok) {
        this.registrationStatus = 'Registrierung erfolgreich! Token: ' + token.substring(0, 20) + '...';
        return 'granted';
      } else {
        this.registrationStatus = 'Registrierung fehlgeschlagen (Server-Fehler).';
        return 'error';
      }
    } catch (e: any) {
      console.error('Token acquisition failed:', e);
      this.registrationStatus = 'Fehler beim Token: ' + (e?.message ?? e);
      return 'error';
    }
  }

  listen() {
    this.initPromise.then(() => {
      if (!this.supported || !this.messaging) return;
      const msg = this.messagingModule;
      msg.onMessage(this.messaging, (payload: any) => {
        console.log('Foreground message:', payload);
        const title = payload.notification?.title ?? 'Neue Nachricht';
        const body = payload.notification?.body ?? '';
        if (Notification.permission === 'granted') {
          new Notification(title, { body, icon: '/icons/icon-192x192.png' });
        } else {
          alert(title + '\n' + body);
        }
      });
    });
  }
}
