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
    /// Interaction logic for RaceCondition.xaml
    /// </summary>
    public partial class RaceConditionWindow : Window
    {
        public RaceConditionWindow()
        {
            InitializeComponent();

            var panel = new StackPanel();
            panel.Children.Add(_button);
            panel.Children.Add(_results);
            Content = panel;
            _button.Click += (sender, args) =>
            {
                _results.Text = "";
                _button.IsEnabled = false;
                Task.Run(() => Go());
                //GoNormal();
            };
        }

        Button _button = new Button { Content = "Go" };
        TextBlock _results = new TextBlock();


        int GetPrimesCount(int start, int count)
        {
            return ParallelEnumerable.Range(start, count).Count(n =>
            Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

        void Go()
        {
            for (int i = 1; i < 5; i++)
            {
                //int count = i;
                int result = GetPrimesCount(i * 1000_000, 1000_000);
                Dispatcher.BeginInvoke(new Action(() =>
                _results.Text += result + " primes between " + (i * 1000000) +
                " and " + ((i + 1) * 1000000 - 1) + Environment.NewLine));
            }
            Dispatcher.BeginInvoke(new Action(() => _button.IsEnabled = true));
        }


        void GoNormal()
        {
            for (int i = 1; i < 5; i++)
            {
                int result = GetPrimesCount(i * 1000_000, 1000_000);
                _results.Text += result + " primes between " + (i * 1000000) +
                " and " + ((i + 1) * 1000000 - 1) + Environment.NewLine;
            }
            _button.IsEnabled = true;
        }
    }
}
