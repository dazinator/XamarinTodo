using Android.App;
using Todo.Services;
using System;
using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;

namespace Todo
{
    [Application]
    public class MainApp : global::Android.App.Application
    {

        private static MainApp _current;

        public MainApp(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {

        }

        public override void OnCreate()
        {
            try
            {
                base.OnCreate();               

                // Application Initialisation ...
                _current = this;

                // Global error handling.
                //   AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
                //   AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            }
            catch (Exception e)
            {
                // Log(e);
                throw;
            }
        }    
              
        public IServiceProvider ServiceProvider { get; set; }

        public static MainApp Current
        {
            get { return _current; }
        }      



    }
}


