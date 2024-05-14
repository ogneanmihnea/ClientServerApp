using System;

namespace Model
{
    [Serializable]
    public class Registration : Entity<long>
    {
        public Registration()
        {
        }

        public Registration(Participant participant, Competition competition)
        {
            this.participant = participant;
            this.competition = competition;
        }

        public Participant participant { get; set; }
        public Competition competition { get; set; }

        public override string ToString()
        {
            return $"{nameof(participant)}: {participant}, {nameof(competition)}: {competition}";
        }
    }
}