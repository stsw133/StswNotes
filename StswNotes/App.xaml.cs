global using StswExpress;
using System.Windows;
using System.Windows.Threading;

namespace StswNotes;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : StswApp
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }

    /// add this event if you want log unhandled exceptions
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => StswLog.Write(StswInfoType.Error, e.Exception.ToString());
}
