using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace Client.Framework
{
    public interface IInvWindowManager: IWindowManager
    {
        void ShowOrActivateWindow(object rootModel, object context = null, IDictionary<string, object> settings = null, bool showDialogue = false);

        void TryCloseWindow(object rootModel, System.Action closedHandler = null);

        bool Confirm(string title, string message, Window rootWindow = null);

        bool Confirm(string title, string message, object rootModel);

        void Warn(string title, string message, Window rootWindow = null);

        void Warn(string title, string message, object rootModel);

        void Inform(string title, string message, Window rootWindow = null);

        void Inform(string title, string message, object rootModel);

        void ShowError(string title, string message, Window rootWindow = null);

        void ShowError(string title, string message, object rootModel);

        List<string> PickFilesToOpen(string filter, string title, bool multiSelect);

        string PickFilesToSave(string filter, string title);

        List<string> GetOpenFileDialogNames(string filter, string title, bool multiSelect);
    }
}
