using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Spectre.Console;

namespace MomentumLauncher;

public class Utilities
{

    public static void DownloadFile(string Url, string Path) => new WebClient().DownloadFile(Url, Path);

    public static async void StartGame(string gamePath, string dllDownload, string dllName)
    {
        AnsiConsole.MarkupLine("[blue]Starting game...[/]");

        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MomentumLauncher";

        if (!File.Exists(appdata + "\\FortniteClient-Win64-Shipping_BE.exe"))
                DownloadFile("https://cdn.discordapp.com/attachments/958139296936783892/1000707724507623424/FortniteClient-Win64-Shipping_BE.exe", appdata + "\\FortniteClient-Win64-Shipping_BE.exe");
        if (!File.Exists(appdata + "\\FortniteLauncher.exe"))
                DownloadFile("https://cdn.discordapp.com/attachments/958139296936783892/1000707724818006046/FortniteLauncher.exe", appdata + "\\FortniteLauncher.exe");
        if (!File.Exists(appdata + "\\" + dllName))
                DownloadFile(dllDownload, appdata + "\\" + dllName);

        string emailPath = Path.Combine(appdata, "email.txt");
        string passwordPath = Path.Combine(appdata, "password.txt");
        
        string email = File.ReadAllText(emailPath);
        string password = File.ReadAllText(passwordPath);
        
        Process launcher = new Process();
        launcher.StartInfo.FileName = appdata + "\\FortniteLauncher.exe";
        
        Process shippingbe = new Process();
        shippingbe.StartInfo.FileName = appdata + "\\FortniteClient-Win64-Shipping_BE.exe";
        
        Process shipping = new Process();
        shipping.StartInfo.FileName = gamePath + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe";
        shipping.StartInfo.Arguments =
            $"-log -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fromfl=eac -fltoken=3db3ba5dcbd2e16703f3978d -nosplash -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiYmU5ZGE1YzJmYmVhNDQwN2IyZjQwZWJhYWQ4NTlhZDQiLCJnZW5lcmF0ZWQiOjE2Mzg3MTcyNzgsImNhbGRlcmFHdWlkIjoiMzgxMGI4NjMtMmE2NS00NDU3LTliNTgtNGRhYjNiNDgyYTg2IiwiYWNQcm92aWRlciI6IkVhc3lBbnRpQ2hlYXQiLCJub3RlcyI6IiIsImZhbGxiYWNrIjpmYWxzZX0.VAWQB67RTxhiWOxx7DBjnzDnXyyEnX7OljJm-j2d88G_WgwQ9wrE6lwMEHZHjBd1ISJdUO1UVUqkfLdU5nofBQ -AUTH_LOGIN={email} -AUTH_PASSWORD={password} -AUTH_TYPE=epic";
        shipping.StartInfo.UseShellExecute = false;
        shipping.StartInfo.RedirectStandardOutput = true;
        shipping.StartInfo.RedirectStandardError = true;
        
        launcher.Start();
        shippingbe.Start();
        shipping.Start();
        
        MomentumLauncher.Injector.Inject(shipping.Id, appdata + "\\" + dllName);
        Environment.Exit(0);


    }
    
}