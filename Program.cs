using System.Globalization;
using System.Text;

var outOfBin = "../../../";
var englishWords = GetFile("englishwords.txt").ToArray();
var transalations = GetFile("translations.txt").ToArray();
var learnedWords = GetFile("learnedwords.txt");
var wordsCount = englishWords.Count();

var random = new Random();

var continuePlaying = true;
do
{
    var randomNumber = random.Next(wordsCount);
    if (IsWordLearned(randomNumber)) 
        continue;

    Console.WriteLine($"You've learned {learnedWords.Count()} from {wordsCount} words.");

    DivideConsole();

    Console.WriteLine($"Type the word '{englishWords[randomNumber].ToUpper()}' in portuguese:");

    var answer = Console.ReadLine();
    if (WordsAreEqual(answer, randomNumber))
    {
        DivideConsole();
        Console.WriteLine("Correct!\nDo you like to set this word as learned? (y)");
        if(UserSayYes())
            SetWordToLearnedWordsTxt(randomNumber);
    }
    else
    {
        DivideConsole();
        Console.WriteLine($"Incorrect, the translation is '{transalations[randomNumber]}'");
    }

    DivideConsole();

    Console.WriteLine("Type 'y' to continue or anything else to stop");
    continuePlaying = UserSayYes();

    Console.Clear();
}
while (continuePlaying);

bool WordsAreEqual(string answer, int randomNumber) =>
    RemoveAccents(answer).ToLower() == RemoveAccents(transalations[randomNumber]).ToLower();

void DivideConsole()
{
    Console.WriteLine("_____________________________________________\n");
}

bool UserSayYes() =>
    Console.ReadLine() == "y";

bool IsWordLearned(int randomNumber) =>
    learnedWords.Any(_ => _ == englishWords[randomNumber]);

void SetWordToLearnedWordsTxt(int randomNumber) =>
    File.AppendAllText($"{outOfBin}learnedwords.txt", Environment.NewLine + englishWords[randomNumber]);

IEnumerable<string> GetFile(string filePath) =>
    File.ReadLines($"{outOfBin}{filePath}");

string RemoveAccents(string text)
{
    StringBuilder stringBuilder = new StringBuilder();
    var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
    foreach (char letter in arrayText)
    {
        if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
            stringBuilder.Append(letter);
    }
    return stringBuilder.ToString();
}