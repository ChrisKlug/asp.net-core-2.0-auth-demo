using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Web.Services
{
    public class DummyUserService : IUserService
    {
        private IDictionary<string, (User User, string Password)> _users = new Dictionary<string, (User User, string Password)>();
        private IDictionary<string, User> _externalUsers = new Dictionary<string, User>();

        public DummyUserService()
        {
            InitializeStorage();
        }

        public Task<User> Add(string name, string email, string password)
        {
            if (_users.ContainsKey(name.ToLower()))
            {
                throw new InvalidOperationException("Username already in use");
            }

            var user = new User(name, email);
            _users.Add(email.ToLower(), (User: user, Password: HashString(password)));
            PersistData();
            return Task.FromResult(user);
        }
        public Task<User> Authenticate(string name, string password)
        {
            if (_users.ContainsKey(name.ToLower()))
            {
                if (_users[name.ToLower()].Password == HashString(password))
                {
                    return Task.FromResult(_users[name.ToLower()].User);
                }
            }
            return null;
        }

        public Task<User> AddExternal(string id, string name, string email)
        {
            if (_users.ContainsKey(id))
            {
                throw new InvalidOperationException("Id already in use");
            }

            var user = new User(name, email);
            _externalUsers.Add(id, user);
            PersistData();
            return Task.FromResult(user);
        }
        public Task<User> AuthenticateExternal(string id)
        {
            if (_externalUsers.ContainsKey(id))
            {
                return Task.FromResult(_externalUsers[id]);
            }
            return Task.FromResult<User>(null);
        }

        private void InitializeStorage()
        {
            if (File.Exists("storage.json"))
            {
                var str = File.ReadAllText("storage.json");
                var data = JObject.Parse(str);
                _users = data.GetValue("users").ToObject<Dictionary<string, (User User, string Password)>>();
                _externalUsers = data.GetValue("externalUsers").ToObject<Dictionary<string, User>>();
            }
        }
        private void PersistData()
        {
            var data = new { users = _users, externalUsers = _externalUsers };
            var str = JsonConvert.SerializeObject(data);
            File.WriteAllText("storage.json", str);
        }
        private string HashString(string str)
        {
            var message = Encoding.Unicode.GetBytes(str);
            var hash = new SHA256Managed();

            var hashValue = hash.ComputeHash(message);
            return Encoding.Unicode.GetString(hashValue);
        }
    }
}
