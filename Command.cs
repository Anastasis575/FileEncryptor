using Encryptor;
using Password;
namespace Encryptor
{
    class Command
    {
        static void Main(string[] args)
        {
            PassWordManager manager;
            if (!Directory.Exists("data") || !File.Exists("./data/pass.txt"))
            {
                manager = new Password.PassWordManager();
                manager.addPass("W7p7LZzv9x4y2");
                manager.toFile();
            }
            else
            {
                manager = new Password.PassWordManager("./data/pass.txt");
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
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
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
                            output = args[i];
                        }
                        else if (previous == "-p" || previous == "--password")
                        {
                            pass = args[i];
                        }
                        else
                        {
                            path = args[i];
                        }
                        break;
                }
                previous = args[i];

            }

            if ((!isEncrypt && !isDecrypt) || string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Please choose the action you would like to use either encryption(-e/--encryption) or decryption(-d/--decryption) followed by path");
                Console.WriteLine("You can also specify the output file name using -o or --output followed by a name");
                return;
            }

            string? decr_key = null;
            if (string.IsNullOrEmpty(pass))
            {
                while (string.IsNullOrEmpty(pass))
                {
                    Console.WriteLine("Please input your password.");
                    pass = Console.ReadLine();
                    if (pass == null) continue;
                    decr_key = manager.Authenticate(pass);
                    if (decr_key != null) break;
                }
            }
            else
            {
                decr_key = manager.Authenticate(pass);
            }
            if (decr_key == null)
            {
                System.Console.WriteLine("Wrong Password. Try again!");
                return;
            };
            string file_data = Encryptor.FileEncryption.ReadFile(path);
            string result_string;
            if (isEncrypt)
            {
                result_string = Encryptor.FileEncryption.Encrypt(file_data, decr_key);
                if (string.IsNullOrEmpty(output))
                    output = "encrypted_file.txt";
            }
            else
            {
                result_string = Encryptor.FileEncryption.Decrypt(file_data, decr_key);
                if (string.IsNullOrEmpty(output))
                    output = "decrypted_file.txt";
            }
            if (!File.Exists(output))
            {
                output = Directory.GetCurrentDirectory() + "/" + output;
            }
            File.WriteAllText(output, result_string);
        }

    }

}