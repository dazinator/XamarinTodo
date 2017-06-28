using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Todo
{
	public class TodoListPageCS : ContentPage
	{
		ListView listView;

        private readonly TodoItemDatabase _db;

        public TodoListPageCS(TodoItemDatabase db)
		{
            _db = db;

            Title = "Todo";

			var toolbarItem = new ToolbarItem
			{
				Text = "+",
				Icon = Device.OnPlatform(null, "plus.png", "plus.png")
			};
			toolbarItem.Clicked += async (sender, e) =>
			{

                var todoPage = ((App)App.Current).ServiceProvider.GetRequiredService<TodoItemPageCS>();
                todoPage.BindingContext = new TodoItem();
                await Navigation.PushAsync(todoPage);              
			};
			ToolbarItems.Add(toolbarItem);

			listView = new ListView
			{
				Margin = new Thickness(20),
				ItemTemplate = new DataTemplate(() =>
				{
					var label = new Label
					{
						VerticalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.StartAndExpand
					};
					label.SetBinding(Label.TextProperty, "Name");

					var tick = new Image
					{
						Source = ImageSource.FromFile("check.png"),
						HorizontalOptions = LayoutOptions.End
					};
					tick.SetBinding(VisualElement.IsVisibleProperty, "Done");

					var stackLayout = new StackLayout
					{
						Margin = new Thickness(20, 0, 0, 0),
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Children = { label, tick }
					};

					return new ViewCell { View = stackLayout };
				})
			};
			listView.ItemSelected += async (sender, e) =>
			{
				((App)App.Current).ResumeAtTodoId = (e.SelectedItem as TodoItem).ID;
				Debug.WriteLine("setting ResumeAtTodoId = " + (e.SelectedItem as TodoItem).ID);

                var todoPage = ((App)App.Current).ServiceProvider.GetRequiredService<TodoItemPageCS>();
                todoPage.BindingContext = e.SelectedItem as TodoItem;
                await Navigation.PushAsync(todoPage);
               
			};

			Content = listView;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Reset the 'resume' id, since we just want to re-start here
			((App)App.Current).ResumeAtTodoId = -1;
			listView.ItemsSource = await _db.GetItemsAsync();
		}
	}
}
