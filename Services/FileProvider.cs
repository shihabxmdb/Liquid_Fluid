using Fluid;
using Microsoft.Extensions.FileProviders;

namespace Liquid_Fluid.Services
{
    public class Service
    {
        private  readonly IWebHostEnvironment _env;

        public Service() { }
        public Service (IWebHostEnvironment env)
        {
            _env = env;
        }
        public  IFileProvider fileProvider()
        {
            var themeRoot = Path.Combine(_env.WebRootPath, "themes");
            var fileProvider = new PhysicalFileProvider(themeRoot);
            var options = new TemplateOptions { FileProvider = fileProvider };
            //options.MemberAccessStrategy.Register(new { Message = "" }.GetType());
            options.MemberAccessStrategy.Register<object>();

            return fileProvider;
        }
        
    }
}
