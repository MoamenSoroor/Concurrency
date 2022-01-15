using ConcurrencyWithWPF;
using ConcurrencyWithWPF.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1;

namespace ConcurrencyWithWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;
        public App()
        {
            Startup startup = new Startup();
            
            serviceProvider = startup.Build();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


            var mainWindow = serviceProvider.GetService<TemplateWindow>();
            mainWindow.Show();

            RaceCondition raceWindow = new RaceCondition();
            raceWindow.Show();

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


    }

}
