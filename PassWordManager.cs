using Newtonsoft.Json;
using System.Text;
namespace Password
{
    class PassWordManager
    {
        const string key="Yq3t6w9y$B&E)H@McQfTjWnZr4u7x!A%";

        private List<Password>? passwords{get;set;}

        public PassWordManager(){
            passwords=new List<Password>();
        }


        public void toFile(){
            string raw=JsonConvert.SerializeObject(passwords);
            string enc=Encryptor.FileEncryption.Encrypt(raw,key);
            if(!Directory.Exists("./data")){
                Directory.CreateDirectory("./data");
            }
            File.WriteAllText("./data/pass.txt",enc,Encoding.UTF8);
        }


        public PassWordManager(string path)
        {
            string raw=Encryptor.FileEncryption.ReadFile(path);
            string? data=null;
            try{
                data=Encryptor.FileEncryption.Decrypt(raw,key);            
            }catch (System.Exception e){
                Console.Error.WriteLine(e);
            }
            if (data!=null){
                passwords=JsonConvert.DeserializeObject<List<Password>>(data);
                if(passwords==null){
                    throw new Exception("Cannot Read Passwords");
                }
            }
        }



        public string? Authenticate(string pass){
            if(!this.containsPass(pass)) return null;
            return key;
        }


        public void addPass(string pass){
            if (this.passwords==null)return;
            if (!this.containsPass(pass))
                this.passwords.Add(new Password(pass));
        }

        public bool containsPass(string pass){
            if(this.passwords==null)return false;
            return this.passwords.FindAll(value=>value.password==pass).Any<Password>();
        }

        public void removePass(string pass){
            if(this.passwords==null)return;
            this.passwords.RemoveAll(value=>value.password==pass);
        }

    }

    public class Password{
        public string password{get; set;}

        public Password(string password)
        {
            this.password = password;
        }
    }
}