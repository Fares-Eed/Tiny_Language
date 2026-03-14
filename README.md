# Tiny PL - Phase 1: Lexical Analysis

## Overview
Lexical Analysis is the first phase of a compiler. 
Its role is to read the source code character by character, group characters into meaningful units called lexemes, and classify each lexeme into a token type based on predefined Regular Expressions. 
The Tiny PL Lexical Analysis phase defines 31 language constructs. 
During the scanning process, comments and whitespace are consumed and discarded. 
The Lexical Analyzer then passes the token stream to the Syntax Analyzer (Parser) as Phase 2 input.

## Implementation Strategy
The scanner is implemented as a C# Windows Forms application. 
It combines all terminal patterns into one master regular expression. 
The implementation uses `Regex.Matches()` to extract all tokens in a single pass, regardless of spacing. 
Extracted lexemes are classified by testing them against token-specific patterns in a strict priority order. 
Keywords and multi-character operators are checked first to prevent partial matching. 
Any character or sequence that does not match any predefined pattern is identified as a lexical error.

## User Interface & Testing
The application features a graphical user interface where input code is entered in a left text box. 
Upon pressing the "Split" button, the scanner tokenizes the source code. 
The output is displayed in a DataGridView on the right, showing each lexeme alongside its classified token type.

![Application Interface](assets/images/Screenshot%202026-03-14%20234144.png)

## Token Specifications
The scanner categorizes raw source text into the following token groups:

| Category | Token Types |
|---|---|
| Literals | NUMBER, STRING |
| Reserved Words | KEYWORD (13 keywords), DATATYPE |
| Names | IDENTIFIER, FUNC_NAME |
| Operators | ASSIGN_OP, ARITH_OP, COND_OP, BOOL_OP |
| Delimiters | SEMICOLON, LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE, COMMA |
| Special | COMMENT (discarded), UNKNOWN (error) |

## Team Members
* **Abdallah Ahmed Hamdy** (ID: 324250010, Section: 8)
* **Fares Mohamed Mohamed Eed** (ID: 324243714, Section: 10)
* **Fatma Bahaa Baddour** (ID: 324243085, Section: 10)

## Repository
GitHub Repo: [https://github.com/Fares-Eed/Tiny_Language](https://github.com/Fares-Eed/Tiny_Language)
