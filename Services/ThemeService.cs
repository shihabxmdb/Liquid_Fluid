
using Fluid;
using Liquid_Fluid.Models;
using Microsoft.Extensions.FileProviders;
using System;
namespace Liquid_Fluid.Services
{
    public class ThemeService : IThemeService
    {
        private  IFileProvider _fileProvider;
        //private readonly string _themeRoot;

        private readonly IWebHostEnvironment _env;

        public ThemeService(IWebHostEnvironment env)
        {
          _env = env;
        }

        public IFileProvider GetFileProvider()
        {
            string _themeRoot = Path.Combine(_env.WebRootPath, "themes");

            // Ensure the directory exists to avoid errors
            if (!Directory.Exists(_themeRoot))
            {
                Directory.CreateDirectory(_themeRoot);
            }

            _fileProvider = new PhysicalFileProvider(_themeRoot);
            return _fileProvider;
        }

        public TemplateOptions GetTemplateOptions()
        {
            var options = new TemplateOptions
            {
                FileProvider = _fileProvider
            };

            options.MemberAccessStrategy.Register<Student>();
            options.MemberAccessStrategy.Register<Contact>();
            options.MemberAccessStrategy.Register<Address>();
            options.MemberAccessStrategy.Register<Course>();

            return options;
        }

        //public TemplateOptions GetTemplateOptions()
        //{
        //    var options = new TemplateOptions { FileProvider = _fileProvider };
        //    ////options.MemberAccessStrategy.Register(new { Message = "" }.GetType());
        //    options.MemberAccessStrategy.Register<object>();
        //    return options;

        //}
    }
}
