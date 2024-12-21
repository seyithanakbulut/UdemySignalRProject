using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstract
{

    // T değeri alacak ve bu T  Değeri bi sınıf olmak zorunda
    public interface IGenericDal<T> where T : class
    {

        void Add(T entity); 
        void Delete (T entity); 

        void Update(T entity);  


        // id ye göre getir
        T GetByID(int id);



        // tüm listeyi getir
        List<T> GetListAll();   
    }
}
