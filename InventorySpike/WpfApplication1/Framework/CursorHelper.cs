using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Framework
{
    public class CursorDisposable : IDisposable
    {
        private Cursor _previousCursor;

        public CursorDisposable(Cursor requestedCursor)
        {
            _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = requestedCursor;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }
    }

    public class WaitCursorDisposable : CursorDisposable
    {
        public WaitCursorDisposable()
            : base(Cursors.Wait)
        {
        }
    }

    public class CursorHelper
    {
        public static void ExecuteWithWaitCursor(Action action)
        {
            using (new WaitCursorDisposable())
            {
                action();
            }
        }

        public static void ExecuteWithCursor(Cursor cursor, Action action)
        {
            using (new CursorDisposable(cursor))
            {
                action();
            }
        }
    }
}