using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstract
{
    // özellikle abouta özgü bir durumda bunu kullanacağz. Ortak olanlar için IGENERİCDAL yeterli
    public interface IAboutDal: IGenericDal<About>
    {


    }
}
