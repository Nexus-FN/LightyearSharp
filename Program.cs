using System.Diagnostics;
using Newtonsoft.Json;
using Spectre.Console;

namespace ChannelLauncher;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Momentum Launcher";
        
        AnsiConsole.MarkupLine("Welcome, " + Environment.UserName + ", to the [underline blue]Momentum launcher.[/]");
        AnsiConsole.MarkupLine("You can select an option using the arrow keys [underline blue]UP[/] and [underline blue]DOWN.[/]");
        
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MomentumLauncher";

        if (!Directory.Exists(appdata))
        {
            AnsiConsole.MarkupLine("[yellow]Appdata folder is missing, creating one for you...[/]");
            Directory.CreateDirectory(appdata);
        }

            var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[blue]What do you want to do[/]?")
                .PageSize(3)
                .AddChoices(new[]
                {
                    "Start game", "Change fortnite path", "Exit"
                }));

        switch (option)
        {
            case "Start game":

                if (!File.Exists(appdata + "\\path.txt"))
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]Fortnite path is missing, please set it first![/]");
                    Main(args);
                }

                if (!File.Exists(appdata + "\\email.txt") || !File.Exists(appdata + "\\password.txt"))
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]Please Enter Your Email and Password! (Account Made From Discord Bot.)[/]");

                    Console.Write("Email: ");
                    string email = Console.ReadLine();
                    File.WriteAllText(appdata + "\\email.txt", email);

                    Console.Write("Password: ");
                    string password = Console.ReadLine();
                    File.WriteAllText(appdata + "\\password.txt", password);

                    AnsiConsole.MarkupLine("[green]Email and Password Saved![/]");
                }
                
                if (!File.Exists("redirect.json"))
                {
                    string fileContent = "{ \"name\": \"Buzz.dll\", \"download\": \"https://cdn.nexusfn.net/file/2023/06/TV.dll\" }";
                    File.WriteAllText("redirect.json", fileContent);
                }

                string fileData = File.ReadAllText("redirect.json");

                var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileData);

                if (!jsonData.TryGetValue("name", out var dllNameObject) || !(dllNameObject is string dllName))
                {
                    throw new Exception("Invalid JSON structure: 'name' key is missing or not a string");
                }

                if (!jsonData.TryGetValue("download", out var dllDownloadObject) || !(dllDownloadObject is string dllDownload))
                {
                    throw new Exception("Invalid JSON structure: 'download' key is missing or not a string");
                }

                if (!dllName.EndsWith(".dll"))
                {
                    dllName += ".dll";
                }

                MomentumLauncher.Utilities.StartGame(File.ReadAllText(appdata + "\\path.txt"), dllDownload, dllName);
                AnsiConsole.MarkupLine("Starting the client");
                break;

            case "Change fortnite path":
                AnsiConsole.MarkupLine("Please enter the path to your fortnite folder");
                string setPath = AnsiConsole.Ask<string>("Path: ");
                string fortnitePath = Path.Combine(setPath, "FortniteGame", "Binaries", "Win64");

                if (Directory.Exists(fortnitePath))
                {
                    Console.WriteLine(fortnitePath);
                    File.WriteAllText(appdata + "\\path.txt", setPath);
                    Console.Clear();
                    AnsiConsole.MarkupLine("Path changed, you can now start the game");
                    Main(args);
                }
                else
                {
                    Console.WriteLine(fortnitePath);
                    AnsiConsole.MarkupLine("[red]Path is invalid![/]");
                    Main(args);
                }
                break;
            case "Exit":
                Environment.Exit(0);
                break;
        }


        Console.ReadKey(true);
    }
}