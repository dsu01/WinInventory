using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Framework
{
    public interface ICancelable
    {
        void Cancel();

        bool CanCancel { get; }
    }

    public interface IDialog : ICancelable
    {
        bool? DialogResult { get; }
    }

    public interface ISaveCancelDialog : IDialog
    {
        void Save();

        bool CanSave { get; }
    }

    public interface IOKCancelDialog : IDialog
    {
        void OK();

        bool CanOK { get; }
    }
}
