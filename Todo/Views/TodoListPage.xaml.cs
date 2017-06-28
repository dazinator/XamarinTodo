using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Todo
{
	public partial class TodoListPage : ContentPage
	{

        private readonly TodoItemDatabase _db;

        public TodoListPage(TodoItemDatabase db)
		{
            _db = db;
            InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Reset the 'resume' id, since we just want to re-start here
			((App)App.Current).ResumeAtTodoId = -1;
			listView.ItemsSource = await _db.GetItemsAsync();
		}

		async void OnItemAdded(object sender, EventArgs e)
		{
            var todoPage = ((App)App.Current).ServiceProvider.GetRequiredService<TodoItemPage>();
            todoPage.BindingContext = new TodoItem();
            await Navigation.PushAsync(todoPage);
		}

		async void OnListItemSelected(object sender, ItemTappedEventArgs e)
		{
			((App)App.Current).ResumeAtTodoId = (e.Item as TodoItem).ID;
			Debug.WriteLine("setting ResumeAtTodoId = " + (e.Item as TodoItem).ID);

            var todoPage = ((App)App.Current).ServiceProvider.GetRequiredService<TodoItemPage>();
            todoPage.BindingContext = e.Item as TodoItem;
            await Navigation.PushAsync(todoPage);
		}
	}
}
