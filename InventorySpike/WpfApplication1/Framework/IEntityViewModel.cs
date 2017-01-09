using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Framework
{
    public interface IEntityViewModel
    {
        object Model { get; }
        Int64? EntityId { get; }
        void RefreshModel();
        bool SaveChanges();
    }

    public interface IEntityViewModel<T> : IEntityViewModel
        where T : class
    {
        new T Model { get; }
    }
}
