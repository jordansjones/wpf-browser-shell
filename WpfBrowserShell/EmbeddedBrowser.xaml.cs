using WpfBrowserShell.Interop;
using System;
using System.Windows.Controls;


using System.Runtime.InteropServices;
using System.Windows;
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
internal interface IServiceProvider
{
    [return: MarshalAs(UnmanagedType.IUnknown)]
    object QueryService(ref Guid guidService, ref Guid riid);
}

namespace WpfBrowserShell
{
    /// <summary>
    /// Interaction logic for EmbeddedBrowser.xaml
    /// </summary>
    public partial class EmbeddedBrowser : UserControl
    {
        public static readonly Guid SID_SWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");

        public EmbeddedBrowser()
        {
            InitializeComponent();
            WindowsInterop.SecurityAlertDialogWillBeShown +=
                new GenericDelegate<bool, bool>(
                    SecurityAlertDialogWillBeShown_);
            browser.Navigated += OnBrowserNavigated;
            browser.Loaded += OnBrowserLoaded;
            browser.LoadCompleted += OnBrowserLoadCompleted;
        }



        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(EmbeddedBrowser), new PropertyMetadata(
                new PropertyChangedCallback( (obj, args) =>
                {
                    var eb = obj as EmbeddedBrowser;
                    if (eb != null)
                    {
                        var s = args.NewValue as string;
                        Uri uri = new Uri(s);
                        eb.browser.Source = uri;
                    }
                })));



        public JsInterop Interop
        {
            get { return (JsInterop)GetValue(InteropProperty); }
            set { SetValue(InteropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InteropProperty =
            DependencyProperty.Register("Interop", typeof(JsInterop), typeof(EmbeddedBrowser), null);



        void OnBrowserLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        // SHDocVw: Provided by adding a reference to Microsoft Internet Controls (COM)
        SHDocVw.DWebBrowserEvents2_Event m_wbEvents2 = null;
        void HookBrowserWindowEvents()
        {
            IServiceProvider serviceProvider = (IServiceProvider)browser.Document;
            if (serviceProvider != null)
            {
                Guid serviceGuid = SID_SWebBrowserApp;
                Guid iid = typeof(SHDocVw.IWebBrowser2).GUID;

                SHDocVw.IWebBrowser2 comBrowser = (SHDocVw.IWebBrowser2)serviceProvider.QueryService(ref serviceGuid, ref iid);
                comBrowser.RegisterAsBrowser = true;

                m_wbEvents2 = comBrowser as SHDocVw.DWebBrowserEvents2_Event;
                if (m_wbEvents2 != null)
                {
                    m_wbEvents2.NewWindow2 += new SHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(OnBrowserNewWindowEvent);
                }
            }
        }

        public void SetParentBrowser(ref object parentAppHandle)
        {
            browser.Navigate(new Uri("about:blank"));
            var serviceProvider = browser.Document as IServiceProvider;
            if (serviceProvider != null)
            {
                Guid serviceGuid = SID_SWebBrowserApp;
                Guid iid = typeof(SHDocVw.IWebBrowser2).GUID;
                //m_comBrowser = (SHDocVw.IWebBrowser2)serviceProvider.QueryService(ref serviceGuid, ref iid);
                //parentAppHandle = m_comBrowser.Application;
            }
        }

        void OnBrowserLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.Interop != null)
            {
                this.Interop.InvokeJs = new Action<string, string>((callback, json) =>
                    {
                        this.browser.InvokeScript(callback, json);
                    });
                browser.ObjectForScripting = this.Interop;
            }
        }

        void OnBrowserNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
           HookBrowserWindowEvents();
        }

        void OnBrowserNewWindowEvent(ref object ppDisp, ref bool Cancel)
        {
            // TODO: Create a new window if allowed
            //       Set the content to an instance of EmbeddedBrowser
            //       Set its parent browser window to ppDisp
            //BrowserPopup wnd = new BrowserPopup();
            //wnd.SetParentBrowser(ref ppDisp);
            //wnd.Show();
            System.Windows.MessageBox.Show("Browser New Window Event");
            Cancel = true;
        }

        bool SecurityAlertDialogWillBeShown_(bool param)
        {
            return true; // Make this a configurable property
        }
    }
}
