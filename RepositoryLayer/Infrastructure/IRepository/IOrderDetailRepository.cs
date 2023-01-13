using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface IOrderDetailRepository : IRepositroy<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    
    }
}
