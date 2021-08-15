using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public partial class GeneralTemplate : Window
    {
        public GeneralTemplate()
        {
            InitializeComponent();
        }

        private Stopwatch watch;

        public  Task AfterSearchAsync()
        {
            watch.Stop();
            this.Status.Text = $"Operation Finished in {watch.ElapsedMilliseconds} ms";

            this.Progress.Visibility = Visibility.Hidden;
            return Task.CompletedTask;
        }

        public Task SearchingAsync()
        {

            return Task.CompletedTask;
        }

        private Task BeforeSearchAsync()
        {
            watch.Start();
            this.Status.Text = $"Searching...";
            this.Progress.Visibility = Visibility.Visible;
            return Task.CompletedTask;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }


    public class GeneralTemplateState
    {
        public string Title { get;}

        public GeneralTemplate Template { get; set; }

        
    }

}
