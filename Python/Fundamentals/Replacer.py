def Replacer(word, positionToReplace, charToReplace = "_"):
  resultWord = ""
  for idx in range(len(word)):
    if (idx + 1) % positionToReplace == 0:
      resultWord += charToReplace
    else:
      resultWord += word[idx]

  print(resultWord)

Replacer("Kevin is good boy", 3, '*')