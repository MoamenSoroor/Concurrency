using ConcurrencyWithWPF.SearchHandlers.Base;
using ConcurrencyWithWPF.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConcurrencyWithWPF
{
    /// <summary>
    /// Interaction logic for GeneralTemplate.xaml
    /// </summary>
    public partial class TemplateWindow : Window
    {
        public TemplateWindow( ISearchHandlerFactory searchHandlerFactory, IUserService client)
        {
            InitializeComponent();
            this.searchHandlerFactory = searchHandlerFactory;
            this.client = client;
            this.Loaded += TemplateWindow_Loaded;

        }


        private Stopwatch watch = new Stopwatch();
        private readonly ISearchHandlerFactory searchHandlerFactory;
        private readonly IUserService client;
        private ISearchHandler searchHandler;

        private async void TemplateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            searchHandler = searchHandlerFactory.Default;

            try
            {
                watch.Start();
                this.Progress.IsIndeterminate = true;
                this.Data.ItemsSource = await client.GetPageAsync(0, 200);
                watch.Stop();
                this.Status.Text = $"200 users Loaded Successfully in {watch.ElapsedMilliseconds} ms";
                this.Progress.IsIndeterminate = false;
                this.Progress.Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                this.Status.Text = "Failed to load data, check the API.";
                this.Progress.Visibility = Visibility.Hidden;
            }

        }


        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await searchHandler.BeforeSearchAsync();
                await searchHandler.SearchingAsync();
                await searchHandler.AfterSearchAsync();
            }
            catch (Exception ex)
            {
                await searchHandler.HandleSearchErrorAsync();
            }
        }

    }


}
