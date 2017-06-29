using Android.App;
using Android.OS;
using Android.Content.PM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Todo.FileProvider;
using Todo.Services;

namespace Todo
{

    [Activity(Label = "Todo", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity :
    global::Xamarin.Forms.Platform.Android.FormsApplicationActivity // superclass new in 1.3
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var app = new App();

            var services = new ServiceCollection();

            var hostEnv = new HostEnvironment();
            hostEnv.ContentRootPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var fp = new PhysicalFileProvider(hostEnv.ContentRootPath);
            hostEnv.ContentRootFileProvider = fp;
            hostEnv.ApplicationName = ApplicationInfo.LoadLabel(PackageManager);
            services.AddSingleton<IHostingEnvironment>(hostEnv);
          
            // Register a view lifecycle manager with the app, and also in the container.
            var viewLifecycleManager = new DroidViewLifecycleManager();
            services.AddSingleton<IDroidViewLifecycleManager>(viewLifecycleManager);
            var mainApp = MainApp.Current;
            mainApp.RegisterActivityLifecycleCallbacks(viewLifecycleManager);

            services.AddSingleton<IAndroidCurrentTopActivity, AndroidCurrentTopActivity>();


            services.AddTransient<IAccountService, AndroidAccountService>();



            app.RegisterServices(services);

            LoadApplication(app);
        }





    }
}


