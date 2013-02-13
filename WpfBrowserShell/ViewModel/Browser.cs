using System;
using System.Windows.Input;
using WpfBrowserShell.Interop;
using WpfBrowserShell.Common;

namespace WpfBrowserShell.ViewModel
{
    class Browser : NotifyBase
    {
        #region Property Location
        private string m_Location;
        public string Location
        {
            get { return m_Location; }
            set { SetValue(ref m_Location, value, () => Location); }
        }
        #endregion
        #region Property Interop
        private JsInterop m_Interop;
        public JsInterop Interop
        {
            get { return m_Interop; }
            set { SetValue(ref m_Interop, value, () => Interop); }
        }
        #endregion
    }
}
