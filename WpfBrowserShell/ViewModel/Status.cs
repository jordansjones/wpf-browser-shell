using WpfBrowserShell.Common;
using System;

namespace WpfBrowserShell.ViewModel
{
    class Status : NotifyBase
    {
        #region Property Completed
        private int m_Completed;
        public int Completed
        {
            get { return m_Completed; }
            set { SetValue(ref m_Completed, value, () => Completed); }
        }
        #endregion
        #region Property Selected
        private int m_Selected;
        public int Selected
        {
            get { return m_Selected; }
            set { SetValue(ref m_Selected, value, () => Selected); }
        }
        #endregion
        #region Property Total
        private int m_Total;
        public int Total
        {
            get { return m_Total; }
            set { SetValue(ref m_Total, value, () => Total); }
        }
        #endregion
				
    }
}
