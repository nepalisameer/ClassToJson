// See https://aka.ms/new-console-template for more information
using ClassToJson;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("");
        Console.WriteLine("=========================================");
        Console.WriteLine("|             Class To Json             |");
        Console.WriteLine("=========================================");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("Paste the Class you want to convert to Json.");
        // Console.WriteLine("Press Enter then Use Ctrl + Z then again press enter");
        Console.WriteLine("Press Enter once or twice");
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("");
        string line;
        string xxx = string.Empty;
        do
        {
            //Check for exit conditions
            line = Console.ReadLine();
            xxx += line;

        } while (!string.IsNullOrWhiteSpace(line));

        //while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        //{
        //    xxx += line;
        //}
        // string classString = Console.In.ReadToEnd();
        ClassGenearator classGenearator = new ClassGenearator();
        if (string.IsNullOrEmpty(xxx))
        {
            Console.WriteLine("Invalid or empty class");
        }
        else
        {
            try
            {
                var obj = classGenearator.Genearator(xxx);
                if (obj != null)
                {
                    string jsonString = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
                    Console.WriteLine("");
                    Console.WriteLine("---------------------->>Output Json<<-----------------------");
                    Console.WriteLine(jsonString);
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("");
                    ClassGenearator.SetText(jsonString);
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("Json has been copied.");
                    Console.WriteLine("Use Ctrl + V or right click and paste wherever you want.");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Please provide the valid class");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Please provide the valid class");
            }
        }
        Console.WriteLine("");
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Convert Again? Y/N");
        var key = Console.ReadKey();
        if (key.KeyChar == 'Y' || key.KeyChar == 'y')
        {
            Console.Clear();
            Main(Array.Empty<string>());
        }
        else
        {
            Environment.Exit(0);
        }
    }
}