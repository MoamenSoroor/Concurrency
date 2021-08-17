using ConcurrencyWithWPF.SearchHandlers.Base;
using ConcurrencyWithWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ConcurrencyWithWPF.SearchHandlers
{
    public class WhenAllSearchHandler : DefaultSearchHandler
    {

        public WhenAllSearchHandler(TemplateWindow window, IUserService client) : base(window, client)
        {

        }
        public async override Task SearchingAsync()
        {

            var countries = window.SearchText.Text.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            List<Task<IList<User>>> usersTask = new List<Task<IList<User>>>();
            usersTask.AddRange(countries.Select(c => client.SearchByCountry(c)));

            var result = await Task.WhenAll(usersTask);

            window.Data.ItemsSource = new ObservableCollection<User>(result.SelectMany(r => r));
        }


    }



}