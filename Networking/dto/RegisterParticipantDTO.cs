using System;

namespace Networking.dto
{
    [Serializable]
    public class RegisterParticipantDTO
    {
        public string Name { get; }
        public DateTime BirthDate { get; }
        public string Proba { get; set; }

        public RegisterParticipantDTO(string name, DateTime birthDate, string proba)
        {
            Name = name;
            BirthDate = birthDate;
            Proba = proba;
        }
    }
}