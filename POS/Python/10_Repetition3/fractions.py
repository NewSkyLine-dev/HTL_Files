def add_fractions(n1, d1, n2, d2):
    n = n1 * d2 + n2 * d1
    d = d1 * d2
    return n, d


def sub_fractions(n1, d1, n2, d2):
    n = n1 * d2 - n2 * d1
    d = d1 * d2
    return n, d


def mul_fractions(n1, d1, n2, d2):
    n = n1 * n2
    d = d1 * d2
    return n, d


def div_fractions(n1, d1, n2, d2):
    n = n1 * d2
    d = n2 * d1
    return n, d