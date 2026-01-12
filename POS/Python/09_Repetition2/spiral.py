import turtle


def spiral(angle, max_size):
    for i in range(max_size):
        turtle.forward(i)
        turtle.left(angle)

spiral(110, 100)