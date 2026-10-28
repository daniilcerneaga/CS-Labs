using System;
using System.Linq;
using System.Text;

namespace CaesarCipherLab;

internal static class Program
{
    // Table 2 in the laboratory assignment: positions 0..30.
    // These are letters, not Unicode code-point offsets.
    private const string RomanianAlphabet = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ";
    private const int AlphabetSize = 31;

    private static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Laboratory Work 1: Caesar Cipher (Romanian alphabet) ===");
        Console.WriteLine($"Alphabet ({AlphabetSize} letters): {RomanianAlphabet}");

        while (true)
        {
            Console.WriteLine("\nChoose task:");
            Console.WriteLine("1 - Task 1.1: Standard Caesar cipher");
            Console.WriteLine("2 - Task 1.2: Caesar cipher with keyword permutation");
            Console.WriteLine("0 - Exit");

            string choice = ReadChoice("Your choice: ", "0", "1", "2");
            if (choice == "0")
                break;

            string operation = ReadChoice("Operation (E = encrypt, D = decrypt): ", "E", "D");
            int key = ReadShift();

            string alphabet = RomanianAlphabet;
            if (choice == "2")
            {
                string keyword = ReadKeyword();
                alphabet = BuildPermutedAlphabet(keyword);
                Console.WriteLine($"Permuted alphabet: {alphabet}");
            }

            string message = ReadMessage(operation == "E" ? "Plaintext: " : "Ciphertext: ");
            string result = Transform(message, key, alphabet, encrypt: operation == "E");

            Console.WriteLine(operation == "E" ? $"Encrypted text: {result}" : $"Decrypted text: {result}");
        }
    }

    private static string ReadChoice(string prompt, params string[] allowed)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
            if (allowed.Contains(value))
                return value;

            Console.WriteLine($"Invalid choice. Allowed values: {string.Join(", ", allowed)}.");
        }
    }

    private static int ReadShift()
    {
        while (true)
        {
            Console.Write("Numeric key (1-30): ");
            string value = Console.ReadLine() ?? "";
            if (int.TryParse(value, out int key) && key >= 1 && key <= 30)
                return key;

            Console.WriteLine("Invalid numeric key. Enter an integer from 1 to 30 inclusive.");
        }
    }

    private static string ReadMessage(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string raw = Console.ReadLine() ?? "";
            string normalized = NormalizeRomanian(raw).Replace(" ", "");

            if (normalized.Length == 0)
            {
                Console.WriteLine("Invalid text. Enter at least one Romanian letter (spaces are allowed).");
                continue;
            }

            if (TryFindInvalidLetter(normalized, out char invalid))
            {
                Console.WriteLine($"Invalid character '{invalid}'. Only Romanian alphabet letters (A-Z, Ă, Â, Î, Ș, Ț and lowercase equivalents) and spaces are allowed.");
                continue;
            }

            return normalized;
        }
    }

    private static string ReadKeyword()
    {
        while (true)
        {
            Console.Write("Keyword (at least 7 Romanian letters, no spaces): ");
            string raw = Console.ReadLine() ?? "";
            string keyword = NormalizeRomanian(raw);

            if (TryFindInvalidLetter(keyword, out char invalid))
            {
                Console.WriteLine($"Invalid keyword character '{invalid}'. Only Romanian alphabet letters (A-Z, Ă, Â, Î, Ș, Ț and lowercase equivalents), without spaces, are allowed.");
                continue;
            }

            if (keyword.Length < 7)
            {
                Console.WriteLine("Invalid keyword. It must contain at least 7 Romanian letters.");
                continue;
            }

            return keyword;
        }
    }

    private static string NormalizeRomanian(string text)
    {
        // Accept both comma-below (Ș/Ț) and cedilla variants (Ş/Ţ).
        // FormC also combines decomposed accents such as A + breve into Ă.
        return text.Normalize(NormalizationForm.FormC)
            .ToUpperInvariant()
            .Replace('Ş', 'Ș')
            .Replace('Ţ', 'Ț')
            .Normalize(NormalizationForm.FormC);
    }

    private static bool TryFindInvalidLetter(string text, out char invalid)
    {
        foreach (char letter in text)
        {
            if (RomanianAlphabet.IndexOf(letter) < 0)
            {
                invalid = letter;
                return true;
            }
        }

        invalid = default;
        return false;
    }

    private static string BuildPermutedAlphabet(string keyword)
    {
        var result = new StringBuilder(AlphabetSize);

        foreach (char letter in keyword + RomanianAlphabet)
        {
            if (result.ToString().IndexOf(letter) < 0)
                result.Append(letter);
        }

        return result.ToString();
    }

    private static string Transform(string text, int key, string alphabet, bool encrypt)
    {
        var result = new StringBuilder(text.Length);
        foreach (char letter in text)
        {
            int position = alphabet.IndexOf(letter);
            int newPosition = encrypt
                ? (position + key) % AlphabetSize
                : (position - key + AlphabetSize) % AlphabetSize;
            result.Append(alphabet[newPosition]);
        }

        return result.ToString();
    }
}
