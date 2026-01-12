"""
author: Fabian Oppermann
file_name: linfunc2.py
"""


# Initialize variables
k = 3
d = 0

# Print results
print("f(x) = 3x")
print("k = 3")
print("d = 0")
print("f(-5) = -15")
print("f(-4) = -12")
print("f(-3) = -9")
print("f(-2) = -6")
print("f(-1) = -3")
print("f(0) = 0")
print("f(1) = 3")
print("f(2) = 6")
print("f(3) = 9")
print("f(4) = 12")
print("f(5) = 15")

# Draw graph
import matplotlib.pyplot as plt

x = [-5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5]
y = [-15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15]

plt.plot(x, y)
plt.xlabel('x')
plt.ylabel('y')
plt.title('f(x) = 3x')
plt.grid(True)
plt.show()