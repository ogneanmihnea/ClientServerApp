using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class IterableDTO
    {
        private IEnumerable<Participant> iterable;

        public IterableDTO(IEnumerable<Participant> iterable)
        {
            this.iterable = iterable;
        }

        public IEnumerable<Participant> Iterable
        {
            get { return iterable; }
            set { iterable = value; }
        }
    }
}