using System;
using System.ComponentModel;
using Caliburn.Micro;

namespace Client.Framework.Docking
{
    /// <summary>
    ///   The dockable window conductor, used to allow for interaction between view and view model.
    /// </summary>
    internal class DockableContentConductor
    {
        #region Fields

        /// <summary>
        ///   The view.
        /// </summary>
        private readonly BaseDockableContent m_View;

        /// <summary>
        ///   The view model.
        /// </summary>
        private readonly object m_ViewModel;

        /// <summary>
        ///   The flag used to identify the view as closing.
        /// </summary>
        private bool m_IsClosing;

        /// <summary>
        ///   The flag used to determine if the view requested deactivation.
        /// </summary>
        private bool m_IsDeactivatingFromView;

        /// <summary>
        ///   The flag used to determine if the view model requested deactivation.
        /// </summary>
        private bool m_IsDeactivatingFromViewModel;

        #endregion Fields

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DockableContentConductor" /> class.
        /// </summary>
        /// <param name = "viewModel">The view model.</param>
        /// <param name = "view">The view.</param>
        public DockableContentConductor(object viewModel, BaseDockableContent view)
        {
            m_ViewModel = viewModel;
            m_View = view;

            view.IsActiveContentChanged += (sender, args) =>
                                                {
                                                    var baseDockableContent = sender as BaseDockableContent;
                                                    if (baseDockableContent != null && baseDockableContent.IsActiveContent)
                                                    {
                                                        var activatable = baseDockableContent.ViewModel as IActivate;
                                                        if (activatable != null && !activatable.IsActive)
                                                            CursorHelper.ExecuteWithWaitCursor(activatable.Activate);
                                                    }
                                                };

            var deactivatable = viewModel as IDeactivate;
            if (deactivatable != null)
            {
                view.Closed += OnClosed;
                deactivatable.Deactivated += OnDeactivated;
            }

            var guard = viewModel as IGuardClose;
            if (guard != null)
                view.Closing += OnClosing;
        }

        /// <summary>
        ///   Called when the view has been closed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void OnClosed(object sender, EventArgs e)
        {
            m_View.Closed -= OnClosed;
            m_View.Closing -= OnClosing;

            if (m_IsDeactivatingFromViewModel)
                return;

            var deactivatable = (IDeactivate)m_ViewModel;

            m_IsDeactivatingFromView = true;
            deactivatable.Deactivate(true);
            m_IsDeactivatingFromView = false;
        }

        /// <summary>
        ///   Called when the view has been deactivated.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "Caliburn.Micro.DeactivationEventArgs" /> instance containing the event data.</param>
        private void OnDeactivated(object sender, DeactivationEventArgs e)
        {
            ((IDeactivate)m_ViewModel).Deactivated -= OnDeactivated;

            if (!e.WasClosed || m_IsDeactivatingFromView)
                return;

            m_IsDeactivatingFromViewModel = true;
            m_IsClosing = true;
            m_View.Close();
            m_IsClosing = false;
            m_IsDeactivatingFromViewModel = false;
        }

        /// <summary>
        ///   Called when the view is about to be closed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.ComponentModel.CancelEventArgs" /> instance containing the event data.</param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            var guard = (IGuardClose)m_ViewModel;

            if (m_IsClosing)
            {
                m_IsClosing = false;
                return;
            }

            bool runningAsync = false, shouldEnd = false;

            bool async = runningAsync;
            guard.CanClose(canClose =>
                               {
                                   if (async && canClose)
                                   {
                                       m_IsClosing = true;
                                       m_View.Close();
                                   }
                                   else
                                       e.Cancel = !canClose;

                                   shouldEnd = true;
                               });

            if (shouldEnd)
                return;

            runningAsync = e.Cancel = true;
        }
    }
}