using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Infrastructure.IRepository
{
    public interface IOrderHeaderRepository : IRepositroy<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int Id, string orderstatus,string? paymentStatus = null);

        void PaymentStatus(int Id, string SessionId, string paymentIntentId);

    }
}
