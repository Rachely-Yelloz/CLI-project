using System.CommandLine;
using System.Transactions;
var bundleCommand = new Command("bundle", "bundle code");
var create_rspCommand = new Command("create-rsp", "create rsp file");



var outputOption = new Option<FileInfo>(new[] { "--output", "-o" }, "The name of the file");
var languageOption = new Option<string[]>("--language", "The language to use");
languageOption.IsRequired = true;
var noteOption = new Option<bool>(new[] { "--note", "-n" }, "write the sorce?");
var sortOption = new Option<string>(new[] { "--sort", "-s" }, getDefaultValue: () => "abc", "which sort?");
var remove_empty_lines_Option = new Option<bool>(new[] { "--remove-empty-lines", "-r" }, "remove empty?");
var authorOption = new Option<string>(new[] { "--author", "-a" }, "enter author");

bundleCommand.AddOption(outputOption);
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(remove_empty_lines_Option);
bundleCommand.AddOption(authorOption);

create_rspCommand.AddOption(outputOption);
bundleCommand.SetHandler((output,lunguage,  note, sort, remove, auther) =>
{
    string[] filesNames = Directory.GetFiles(Directory.GetCurrentDirectory());
    if (lunguage[0] != "all")
    {
        List<string> res = new List<string>();
        for (int i = 0; i < filesNames.Length; i++)
        {
            switch (lunguage[i])
            {
                case "c#" or "C#":
                    if (filesNames[i].Contains(".cs"))
                    {
                        res.AddRange(filesNames.Where(x => x.Contains(".cs")));
                    }
                    break;
                case "java" or "Java":
                    if (filesNames[i].Contains(".java"))
                    {
                        res.AddRange(filesNames.Where(x => x.Contains(".java")));
                    }
                    break;
                case "python" or "Python":
                    if (filesNames[i].Contains(".py"))
                    {
                        res.AddRange(filesNames.Where(x => x.Contains(".py")));
                    }
                    break;
                case "javascript" or "Javascript":
                    if (filesNames[i].Contains(".js"))
                    {
                        res.AddRange(filesNames.Where(x => x.Contains(".js")));
                    }
                    break;
                case "html" or "Html":
                    if (filesNames[i].Contains(".html"))
                    {
                        res.AddRange(filesNames.Where(x => x.Contains(".html")));
                    }
                    break;
                case "css" or "Css":
                    if (filesNames[i].Contains(".css"))
                    {
                        res.AddRange(filesNames.Where(x => x.Contains(".css")));
                    }
                    break;
                default:
                    Console.WriteLine("not recognize language " + lunguage[i]);
                    break;
            }
        }
        filesNames = res.ToArray();

    }

    if (sort == "abc")
    {
        filesNames = filesNames.OrderBy(f => Path.GetFileName(f)).ToArray();
    }
    if (sort == "lunguage")
    {
        filesNames = filesNames.OrderBy(f => Path.GetExtension(f)).ToArray();
    }
    using (var outputFile = File.CreateText(output.FullName))
    {
        if (auther != null)
        {
            outputFile.WriteLine("author: " + auther);
        }
        string[] cointeins;
        for (int i = 0; i < filesNames.Length; i++)
        {
            if (note )
            {
                outputFile.WriteLine("file name: " + Path.GetFileName(filesNames[i]));
                outputFile.WriteLine("file path: " + filesNames[i]);
                outputFile.WriteLine("file content: ");
            }
            cointeins = File.ReadAllLines(filesNames[i]);
            if (remove)
            {
                cointeins = cointeins.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            }
            outputFile.WriteLine(string.Join("\n", cointeins));

        }


    }
}, outputOption,languageOption,  noteOption, sortOption, remove_empty_lines_Option, authorOption);
var rootCommand = new RootCommand("root comand");
rootCommand.AddCommand(bundleCommand);
create_rspCommand.SetHandler(() =>
{
    var responseFile = new FileInfo("response.rsp");
    Console.WriteLine("enter values to the bundle command:");
    using (var outputFile = File.CreateText("response.rsp"))
    {
        Console.Write("Output file path: ");
        var Output = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(Output))
        {
            Console.Write("Enter the output file path: ");
            Output = Console.ReadLine();
        }
        outputFile.WriteLine("--output " + Output);
        Console.Write("Enter the language: ");
        Console.Write("Languages (comma-separated): ");
        var languages = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(languages))
        {
            Console.Write("Please enter at least one programming language: ");
            languages = Console.ReadLine();
        }
        outputFile.WriteLine("--language " + languages);
        Console.Write("Include notes? (y/n): ");
        var note = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(note) || (note != "y" && note != "n"))
        {
            Console.Write("Please enter 'y' or 'n': ");
            note = Console.ReadLine();
        }
        if (note == "y")
        {
            outputFile.WriteLine("--note");
        }
        Console.Write("Sort by (abc/language): ");
        var sort = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(sort) || (sort != "abc" && sort != "language"))
        {
            Console.Write("Please enter 'abc' or 'language': ");
            sort = Console.ReadLine();
        }
        outputFile.WriteLine("--sort " + sort);
        Console.Write("Remove empty lines? (y/n): ");
        var remove = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(remove) || (remove != "y" && remove != "n"))
        {
            Console.Write("Please enter 'y' or 'n': ");
            remove = Console.ReadLine();
        }
        if (remove == "y")
        {
            outputFile.WriteLine("--remove-empty-lines");
        }
        Console.Write("Author: ");
        var author = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(author))
        {
            outputFile.WriteLine("--author " + author);
        }
        Console.WriteLine("Response file created successfully: " + responseFile.FullName);

    }
});
rootCommand.AddCommand(create_rspCommand);

rootCommand.InvokeAsync(args);
