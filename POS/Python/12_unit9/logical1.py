a = {1, 2, 3, 4}
b = {3, 4, 5, 6}
c = {3, 4}

print(a.intersection(b)) 
print(a.union(b))
print(a.difference(b))
print(a.symmetric_difference(b))


print(3 in c)
print(a.issubset(b))
print(c.issubset(a))
print(a.issubset(a))