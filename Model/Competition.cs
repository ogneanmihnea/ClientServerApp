using System;

namespace Model
{
    [Serializable]
    public class Competition : Entity<long>
    {
        public string trial { get; set; }
        public int startingAgeInterval { get; set; }
        public int endingAgeInterval { get; set; }

        public Competition()
        {
        }

        public Competition(string trial, int startingAgeInterval, int endingAgeInterval)
        {
            this.trial = trial;
            this.startingAgeInterval = startingAgeInterval;
            this.endingAgeInterval = endingAgeInterval;
        }

        public override string ToString()
        {
            return
                $"{nameof(trial)}: {trial}, {nameof(startingAgeInterval)}: {startingAgeInterval}, {nameof(endingAgeInterval)}: {endingAgeInterval}";
        }
    }
}