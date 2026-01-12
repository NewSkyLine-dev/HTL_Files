/*
 * File: sw.js
 * Project: Hello-PWA
 * File Created: Friday, 5th September 2025 12:02:05 pm
 * Author: Fabian Oppermann (fabian.oppermann@gmx.net)
 * -----
 * Last Modified: Friday, 5th September 2025 12:02:05 pm
 * Modified By: Fabian Oppermann (fabian.oppermann@gmx.net)
 */

var cacheName = "hello-pwa";
var filesToCache = ["/", "/index.html", "/css/style.css", "/js/main.js"];

/* Start the service worker and cache all of the app's content */
self.addEventListener("install", function (e) {
    e.waitUntil(
        caches.open(cacheName).then(function (cache) {
            return cache.addAll(filesToCache);
        })
    );
});

/* Serve cached content when offline */
self.addEventListener("fetch", function (e) {
    e.respondWith(
        caches.match(e.request).then(function (response) {
            return response || fetch(e.request);
        })
    );
});
