import { Component, OnInit } from '@angular/core';
import { NgIf } from '@angular/common';
import { PushService } from './services/push';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [FormsModule, NgIf],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  message = '';
  sendStatus = '';

  constructor(public push: PushService) {}

  get registrationStatus() {
    return this.push.registrationStatus;
  }

  async ngOnInit() {
    await this.push.requestPermission();
    this.push.listen();
  }

  async send() {
    if (!this.message.trim()) return;
    this.sendStatus = 'Senden...';
    try {
      const res = await fetch('http://localhost:3333/send', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ message: this.message }),
      });
      const data = await res.json();
      this.sendStatus = `Gesendet! Erfolgreich: ${data.successCount}, Fehler: ${data.failureCount}`;
      this.message = '';
    } catch (e: any) {
      this.sendStatus = 'Fehler beim Senden: ' + (e?.message ?? e);
    }
  }
}
