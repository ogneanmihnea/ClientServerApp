using System;

namespace Model
{
    [Serializable]
    public class User : Entity<long>
    {
        public string username { get; set; }
        public string password { get; set; }

        public User()
        {
        }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override string ToString()
        {
            return $"{nameof(username)}: {username}, {nameof(password)}: {password}";
        }
    }
}