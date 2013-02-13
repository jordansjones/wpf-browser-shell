using Newtonsoft.Json;
using WpfBrowserShell.Common;
using WpfBrowserShell.Interop;
using System;
using System.Linq;
using System.Windows.Input;


namespace WpfBrowserShell.ViewModel
{
    class Todo : NotifyBase
    {
        public Todo()
        {
            Initialized = false;
            Status = new Status()
            {
                Total = 0,
                Completed = 0,
                Selected = 0
            };
            MainMenu = new MainMenu();
            InitMainMenu();
            Browser = new Browser()
            {
                Location = "http://jordansjones.github.com/WpfJsInterop/ ",
                Interop = new JsInterop()
            };
            Browser.Interop.JsReady += OnJsReady;
            Browser.Interop.Status += OnStatus;
            
        }
        
        #region Property Initialized
        private bool m_Initialized;
        public bool Initialized
        {
            get { return m_Initialized; }
            set { SetValue(ref m_Initialized, value, () => Initialized); }
        }
        #endregion
        #region Property Status
        private Status m_Status;
        public Status Status
        {
            get { return m_Status; }
            set { SetValue(ref m_Status, value, () => Status); }
        }
        #endregion
        public MainMenu MainMenu { get; private set; }
        public Browser Browser { get; private set; }

        int TaskCount { get; set; }
        string SelectedItemsJson { get; set; }
        void OnStatus(object sender, JsStatus e)
        {
            
            SelectedItemsJson = JsonConvert.SerializeObject(e.Selected);
            Status = new Status()
            {
                Total = e.Total,
                Completed = e.Completed,
                Selected = e.Selected.Count(),
            };
        }

        void OnJsReady(object sender, EventArgs e)
        {
            Browser.Interop.JsReady -= OnJsReady;
            Initialized = true;
            CommandManager.InvalidateRequerySuggested();
        }

        void InitMainMenu()
        {
            MainMenu.Add(new MenuItem()
            {
                Name = "New Todo Item",
                Command = new RelayCommand<object>((param) =>
                {
                    if (Initialized)
                        Browser.Interop.NotifyJs("add", "[\"Todo Item " + TaskCount++ + "\"]");
                }, (param) =>
                {
                    return Initialized;
                })
            });

            MainMenu.Add(new MenuItem()
            {
                Name = "Remove Selected",
                Command = new RelayCommand<object>((param) =>
                {
                    if (Initialized)
                        Browser.Interop.NotifyJs("remove", SelectedItemsJson);
                }, (param) =>
                {
                    return Initialized;
                })
            });
        }
    }
}
