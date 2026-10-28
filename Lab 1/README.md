# Laboratory Work No. 1 — Caesar Cipher

C# console application for both tasks in the assignment:

- **1.1** Caesar cipher using the 31-letter Romanian alphabet and numeric shift 1–30.
- **1.2** Caesar cipher using the same numeric shift and a keyword of at least 7 Romanian letters to permute the alphabet.

Includes encryption/decryption, validation with retry prompts, uppercasing, removal of spaces from input messages, keyword deduplication, display of the permuted alphabet, and acceptance of cedilla spellings `Ş/Ţ` alongside `Ș/Ț`.

## Run

Requires **.NET 8 SDK**.

```bash
dotnet run --project CaesarCipherLab.csproj
```

Or open `CaesarCipherLab.csproj` in Visual Studio and run the console project.

## Alphabet (Table 2)

`AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ` (31 letters).

Shifts are computed with indices in this exact string (not ASCII/Unicode numeric codes).

## Example

For task 1.1, `ABC` with shift 3 becomes **`BCD`**, because the Romanian alphabet places `Ă` and `Â` between `A` and `B`.

For task 1.2, the application prints the generated 31-letter alphabet before processing the message.

Note: the PDF's `CIFRULCEZAR` → `FLIUXOFHCDU` example is for the **26-letter English alphabet**, not this Romanian-alphabet assignment.
