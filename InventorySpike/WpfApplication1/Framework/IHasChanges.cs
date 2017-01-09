using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Client.Framework
{
    public interface IHasChanges
    {
        bool HasChanges { get; set; }

        void AcceptChanges();
    }
}
