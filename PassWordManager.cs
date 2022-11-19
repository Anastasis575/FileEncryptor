using Newtonsoft.Json;
using System.Text;
namespace FileEncryptor
{
    class PassWordManager
    {
        const string Key="Yq3t6w9y$B&E)H@McQfTjWnZr4u7x!A%";

        private List<string>? Passwords{get;set;}

        public PassWordManager(){
            Passwords=new List<string>();
        }


        public void ToFile(){
            string raw=JsonConvert.SerializeObject(Passwords);
            string enc=FileEncryption.Encrypt(raw,Key);
            if(!Directory.Exists("./data")){
                Directory.CreateDirectory("./data");
            }
            File.WriteAllText("./data/pass.txt",enc,Encoding.UTF8);
        }


        public PassWordManager(string path)
        {
            string raw=FileEncryption.ReadFile(path);
            string? data=null;
            try{
                data=FileEncryption.Decrypt(raw,Key);
            }catch (Exception e){
                Console.Error.WriteLine(e);
            }
            if (data!=null){
                Passwords=JsonConvert.DeserializeObject<List<string>>(data);
                if(Passwords==null){
                    throw new Exception("Cannot Read Passwords");
                }
            }
        }



        public string? Authenticate(string pass){
            if(!this.ContainsPass(pass)) return null;
            return Key;
        }


        public void AddPass(string pass){
            if (this.Passwords==null)return;
            if (!this.ContainsPass(pass))
                this.Passwords.Add(pass);
        }

        public bool ContainsPass(string pass){
            if(this.Passwords==null)return false;
            return this.Passwords.FindAll(value=>value==pass).Any();
        }

        public void RemovePass(string pass){
            if(this.Passwords==null)return;
            this.Passwords.RemoveAll(value=>value==pass);
        }

    }
}