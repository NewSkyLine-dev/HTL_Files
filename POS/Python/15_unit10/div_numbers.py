"""
author: Oppermann Fabian
file_name: div_numbers.py
"""

def div_numbers(numbers, i1, i2):
    try:
        return numbers[i1] / numbers[i2]
    except ZeroDivisionError:
        return float("nan")
    except IndexError:
        return None


print(div_numbers((1, 2, 3, 4, 5), 1, 2))