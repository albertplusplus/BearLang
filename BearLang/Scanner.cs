
namespace BearLang;

class Scanner
{
    string source;
    List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
    {
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for",   TokenType.FOR},
        {"fun",   TokenType.FUN},
        {"if",    TokenType.IF},
        {"nil",   TokenType.NIL},
        {"or",    TokenType.OR},
        {"print", TokenType.PRINT},
        {"return",TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this",  TokenType.THIS},
        {"true",  TokenType.TRUE},
        {"var",   TokenType.VAR},
        {"while", TokenType.WHILE},
    };

    public Scanner(string source)
    {
        this.source = source;
    }

    public List<Token> scanTokens()
    {
        while (!isAtEnd())
        {
            start = current;
            scanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private void scanToken()
    {
        char c = advance();
        switch(c)
        {
            case '(' : addToken(TokenType.LEFT_PAREN); break;
            case ')' : addToken(TokenType.RIGHT_PAREN); break;
            case '{' : addToken(TokenType.LEFT_BRACE); break;
            case '}' : addToken(TokenType.RIGHT_BRACE); break;
            case ',' : addToken(TokenType.COMMA); break;
            case '.' : addToken(TokenType.DOT); break;
            case '-' : addToken(TokenType.MINUS); break;
            case '+' : addToken(TokenType.PLUS); break;
            case ';' : addToken(TokenType.SEMICOLON); break;
            case '*' : addToken(TokenType.STAR); break;
            case '!' : addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=' : addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<' : addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>' : addToken(match('>') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '/' : handleSlash(); break;
            case ' ' : break;
            case '\r': break;
            case '\t': break;
            case '\n': line++; break;
            case '"' : collectString(); break;
            default:
                if (char.IsDigit(c))
                    number();
                else if (char.IsLetter(c))
                    identifier();
                else
                    Bear.error(line, "Unexpected character."); break;
        };
    }

    private void collectString()
    {
        while (peek() != '"' && !isAtEnd())
        {
            if (peek() == '\n') line++;
            advance();
        }

        if (isAtEnd())
        {
            Bear.error(line, "Unterminated string");
            return;
        }

        advance();

        string value = source.Substring(start+1, current - start - 1);
        addToken(TokenType.STRING, value);
    }

    private void identifier()
    {
        while (char.IsLetterOrDigit(peek()) || peek() == '_') advance();

        string text = source.Substring(start, current-start);
        TokenType type;
        if (!keywords.ContainsKey(text))
            type = TokenType.IDENTIFIER;
        else
            type = keywords[text];
        addToken(type);
    }

    private void number()
    {
        while (char.IsDigit(peek())) advance();

        if (peek() == '.' && char.IsDigit(peekNext()))
        {
            advance();
            while (char.IsDigit(peek())) advance();
        }
        addToken(TokenType.NUMBER, float.Parse(source.Substring(start, current-start)));
    }

    private char peekNext()
    {
        if (current + 1 > source.Length) return '\0';
        return source[current+1];
    }

    private void handleSlash()
    {
        if (match('/'))
            while (peek() != '\n' && !isAtEnd()) advance();
        else addToken(TokenType.SLASH);
    }

    private char peek()
    {
        if (isAtEnd()) return '\0';
        return source[current];
    }

    private bool match(char expected)
    {
        if (isAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private void addToken(TokenType type) => addToken(type, null);

    private void addToken(TokenType type, object literal)
    {
        string text = source.Substring(start, current-start);
        tokens.Add(new Token(type, text, literal, line));
    }


    private char advance() => source[current++];

    private bool isAtEnd() => current >= source.Length;
}
