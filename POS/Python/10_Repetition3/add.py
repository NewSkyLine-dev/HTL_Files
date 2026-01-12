def add_times(t1, t2, h1, m1, s1, h2, m2, s2):
    return (t1 * 24 + t2 * 24 + h1 + h2) * 3600 + (m1 + m2) * 60 + s1 + s2