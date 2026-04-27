using System;
using System.Collections.Generic;

public class TinyPLParser
{
    private List<Token> tokens;
    private int index = 0;
    private Token currentToken;

    public TinyPLParser(List<Token> tokens)
    {
        this.tokens = tokens;
        if (tokens.Count > 0)
            currentToken = tokens[index];
    }

    private void Match(string expected)
    {
        if (index < tokens.Count && (currentToken.Value == expected || currentToken.Type == expected))
        {
            index++;
            if (index < tokens.Count)
                currentToken = tokens[index];
        }
        else
        {
            throw new Exception($"Syntax Error: Expected '{expected}' but found '{currentToken?.Value}' at index {index}");
        }
    }

    private Token PeekNextToken()
    {
        if (index + 1 < tokens.Count) return tokens[index + 1];
        return null;
    }

    private bool IsDatatype()
    {
        return currentToken?.Value == "int" || currentToken?.Value == "float" || currentToken?.Value == "string";
    }

    public void ParseProgram()
    {
        while (index < tokens.Count && IsDatatype() && PeekNextToken()?.Value != "main")
        {
            ParseFunctionStatement();
        }
        ParseMainFunction();
    }

    private void ParseFunctionStatement()
    {
        ParseFunctionDeclaration();
        ParseFunctionBody();
    }

    private void ParseFunctionDeclaration()
    {
        ParseDatatype();
        Match("Identifier");
        Match("(");

        if (currentToken.Value != ")")
        {
            ParseDatatype();
            Match("Identifier");
            while (currentToken.Value == ",")
            {
                Match(",");
                ParseDatatype();
                Match("Identifier");
            }
        }
        Match(")");
    }

    private void ParseMainFunction()
    {
        ParseDatatype();
        Match("main");
        Match("(");
        Match(")");
        ParseFunctionBody();
    }

    private void ParseFunctionBody()
    {
        Match("{");
        while (index < tokens.Count && currentToken.Value != "return" && currentToken.Value != "}")
        {
            ParseStatement();
        }
        ParseReturnStatement();
        Match("}");
    }

    private void ParseStatement()
    {
        if (IsDatatype())
        {
            ParseDeclarationStatement();
        }
        else if (currentToken.Type == "Identifier")
        {
            ParseAssignmentStatement();
        }
        else if (currentToken.Value == "read")
        {
            ParseReadStatement();
        }
        else if (currentToken.Value == "write")
        {
            ParseWriteStatement();
        }
        else if (currentToken.Value == "if")
        {
            ParseIfStatement();
        }
        else if (currentToken.Value == "repeat")
        {
            ParseRepeatStatement();
        }
        else
        {
            throw new Exception($"Syntax Error: Unexpected statement starts with '{currentToken?.Value}'");
        }
    }

    private void ParseDatatype()
    {
        if (IsDatatype())
            Match(currentToken.Value);
        else
            throw new Exception($"Syntax Error: Expected Datatype (int, float, string) but found '{currentToken?.Value}'");
    }

    private void ParseDeclarationStatement()
    {
        ParseDatatype();
        ParseIdentifierOrAssignment();
        while (currentToken.Value == ",")
        {
            Match(",");
            ParseIdentifierOrAssignment();
        }
        Match(";");
    }

    private void ParseIdentifierOrAssignment()
    {
        Match("Identifier");
        if (currentToken.Value == ":=")
        {
            Match(":=");
            ParseExpression();
        }
    }

    private void ParseAssignmentStatement()
    {
        Match("Identifier");
        Match(":=");
        ParseExpression();
        if (currentToken.Value == ";") Match(";");
    }

    private void ParseReadStatement()
    {
        Match("read");
        Match("Identifier");
        Match(";");
    }

    private void ParseWriteStatement()
    {
        Match("write");
        if (currentToken.Value == "endl")
        {
            Match("endl");
        }
        else
        {
            ParseExpression();
        }
        Match(";");
    }

    private void ParseReturnStatement()
    {
        Match("return");
        ParseExpression();
        if (currentToken.Value == ";") Match(";");
    }

    private void ParseIfStatement()
    {
        Match("if");
        ParseConditionStatement();
        Match("then");

        while (currentToken.Value != "elseif" && currentToken.Value != "else" && currentToken.Value != "end")
        {
            ParseStatement();
        }

        if (currentToken.Value == "elseif")
        {
            ParseElseIfStatement();
        }
        else if (currentToken.Value == "else")
        {
            Match("else");
            while (currentToken.Value != "end")
            {
                ParseStatement();
            }
            Match("end");
        }
        else
        {
            Match("end");
        }
    }

    private void ParseElseIfStatement()
    {
        Match("elseif");
        ParseConditionStatement();
        Match("then");

        while (currentToken.Value != "elseif" && currentToken.Value != "else" && currentToken.Value != "end")
        {
            ParseStatement();
        }

        if (currentToken.Value == "elseif") ParseElseIfStatement();
        else if (currentToken.Value == "else")
        {
            Match("else");
            while (currentToken.Value != "end") ParseStatement();
            Match("end");
        }
        else Match("end");
    }

    private void ParseRepeatStatement()
    {
        Match("repeat");
        while (currentToken.Value != "until")
        {
            ParseStatement();
        }
        Match("until");
        ParseConditionStatement();
    }

    private void ParseConditionStatement()
    {
        ParseCondition();
        while (currentToken.Value == "&&" || currentToken.Value == "||")
        {
            Match(currentToken.Value);
            ParseCondition();
        }
    }

    private void ParseCondition()
    {
        Match("Identifier");

        if (currentToken.Value == "<" || currentToken.Value == ">" || currentToken.Value == "=" || currentToken.Value == "<>")
            Match(currentToken.Value);
        else
            throw new Exception($"Expected Condition Operator (<, >, =, <>) but got {currentToken.Value}");

        ParseTerm();
    }

    private void ParseExpression()
    {
        if (currentToken.Type == "String" || currentToken.Value.StartsWith("\""))
        {
            Match(currentToken.Type == "String" ? "String" : currentToken.Value);
        }
        else
        {
            ParseTerm();
            while (currentToken.Value == "+" || currentToken.Value == "-" || currentToken.Value == "*" || currentToken.Value == "/")
            {
                Match(currentToken.Value);
                ParseTerm();
            }
        }
    }

    private void ParseTerm()
    {
        if (currentToken.Type == "Number")
        {
            Match("Number");
        }
        else if (currentToken.Type == "Identifier")
        {
            Match("Identifier");

            if (currentToken.Value == "(")
            {
                Match("(");
                if (currentToken.Value != ")")
                {
                    ParseTerm();
                    while (currentToken.Value == ",")
                    {
                        Match(",");
                        ParseTerm();
                    }
                }
                Match(")");
            }
        }
        else if (currentToken.Value == "(")
        {
            Match("(");
            ParseExpression();
            Match(")");
        }
        else
        {
            throw new Exception($"Expected Term (Number, Identifier) but found {currentToken.Value}");
        }
    }
}