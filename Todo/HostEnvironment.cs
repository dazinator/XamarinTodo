using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Todo
{
    public class HostEnvironment : IHostingEnvironment
    {

        public HostEnvironment()
        {

        }
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

    }
}
