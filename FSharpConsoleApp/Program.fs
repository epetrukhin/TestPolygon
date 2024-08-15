open System

let splitAtSpaces (text: string) =
    text.Split ' '
    |> Array.toList

let wordCount text =
    let words = splitAtSpaces text
    let numWords = words.Length
    let distinctWords = List.distinct words
    let numDups = numWords - distinctWords.Length
    (numWords, numDups)

let showWordCount text =
    let numWords, numDups = wordCount text
    printfn $"--> {numWords} words in the text"
    printfn $"--> {numDups} duplicate words"

showWordCount "Foo bar and foo bar"

Console.ForegroundColor <- ConsoleColor.Yellow 
Console.WriteLine "Press any key to close..."

Console.ReadKey true |> ignore