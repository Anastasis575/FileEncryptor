namespace FileEncryptor
{
    class Command
    {
        static void Main(string[] args)
        {
            PassWordManager manager;
            if (!Directory.Exists("data") || !File.Exists("./data/pass.txt"))
            {
                manager = new PassWordManager();
                manager.AddPass("W7p7LZzv9x4y2");
                manager.ToFile();
            }
            else
            {
                manager = new PassWordManager("./data/pass.txt");
            }

            bool isEncrypt = false, isDecrypt = false;
            string? path = null;
            string? pass = null;
            string? output = null;
            string? previous = null;
            if (args.Length < 2)
            {
                Console.WriteLine("Please choose the action you would like to use either encryption(-e/--encryption) or decryption(-d/--decryption) followed by path");
                return;
            }
            foreach (var t in args)
            {
                switch (t)
                {
                    case "--encrypt":
                    case "-e":
                        isEncrypt = true;
                        break;
                    case "--decrypt":
                    case "-d":
                        isDecrypt = true;
                        break;
                    case "--output":
                    case "-o":
                    case "-p":
                    case "--password":
                        break;
                    default:
                        if (previous == "-o" || previous == "--output")
                        {
                            output = t;
                        }
                        else if (previous == "-p" || previous == "--password")
                        {
                            pass = t;
                        }
                        else
                        {
                            path = t;
                        }
                        break;
                }
                previous = t;
            }

            if ((!isEncrypt && !isDecrypt) || string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Please choose the action you would like to use either encryption(-e/--encryption) or decryption(-d/--decryption) followed by path");
                Console.WriteLine("You can also specify the output file name using -o or --output followed by a name");
                return;
            }

            string? decrΚey = null;
            if (string.IsNullOrEmpty(pass))
            {
                while (string.IsNullOrEmpty(pass))
                {
                    Console.WriteLine("Please input your password.");
                    pass = Console.ReadLine();
                    if (pass == null) continue;
                    decrΚey = manager.Authenticate(pass);
                    if (decrΚey != null) break;
                }
            }
            else
            {
                decrΚey = manager.Authenticate(pass);
            }
            if (decrΚey == null)
            {
                Console.WriteLine("Wrong Password. Try again!");
                return;
            };
            string fileData = FileEncryption.ReadFile(path);
            string resultString;
            if (isEncrypt)
            {
                resultString = FileEncryption.Encrypt(fileData, decrΚey);
                if (string.IsNullOrEmpty(output))
                    output = "encrypted_file.txt";
            }

            if (isDecrypt)
            {

            }
            {
                resultString = FileEncryption.Decrypt(fileData, decrΚey);
                if (string.IsNullOrEmpty(output))
                    output = "decrypted_file.txt";
            }
            if (!File.Exists(output))
            {
                output = Directory.GetCurrentDirectory() + "/" + output;
            }
            File.WriteAllText(output, resultString);
        }

    }

}