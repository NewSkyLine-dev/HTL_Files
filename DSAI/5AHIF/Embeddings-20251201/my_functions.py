import matplotlib.pyplot as plt


def draw_vectors(vectors, xlim, ylim, title):
    # Create a figure and axis
    fig, ax = plt.subplots()

    # Set the limits of the coordinate system
    ax.set_xlim(xlim[0], xlim[1])
    ax.set_ylim(ylim[0], ylim[1])

    # Plot the vector
    for vector in vectors:
        ax.quiver(
            0,
            0,
            vector[0],
            vector[1],
            angles="xy",
            scale_units="xy",
            scale=1,
            color=vector[2],
        )

    ax.axvline(x=0, color="grey", lw=1)
    ax.axhline(y=0, color="grey", lw=1)

    # Set the labels and title
    ax.set_xlabel("x")
    ax.set_ylabel("y")
    ax.set_title(title)

    # Show the plot
    plt.show()
