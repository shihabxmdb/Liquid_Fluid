using System.Diagnostics;
using Liquid_Fluid.Models;
using Microsoft.AspNetCore.Mvc;
using Fluid;
using Microsoft.Extensions.FileProviders;
using Liquid_Fluid;

namespace Liquid_Fluid.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IStorefrontService _storefrontService;
        // FluidParser is thread-safe; usually better to keep one instance
        private static readonly FluidParser _parser = new FluidParser();

        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger,//, IStorefrontService storefrontService)
            IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            //_storefrontService = storefrontService;
        }

        public async Task<IActionResult> Index()
        {
            var themeRoot = Path.Combine(_env.WebRootPath, "themes");
            var fileProvider = new PhysicalFileProvider(themeRoot);
            var options = new TemplateOptions { FileProvider = fileProvider };
            //options.MemberAccessStrategy.Register(new { Message = "" }.GetType());
            options.MemberAccessStrategy.Register<object>();

            // 1. Render the inner page (home.liquid)
            var homeFile = fileProvider.GetFileInfo("home.liquid");
            string homeSource = await ReadFile(homeFile);
            _parser.TryParse(homeSource, out var homeTemplate, out var error);

            var context = new TemplateContext(new { Message = "This is Body 22" }, options);
            var bodyContent = await homeTemplate.RenderAsync(context);

            // 2. Render the layout and inject the bodyContent
            var layoutFile = fileProvider.GetFileInfo("layout.liquid");
            string layoutSource = await ReadFile(layoutFile);

            if (_parser.TryParse(layoutSource, out var layoutTemplate))
            {
                // Inject the already-rendered HTML into the "content" variable
                context.SetValue("content", bodyContent);
                var finalHtml = await layoutTemplate.RenderAsync(context);

                return Content(finalHtml, "text/html");
            }

            return Content("Error loading layout", "text/plain");
        }

        private async Task<string> ReadFile(IFileInfo fileInfo)
        {
            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        //public async Task<IActionResult> Index()
        //{
        //    var themeRoot = Path.Combine(_env.WebRootPath, "themes");
        //    var fileProvider = new PhysicalFileProvider(themeRoot);

        //    // List all files the provider can see
        //    //var contents = fileProvider.GetDirectoryContents("");
        //    //foreach (var item in contents)
        //    //{
        //    //    Console.WriteLine($"Found file: {item.Name}");
        //    //}

        //    var relativePath = "home.liquid";
        //    var fileInfo = fileProvider.GetFileInfo(relativePath);

        //    if (!fileInfo.Exists) 
        //        return NotFound();

        //    // 1. Better approach: Use a cache here so you don't parse every time
        //    string source;
        //    using (var stream = fileInfo.CreateReadStream())
        //    using (var reader = new StreamReader(stream))
        //    {
        //        source = await reader.ReadToEndAsync();
        //    }

        //    if (_parser.TryParse(source, out var template, out var error))
        //    {
        //        var model = new { Message = "Hello World!" };

        //        // 2. MUST register members or Liquid won't see them
        //        var options = new TemplateOptions();
        //        options.MemberAccessStrategy.Register(model.GetType());

        //        // 3. Allow {% include %} tags to work
        //        options.FileProvider = fileProvider;

        //        var context = new TemplateContext(model, options);




        //        var html = await template.RenderAsync(context);
        //        return Content(html, "text/html");
        //    }

        //    return Content($"Liquid Error: {error}", "text/plain");
        //}

        //public async Task<IActionResult> Index()
        //{
        //    // 1. Setup paths (Assuming 'themes' folder is in your project root)
        //    var themeRoot = Path.Combine(_env.ContentRootPath, "themes");
        //    if (!Directory.Exists(themeRoot))
        //    {
        //        Directory.CreateDirectory(themeRoot);
        //    }
        //    var fileProvider = new PhysicalFileProvider(themeRoot);

        //    // Example: "tenant1/v1.0/templates/home.liquid"
        //    // Note: Ensure tenant and ThemeId are resolved from your logic/database
        //    //var relativePath = $"tenant_{tenant.StoreId}/{tenant.ThemeId}/templates/home.liquid";

        //    // 2. Just look for the file name directly
        //    var relativePath = "home.liquid";

        //    var fileInfo = fileProvider.GetFileInfo(relativePath);

        //    if (!fileInfo.Exists)
        //    {
        //        return NotFound($"Template not found at: {relativePath}");
        //    }

        //    // 2. Read the file from hard disk
        //    string source;
        //    using (var stream = fileInfo.CreateReadStream())
        //    using (var reader = new StreamReader(stream))
        //    {
        //        source = await reader.ReadToEndAsync();
        //    }

        //    // 3. Parse the Liquid source
        //    if (_parser.TryParse(source, out var template, out var error))
        //    {
        //        // 4. Fetch your data model
        //        //var model = await _storefrontService.BuildHomeModelAsync(tenant.StoreId);

        //        // 4. Create the "Model" (the data you want to send to the template)
        //        var model = new { Message = "Hello World from C#!" };


        //        // 5. Configure Fluid Context
        //        //var options = new TemplateOptions();

        //        // CRITICAL: This allows Fluid to see your C# properties
        //        // You can register specific types, or use 'RegisterAny' for flexibility (dev only)
        //        //options.MemberAccessStrategy.Register(model.GetType());

        //        //var context = new TemplateContext(model, options);
        //        var context = new TemplateContext(model);

        //        // Allow the template to find partials (like {% include 'header' %}) on disk
        //        //context.FileProvider = fileProvider;

        //        // 6. Render and return
        //        var html = await template.RenderAsync(context);
        //        return Content(html, "text/html");
        //    }
        //    else
        //    {
        //        // Return the error so you can see what's wrong with your Liquid syntax
        //        return Content($"Liquid Syntax Error: {error}", "text/plain");
        //    }
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
