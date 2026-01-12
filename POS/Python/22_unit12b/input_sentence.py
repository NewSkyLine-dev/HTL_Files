"""
author: Oppermann Fabian
file_name: input_sentence.py
"""

class text_analyzer:
    def input_sentences(self):
        sentenceList = []
        while True:
            try:
                sentence = input("Bitte geben Sie die zu analysierenden Sätze ein (CTRL-D bricht ab): ")
                sentenceList.append(sentence)
            except EOFError:
                break
        return sentenceList 


def split_sentence(lst):
    res = []
    lst2 = []
    for words in lst:
        repl = words.replace(".", "")
        lst2.append(repl)
    for elems in lst2:
        sub = elems.split()
        res.append(sub)
    return res


def purge_bad_words(lst):
    for wordInd in lst:
        for deepWord in wordInd:
            if len(deepWord) <= 2:
                del wordInd[wordInd.index(deepWord)]
            elif not deepWord.isalpha():
                del wordInd[wordInd.index(deepWord)]
            else:
                continue 
    return lst
        
        
def analyze_words(lst):
    lst_copy = []
    approx = {}
    for sentences in lst:
        for words in sentences:
            lst_copy.append(words.lower())
    for words in lst_copy:
        approx[words] = lst_copy.count(words)
    return approx

        
def analyze_letters(lst):
    approx = {}
    lettersL = []
    for Sentences in lst:
        for words in Sentences:
            for letter in words:
                lettersL.append(letter.lower())
    for letters in lettersL:
        approx[letters] = lettersL.count(letters)
    return approx

    

TextFunction = text_analyzer()

lowEndList = TextFunction.input_sentences()

splitedList = split_sentence(lowEndList)

pruged = purge_bad_words(splitedList)
            
analyze_words = analyze_words(pruged)

print(analyze_letters(pruged))
