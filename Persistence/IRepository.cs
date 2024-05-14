

using System.Collections.Generic;
using Model;

namespace Persistence
{

    public interface IRepository<ID, E> where E : Entity<ID>
    {

        IEnumerable<E> FindAll();

        E findOne(ID id);


        E save(E entity);



        E delete(ID id);


        E update(E entity);
    }
}