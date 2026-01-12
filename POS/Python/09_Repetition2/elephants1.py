weight = int(input("Gewicht: "))
color = input("Farbe: ")
origin = input("Herkunft: ")

if weight == 3500 and color == "grey" and origin == "Africa":
    print("Category 1")
if weight == 3500 and color == "brown" and origin == "Africa":
    print("Category 2")
if weight == 3500 and color == "white" and origin == "Africa":
    print("Category 3")
if weight == 4000 and color == "grey" and origin == "Africa":
    print("Category 4")
if weight == 4000 and color == "brown" and origin == "Africa":
    print("Category 5")
if weight == 4000 and color == "brown" and origin == "India":
    print("Category 6")
if weight == 4000 and color == "white" and origin == "India":
    print("Category 7")
if weight == 4500 and color == "grey" and origin == "India":
    print("Category 8")
if weight == 4500 and color == "brown" and origin == "India":
    print("Category 9")
if weight == 4500 and color == "white" and origin == "India":
    print("Category 10")
else:
    print(-1)