using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for SynchronizationContext.xaml
    /// </summary>
    public partial class SynchronizationContextWindow : Window
    {
        SynchronizationContext uiSyncContext;

        public SynchronizationContextWindow()
        {
            InitializeComponent();
            uiSyncContext = SynchronizationContext.Current;
            new Thread(DoAction).Start();

        }

        // NOTES: 
        // Calling Post is equivalent to calling BeginInvoke on a Dispatcher or Control;
        // there’s also a Send method that is equivalent to Invoke.


        private void DoAction()
        {

            Thread.Sleep(2000); // Simulate time-consuming task

            // exception or race condition will be happened.
            //System.InvalidOperationException: 'The calling thread cannot access
            //this object because a different thread owns it.'
            // ERROR: BAD CODE !!!!
            // this.DataControl.Text = "hello world";

            // marshal setting the Text of Textbox from the thread that executes 
            // the current method to the UI Thread SynchronizationContext.
            uiSyncContext.Post((v) => this.DataControl.Text = "hello world" ,null);


        }
    }
}
