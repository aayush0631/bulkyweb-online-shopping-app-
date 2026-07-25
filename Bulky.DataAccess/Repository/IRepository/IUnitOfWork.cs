using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IAppllicationUserRepository ApplicationUser { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeader { get; }
        ICompanyRepository Company { get; }

        void Save();
    }
}
