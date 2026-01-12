"""
author: Oppermann Fabian
file_name: bit.py
"""


# a) Verschiebe um jeweils 1 Bit nach links: 0, 1, 2, 3, 4, 5
print(bin(0b1101))
print(bin(0b1101 << 1))
print(bin(0b1101 << 2))
print(bin(0b1101 << 3))
print(bin(0b1101 << 4))
print(bin(0b1101 << 5))

# b) Verschiebe um jeweils 2 Bit nach links: 0, 1, 2, 3, 4, 5
print(bin(0b1101))
print(bin(0b1101 << 2))
print(bin(0b1101 << 4))
print(bin(0b1101 << 6))
print(bin(0b1101 << 8))
print(bin(0b1101 << 10))

# c) Verschiebe um jeweils 1 Bit nach rechts: 0, 1, 2, 3, 4, 5
print(bin(0b1101))
print(bin(0b1101 >> 1))
print(bin(0b1101 >> 2))
print(bin(0b1101 >> 3))
print(bin(0b1101 >> 4))
print(bin(0b1101 >> 5))


def bit(b, i):
    return (b & (1 << i)) >> i


def set(b, i):
    return b | (1 << i)


def clear(b, i):
    return b & ~(1 << i)