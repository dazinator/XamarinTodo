using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Gluon.Client.Jwt;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Todo
{
    public class App : Application
    {

        public App()
        {


        }

        public void RegisterServices(IServiceCollection services)
        {
            // Register services here.
            services.AddEntityFrameworkSqlite();

            services.AddDbContext<TodoItemDatabase>();


            services.AddTransient<NavigationPage>((a) =>
            {
                var todolistpage = a.GetRequiredService<TodoListPage>();
                return new NavigationPage(todolistpage);
            });

            services.AddTransient<TodoListPage>();
            services.AddTransient<TodoItemPage>();
            services.AddTransient<TodoItemPageCS>();
            services.AddTransient<TodoListPageCS>();

            var tokenApiClient = new TokenApiClient(new Uri(AuthContstants.TokenApiUrl));

            services.AddSingleton<ITokenApiClient>(tokenApiClient);
            services.AddSingleton<ITokenManager, TokenManager>();

            services.AddTransient<AccountSelectPage>();

            services.AddMemoryCache();

            ServiceProvider = services.BuildServiceProvider();

            using (var scope = ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TodoItemDatabase>();
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }

           

            ShowMainPage();

        }

        public IServiceProvider ServiceProvider { get; set; }

        private void ShowMainPage()
        {
            Resources = new ResourceDictionary();
            Resources.Add("primaryGreen", Color.FromHex("91CA47"));
            Resources.Add("primaryDarkGreen", Color.FromHex("6FA22E"));


            //  TodoItemDatabase
            var nav = ServiceProvider.GetRequiredService<NavigationPage>();

            //var nav = new NavigationPage(new TodoListPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
            nav.BarTextColor = Color.White;

            MainPage = nav;
        }


        public int ResumeAtTodoId { get; set; }

        protected override void OnStart()
        {
            //Debug.WriteLine("OnStart");

            //// always re-set when the app starts
            //// users expect this (usually)
            ////			Properties ["ResumeAtTodoId"] = "";
            //if (Properties.ContainsKey("ResumeAtTodoId"))
            //{
            //	var rati = Properties["ResumeAtTodoId"].ToString();
            //	Debug.WriteLine("   rati=" + rati);
            //	if (!String.IsNullOrEmpty(rati))
            //	{
            //		Debug.WriteLine("   rati=" + rati);
            //		ResumeAtTodoId = int.Parse(rati);

            //		if (ResumeAtTodoId >= 0)
            //		{
            //			var todoPage = new TodoItemPage();
            //			todoPage.BindingContext = await Database.GetItemAsync(ResumeAtTodoId);
            //			await MainPage.Navigation.PushAsync(todoPage, false); // no animation
            //		}
            //	}
            //}
        }

        protected override void OnSleep()
        {
            //Debug.WriteLine("OnSleep saving ResumeAtTodoId = " + ResumeAtTodoId);
            //// the app should keep updating this value, to
            //// keep the "state" in case of a sleep/resume
            //Properties["ResumeAtTodoId"] = ResumeAtTodoId;
        }

        protected override void OnResume()
        {
            //Debug.WriteLine("OnResume");
            //if (Properties.ContainsKey("ResumeAtTodoId"))
            //{
            //	var rati = Properties["ResumeAtTodoId"].ToString();
            //	Debug.WriteLine("   rati=" + rati);
            //	if (!String.IsNullOrEmpty(rati))
            //	{
            //		Debug.WriteLine("   rati=" + rati);
            //		ResumeAtTodoId = int.Parse(rati);

            //		if (ResumeAtTodoId >= 0)
            //		{
            //			var todoPage = new TodoItemPage();
            //			todoPage.BindingContext = await Database.GetItemAsync(ResumeAtTodoId);
            //			await MainPage.Navigation.PushAsync(todoPage, false); // no animation
            //		}
            //	}
            //}
        }


    }
}

