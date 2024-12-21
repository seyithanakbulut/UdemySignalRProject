using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        void TAdd(T entity);
        void TDelete(T entity);

        void TUpdate(T entity);


        // id ye göre getir
        T TGetByID(int id);



        // tüm listeyi getir
        List<T> TGetListAll();
    }
}
