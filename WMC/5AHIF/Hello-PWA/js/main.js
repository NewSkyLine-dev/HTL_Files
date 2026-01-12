/*
 * File: main.js
 * Project: js
 * File Created: Friday, 5th September 2025 12:03:01 pm
 * Author: Fabian Oppermann (fabian.oppermann@gmx.net)
 * -----
 * Last Modified: Friday, 5th September 2025 12:03:01 pm
 * Modified By: Fabian Oppermann (fabian.oppermann@gmx.net)
 */

window.onload = () => {
    "use strict";

    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("./sw.js");
    }
};
