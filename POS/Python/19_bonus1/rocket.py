import turtle
import time

# Konstanten

G = 6.67428e-11
M = 7.3477e22
R = 1737100

# Anfangsparameter

v0 = 0
s0 = R + 100000
t = 0
fuel = 10000
thrust = 0

# Simulationsparameter

delta_t = 0.1

# Funktionen

def acceleration(thrust, fuel):
    if thrust == 0:
        return -G * M / (s0 * s0)
    else:
        return (thrust * fuel / (s0 * s0)) - G * M / (s0 * s0)

def output_state(v, s, a, t, fuel, thrust):
    print("Geschwindigkeit: %.2f m/s" % v)
    print("Höhe: %.2f m" % s)
    print("Beschleunigung: %.2f m/s2" % a)
    print("Zeit: %.2f s" % t)
    print("Treibstoff: %.2f L" % fuel)
    print("Schub: %d" % thrust)

# Hauptschleife

def switchon_thrust():
    global thrust
    thrust = 1

def switchoff_thrust():
    global thrust
    thrust = 0

def init_turtle():
    global screen
    screen = turtle.getscreen()
    screen.onkey(switchon_thrust, "1")
    screen.onkey(switchoff_thrust, "0")
    screen.listen()

# Hauptprogramm

init_turtle()

while s0 > 0:
    a = acceleration(thrust, fuel)
    fuel -= thrust * delta_t
    v0 += a * delta_t
    s0 += v0 * delta_t + a * delta_t * delta_t / 2
    t += delta_t
    output_state(v0, s0, a, t, fuel, thrust)
    time.sleep(delta_t)

print("Landung erfolgreich!")