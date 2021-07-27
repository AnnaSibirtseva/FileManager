using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    /// <summary>
    /// Output the result of all other methods.
    /// </summary>
    static void Main()
    {
        MainMenu();
        string path = "";
        string query;
        string buff = "";
        List<string> saved_path = new List<string>();
        while (true)
        {
            Console.Write(path + ">");
            query = Console.ReadLine();
            if (!ValidateQuery(query, out string[] queryParameters))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("INVALID REQUEST");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                ProcessQuery(queryParameters, ref path, ref buff, ref saved_path);
            }
        }
    }

    /// <summary>
    /// This method checks userы input.
    /// </summary>
    /// <param name="query">Current query in string format.</param>
    /// <param name="queryParameters">Current query.</param>
    /// <returns>True if operation exists, flalse otherwise./returns>
    public static bool ValidateQuery(string query, out string[] queryParameters)
    {
        queryParameters = query.Split(' ');
        string[] opers = { "back", "help", "to", "sd", "sf", "pick", "copy", "paste", "delete", "print", "create", "glue", "sfc", "exit" };
        // If the command entered by the user was not in the array, then we give an error.
        if (!opers.Contains(queryParameters[0]))
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// With a user-specified command, select what the program should output.
    /// </summary>
    /// <param name = "queryParameters">Current query.</param>
    /// <param name = "path">Path to sth.</param>
    /// <param buffer ="buff">Last saved thing.</param>
    public static void ProcessQuery(string[] queryParameters, ref string path, ref string buff, ref List<string> saved_paths)
    {
        string comand = queryParameters[0];
        try
        {
            switch (comand)
            {
                case "back":
                    Back(ref path);
                    break;
                case "to":
                    ForwardTo(queryParameters[1], ref path);
                    break;
                case "help":
                    Help();
                    break;
                case "sd":
                    ShowContent(ref path);
                    break;
                case "sf":
                    ShowFiles(ref path);
                    break;
                case "pick":
                    PickFile(queryParameters[1], ref path);
                    break;
                case "copy":
                    Copy(ref path, ref buff);
                    break;
                case "exit":
                    Console.WriteLine("Bye! Have a nice day!");
                    Environment.Exit(0);
                    break;
                case "paste":
                    Paste(ref path, ref buff);
                    break;
                case "delete":
                    Delete(ref path);
                    break;
                case "print":
                    Print(ref path);
                    break;
                case "create":
                    Create(ref path);
                    break;
                case "sfc":
                    SafeForConcatenation(ref path, ref saved_paths);
                    break;
                case "glue":
                    Glue(ref path, ref saved_paths);
                    break;
            }
        }
        // Print an error if Exeption gets out.
        catch
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*ERROR*");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    /// <summary>
    /// To work with the file, the user needs to select it.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="path">Current path.</param>
    public static void PickFile(string fileName, ref string path)
    {

            // We select the required file if it does not exist then we give an error. 
            string ap_path = Path.Combine(path, fileName);
            if (!File.Exists(ap_path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("THE FILE DOESN'T EXIST");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            path = ap_path;

    }

    /// <summary>
    /// Output the selected file to the console.
    /// </summary>
    /// <param name="path">Current path.</param>
    public static void Print(ref string path)
    {
        // If no file is selected - error.
        if (!File.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("THE FILE ISN'T SELECTED");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        // Choosing the encoding.
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Select a code to encode the file (ASCII, UTF-8, Unicode, UTF-32):");
        Console.ForegroundColor = ConsoleColor.White;
        // Translate the file into the selected encoding.
        string encode = Console.ReadLine();
        string[] encodes = { "ASCII", "UTF-8", "Unicode", "UTF-32" };
        if (encodes.Contains(encode))
        {
            string text = File.ReadAllText(@path, Encoding.GetEncoding(encode));
            Console.WriteLine(text);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("INCORRECT ENCODING");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
    }
    /// <summary>
    /// If the user wants to move the file after copying he must paste it.
    /// </summary>
    /// <param name="path">Current path.</param>
    /// <param name="buff">Last saved thing.</param>
    public static void Paste(ref string path, ref string buff)
    {
        // Check that the buffer and directory are not empty.
        if (!Directory.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("THIS ISN'T A DIRECTORY");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        if (buff == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("BUFFER IS EMPTY");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        File.Copy(buff, Path.Combine(path, Path.GetFileName(buff)));
    }
    /// <summary>
    /// Creates files in the encoding selected by the user. 
    /// </summary>
    public static void Create(ref string path)
    {
        if (!Directory.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("THIS ISN'T A DIRECTORY");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Write name for the file:");
        Console.ForegroundColor = ConsoleColor.White;
        string name = Path.DirectorySeparatorChar + Console.ReadLine() + ".txt";
        path = path + name;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Write something to fill out the file (write empty string to stop filling the file):");
        Console.ForegroundColor = ConsoleColor.White;
        string text = "";
        string newtext = "";
        do
        {
            newtext = Console.ReadLine();
            text += newtext;
        }while (newtext != "");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Select a code to encode the file (ASCII, UTF-8, Unicode, UTF-32):");
        Console.ForegroundColor = ConsoleColor.White;
        string encode = Console.ReadLine();
        string[] encodes = { "ASCII", "UTF-8", "Unicode", "UTF-32" };
        if (encodes.Contains(encode))
        {
            File.AppendAllText(@path, text, Encoding.GetEncoding(encode));
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("INCORRECT ENCODING");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }  

    }
    /// <summary>
    /// Show the window with tips if the user is confused.
    /// </summary>
    public static void Help()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("***HELPER***");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("back - Goes back");
        Console.WriteLine("help - Shows this window");
        Console.WriteLine("to - Transits to another directory");
        Console.WriteLine("sd - Shows directions you can go to");
        Console.WriteLine("sf - Shows files");
        Console.WriteLine("pick - Choosing a file");
        Console.WriteLine("copy - Copies file to buffer");
        Console.WriteLine("paste - Pastes file, that was saved in buffer");
        Console.WriteLine("delete - Deletes files");
        Console.WriteLine("print - Prints files to console");
        Console.WriteLine("sfc - Saves file for concatenation");
        Console.WriteLine("glue = Concatenatinates files");
        Console.WriteLine("exit - Exits program");
    }
    /// <summary>
    /// Shows the main menu in the beginning of the programm.
    /// </summary>
    public static void MainMenu()
    {
        Console.WriteLine("Hello and welcome, my dear friend!");
        Console.WriteLine("I'm your little helper - 'File manager'");
        Console.WriteLine("Here is a brief instruction for my program so that you don't get lost in it");
        Console.WriteLine("If you want more detailed instructions in Russian, it is attached in the format .txt in the program folder");
        Console.WriteLine("");
        Console.WriteLine("  help - Displays a shortened version for all functions ");
        Console.WriteLine("  back - To go back one directory");
        Console.WriteLine("  to - To go to the next directory");
        Console.WriteLine("  sd - Displays a list of your computer's disks");
        Console.WriteLine("  sf - Displays a list of files in the directory");
        Console.WriteLine("  pick - By selecting a file, you can work with it");
        Console.WriteLine("  copy - Copies the selected file in the buffer");
        Console.WriteLine("  paste - Pastes the file you copied to a new directory");
        Console.WriteLine("  delete - Deletes the file you selected");
        Console.WriteLine("  print - Tells you to select the encoding, and then outputs the text of the selected file to the console");
        Console.WriteLine("  create - Creates a file after you select the encoding");
        Console.WriteLine("  sfc - Saves files for concatenation/glue");
        Console.WriteLine("  glue - Connects objects in 'sfc' and outputs them to the console");
        Console.WriteLine("  exit - Ends the program");
        Console.WriteLine("");
    }
    /// <summary>
    /// Goes to the next directory.
    /// </summary>
    /// <param name="direct">Direction to save sth.</param>
    /// <param name="path">Current path to do sth.</param>
    public static void ForwardTo(string direct, ref string path)
    {

            // Checking that this directory can be navigated to.
            if (!ValidateDirect(direct))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("INVALID DIRECTORY");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            string ap_path = Path.Combine(path, direct);
            if (!Directory.Exists(ap_path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("THERE IS NO SUCH DIRECTORY");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            path = ap_path;

    }
    /// <summary>
    /// Delete the selected file.
    /// </summary>
    /// <param name="path">Current path to do sth.</param>
    public static void Delete(ref string path)
    {
        if (!File.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("THE FILE ISN'T SELECTED");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        File.Delete(path);
    }
    /// <summary>
    /// Copy the selected file to the buffer.
    /// </summary>
    /// <param name="path">Current path to do sth.</param>
    /// <param name="buff">Last saved thing.</param>
    public static void Copy(ref string path, ref string buff)
    {
        if (!File.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("THE FILE ISN'T SELECTED");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        buff = path;
    }
    /// <summary>
    /// Check that the input doesn't contain illegal chars.
    /// </summary>
    /// <param name="direct">Direction to save sth.</param>
    /// <returns>Checks that direction is correct.</returns>
    public static bool ValidateDirect(string direct)
    {
        // If the input contains illegal characters, we output an error.
        char[] invalidPathChars = Path.GetInvalidPathChars();
        foreach (char elem in direct)
        {
            if (Array.IndexOf(invalidPathChars, elem) != -1)
            {
                return false;
            }
        }
        return true;

    }
    /// <summary>
    /// Go back one directory.
    /// </summary>
    /// <param name="path">Current path.</param>
    public static void Back(ref string path)
    {
        // If the user is already in the root folder, we issue an error.
        if (path == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("YOU CAN'T GO BACK");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        path = Convert.ToString(Directory.GetParent(path));
    }
    /// <summary>
    /// Safes files for future concatination
    /// </summary>
    /// <param name="">Current path.</param>
    /// <param name="">List of saved paths.</param>
    public static void SafeForConcatenation(ref string path, ref List<string> saved_paths)
    {
        if (!File.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FILE ISN'T SELECTED");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        saved_paths.Add(path);
    }
    /// <summary>
    /// Concatenate files.
    /// </summary>
    /// <param name="path">Current path.</param>
    public static void Glue(ref string path, ref List<string> saved_paths)
    {
        // If user didn't save anything - throw an exception.
        if (saved_paths.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("YOU HAVEN'T CHOOSE ANYTHING!");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        foreach (string elem in saved_paths)
        {
            Console.WriteLine(File.ReadAllText(elem));
        }
    }
    /// <summary>
    /// Show the eady-to-use disks in the directory.
    /// </summary>
    /// <param name="path">Current path.</param>
    public static void ShowContent(ref string path)
    {
        // Display ready-to-use disks.
        string[] subThings;
        if (path == "")
        {
            subThings = Environment.GetLogicalDrives();
        }
        else
        {
            subThings = Directory.GetDirectories(path);
        }
        foreach (var elem in subThings)
        {
            Console.WriteLine(elem);
        }
    }
    /// <summary>
    /// Shows all folders and files in the directory.
    /// </summary>
    /// <param name="path">Current path.</param>
    public static void ShowFiles(ref string path)
    {
        // If the folder is empty, throw an error.
        if (path == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NO FILES");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        string[] files = Directory.GetFiles(path);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("***List of files in the directory***");
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var file in files)
        {
            Console.WriteLine(Path.GetFileName(file));
        }
    }
}

