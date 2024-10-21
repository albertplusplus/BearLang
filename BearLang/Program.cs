// See https://aka.ms/new-console-template for more information

namespace BearLang;

class Bear
{
    static bool hadError = false;
    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
           Console.WriteLine("Usage: bear [script]");
           Environment.Exit(1);
        }
        if (args.Length == 1)
            runFile(args[0]);
        else
            runPrompt();

    }

    public static void runFile(string path)
    {
        var bytes = File.ReadAllText(path);
        run(new string(bytes));
    }

    private static void runPrompt()
    {
        for (;;)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                break;
            run(input);
        }
    }

    private static void run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.scanTokens();
        foreach (var token in tokens)
        {
            Console.WriteLine($"{token.type.ToString().PadRight(13,' ')} : {token.lexeme}");
        }
    }

    public static void error(int line, string message)
    {
        report(line, "", message);

    }

    private static void report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");

    }

}
