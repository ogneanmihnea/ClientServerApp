using System;

namespace Model
{
    [Serializable]
    public class Entity<ID>
    {
        protected ID id;

        public ID Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}