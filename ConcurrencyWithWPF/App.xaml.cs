using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.OnStartup(e);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var builder = new StringBuilder()
                .AppendLine($"{e.Exception.Message} {e.Exception.StackTrace}")
                .AppendLine(e.Exception.InnerException.Message)
                .AppendLine(e.Exception.InnerException.StackTrace);

            builder.AppendLine(
                string.Join(Environment.NewLine, 
                e.Exception.InnerExceptions?.
                    Select(e => $"Exception: {e.Message}{Environment.NewLine}StackTrace: {e.StackTrace}")));

            File.WriteAllText("logs.txt", builder.ToString());
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }
    }
}
