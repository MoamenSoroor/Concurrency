using System;
using System.Collections.Generic;
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
    /// Interaction logic for DeadLock.xaml
    /// </summary>
    public partial class DeadLock : Window
    {
        public DeadLock()
        {
            InitializeComponent();
        }


        // The code in this example will deadlock if called from a UI or ASP.NET Classic
        // context because both of those contexts only allow one thread in at a time.
        // CreateDeadLock will call GetValueAfter, which begins the delay. CreateDeadLock then (synchronously)
        // waits for that method to complete, blocking the context thread. When the delay
        // completes, await attempts to resume GetValueAfter within the captured context,
        // but it cannot because there’s already a thread blocked in the context, and the
        // context only allows one thread at a time. CreateDeadLock can be prevented two ways:
        // you can use ConfigureAwait(false) within GetValueAfter(which causes await to ignore
        // its context), or you can await the call to GetValueAfter(making CreateDeadLock into an
        // async method).

        private void CreateDeadLock(object sender, RoutedEventArgs e)
        {
            Task task = GetValueAfter(500);
            
            // deadlock
            task.Wait();

            txtResult.Text += " 10";
        }

        private async Task<int> GetValueAfter(int delay)
        {
            // SynchronizationContext of UI Thread will be captured
            await Task.Delay(delay);
            // should execute on the captured context (SynchronizationContext)
            return 10;
        }

    }
}
