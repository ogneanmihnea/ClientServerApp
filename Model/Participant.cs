using System;

namespace Model
{
    [Serializable]
    public class Participant : Entity<long>
    {
        public string name { get; set; }
        public DateTime birthDate { get; set; }

        public Participant()
        {
        }

        public Participant(string name, DateTime birthDate)
        {
            this.name = name;
            this.birthDate = birthDate;
        }

        public override string ToString()
        {
            return $"{nameof(name)}: {name}, {nameof(birthDate)}: {birthDate}";
        }
    }
}