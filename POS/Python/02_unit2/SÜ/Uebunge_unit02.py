"""
author: Fabian Oppermann
file_name: Uebunge_unit02.py
"""

#14
import math

area = 1000
side_length = math.sqrt(area)
perimeter = side_length * 4

print("The side length is", side_length, "m")
print("The perimeter is", perimeter, "m")

print("----------------------------------------------------------------")
#15
import math

perimeter = 1020.5
side_length = 0.35
remaining_side_length = perimeter - (2 * side_length)
area = side_length * remaining_side_length

print("The remaining side length is", remaining_side_length, "m")
print("The area is", area, "m^2")

print("----------------------------------------------------------------")

#16
def area_circle(diameter):
    radius = diameter / 2
    area = math.pi * radius ** 2
    return area


def circumference_circle(diameter):
    radius = diameter / 2
    circumference = 2 * math.pi * radius
    return circumference


diameter = 15
print("Area of circle with diameter of 15cm is:", area_circle(diameter))
print("Circumference of circle with diameter of 15cm is:", circumference_circle(diameter))

print("-------------------------------------------------------------------------------------------------")

#17
def main():
    area = 323
    radius = (area / math.pi) ** 0.5
    circumference = 2 * math.pi * radius
    print("The radius is", radius, "m")
    print("The circumference is", circumference, "m")


if __name__ == '__main__':
    main()

print("------------------------------------------------------------------------------------------------")

#19
def volume(diameter, height):
    return (diameter / 2) ** 2 * height * math.pi


def circumference(diameter):
    return diameter * math.pi


print(volume(30, 5))
print(circumference(30))
print(volume(30, 5) / 8)
print(circumference(30) / 8)

print("------------------------------------------------------------------------------------------------")

#20
def digit_sum(n):
    return sum(int(i) for i in str(n))


print(digit_sum(37812901))

print("------------------------------------------------------------------------------------------------")

#21
print(int('1', 2))
print(int('10', 2))
print(int('11', 2))
print(int('011', 2))
print(int('110', 2))
print(int('101', 2))
print(int('1011010', 2))

print("------------------------------------------------------------------------------------------------")

