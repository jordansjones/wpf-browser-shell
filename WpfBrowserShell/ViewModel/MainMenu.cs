using WpfBrowserShell.Common;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WpfBrowserShell.ViewModel
{

    class MenuItem : NotifyBase
    {
        #region Property Name
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { SetValue(ref m_Name, value, () => Name); }
        }
        #endregion
        #region Property Command
        private ICommand m_Command;
        public ICommand Command
        {
            get { return m_Command; }
            set { SetValue(ref m_Command, value, () => Command); }
        }
        #endregion
    }

    class MainMenu : ObservableCollection<MenuItem>
    {
    }
}
