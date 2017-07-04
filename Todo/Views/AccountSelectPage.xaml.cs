using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Todo
{
    public partial class AccountSelectPage : ContentPage
    {

        private readonly TodoItemDatabase _db;
        private readonly IAccountService _accountService;
        //   private readonly ObservableCollection<IUserAccount> _accounts;

        public AccountSelectPage(TodoItemDatabase db, IAccountService accountService)
        {
            _db = db;
            _accountService = accountService;
            InitializeComponent();
            //this.BindingContext = _accounts;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reset the 'resume' id, since we just want to re-start here
            // ((App)App.Current).ResumeAtTodoId = -1;

            var accounts = await _accountService.GetAccounts(AuthContstants.AccountType);
            Device.BeginInvokeOnMainThread(() =>
            {
                var observableAccountsList = new ObservableCollection<IUserAccount>();
                foreach (var item in accounts)
                {
                    observableAccountsList.Add(item);
                }
                listView.ItemsSource = observableAccountsList;
            });

        }

        async void OnAddNewAccount(object sender, EventArgs e)
        {
            await _accountService.AddAccount(AuthContstants.AccountType, null);
            var accounts = await _accountService.GetAccounts(AuthContstants.AccountType);

            Device.BeginInvokeOnMainThread(() =>
            {
                var observableAccountsList = new ObservableCollection<IUserAccount>();
                foreach (var item in accounts)
                {
                    observableAccountsList.Add(item);
                }
                listView.ItemsSource = observableAccountsList;
            });

            //var todoPage = ((App)App.Current).ServiceProvider.GetRequiredService<TodoItemPage>();
            //todoPage.BindingContext = new TodoItem();
            //await Navigation.PushAsync(todoPage);
            // listView.ItemsSource = await _accountService.GetAccounts("com.todo.auth_example");

        }

        async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void OnListItemSelected(object sender, ItemTappedEventArgs e)
        {
            //((App)App.Current).ResumeAtTodoId = (e.Item as TodoItem).ID;
            //Debug.WriteLine("setting ResumeAtTodoId = " + (e.Item as TodoItem).ID);

            var account = (e.Item as IUserAccount);
            await _accountService.SetAccount(account);

            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopAsync();
            });

            // ((App)App.Current).ResumeAtTodoId = (e.Item as TodoItem).ID;
            //  Debug.WriteLine("setting ResumeAtTodoId = " + (e.Item as TodoItem).ID);

            //var todoPage = ((App)App.Current).ServiceProvider.GetRequiredService<TodoItemPage>();
            //todoPage.BindingContext = e.Item as TodoItem;
            //await Navigation.PushAsync(todoPage);
        }



        //async void OnAuthenticate(object sender, EventArgs e)
        //{

        //    await _accountService.("com.todo.auth_example", null);
        //}

    }
}
