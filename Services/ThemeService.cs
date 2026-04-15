
using Fluid;
using Microsoft.Extensions.FileProviders;
using System;
namespace Liquid_Fluid.Services
{

    public interface IThemeService
    {
        IFileProvider GetFileProvider();
        TemplateOptions GetTemplateOptions();
    }
    public class ThemeService : IThemeService
    {
        private readonly IFileProvider _fileProvider;
        private readonly string _themeRoot;

        public ThemeService(IWebHostEnvironment env)
        {
            _themeRoot = Path.Combine(env.WebRootPath, "themes");

            // Ensure the directory exists to avoid errors
            if (!Directory.Exists(_themeRoot))
            {
                Directory.CreateDirectory(_themeRoot);
            }

            _fileProvider = new PhysicalFileProvider(_themeRoot);
        }

        public IFileProvider GetFileProvider() => _fileProvider;

        public TemplateOptions GetTemplateOptions()
        {
            var options = new TemplateOptions { FileProvider = _fileProvider };
            ////options.MemberAccessStrategy.Register(new { Message = "" }.GetType());
            options.MemberAccessStrategy.Register<object>();
            return options;
           
        }
    }
}
