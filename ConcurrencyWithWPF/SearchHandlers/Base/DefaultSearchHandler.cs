using ConcurrencyWithWPF.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace ConcurrencyWithWPF.SearchHandlers.Base
{
    public class DefaultSearchHandler : ISearchHandler
    {

        public DefaultSearchHandler(TemplateWindow window, IUserService client)
        {
            this.window = window;
            this.client = client;
        }


        public string Title { get; protected set; }

        protected Stopwatch watch = new Stopwatch();
        protected readonly TemplateWindow window;
        protected readonly IUserService client;

        public virtual Task BeforeSearchAsync()
        {
            watch.Start();
            window.Progress.IsIndeterminate = true;
            window.Status.Text = $"Searching...";
            window.Progress.Visibility = Visibility.Visible;
            return Task.CompletedTask;
        }

        public virtual Task AfterSearchAsync()
        {
            if(watch.IsRunning)
                watch.Stop();
            window.Status.Text = $"Operation Finished in {watch.ElapsedMilliseconds} ms";
            window.Progress.IsIndeterminate = false;
            window.Progress.Visibility = Visibility.Hidden;
            return Task.CompletedTask;
        }

        public async virtual Task SearchingAsync()
        {
            window.Data.ItemsSource = await client.GetPageAsync(0, 100);
        }

        public virtual Task HandleSearchErrorAsync()
        {
            if (watch.IsRunning)
                watch.Stop();
            window.Status.Text = $"Operation Failed in {watch.ElapsedMilliseconds} ms";
            window.Progress.IsIndeterminate = false;
            window.Progress.Visibility = Visibility.Hidden;
            return Task.CompletedTask;
        }
    }



}