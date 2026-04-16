
using Fluid;
using Liquid_Fluid;
using Liquid_Fluid.Models;
using Liquid_Fluid.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Diagnostics;

namespace Liquid_Fluid.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        private static readonly FluidParser _parser = new FluidParser();

        private readonly IThemeService _themeService;

        public HomeController(ILogger<HomeController> logger,IThemeService themeService)
        {
            _logger = logger;
            _themeService = themeService;
        }
        [HttpGet("/index")]
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var students = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    Name = "Rahim",
                    Age = 20,
                    Contact = new Contact
                    {
                        Email = "rahim@gmail.com",
                        Phone = "01700000000"
                    },
                    Address = new Address
                    {
                        City = "Dhaka",
                        Zip = "1207"
                    },
                    Courses = new List<Course>
                    {
                        new Course { CourseId = "C101", CourseName = "Math", Marks = 85 },
                        new Course { CourseId = "C102", CourseName = "English", Marks = 78 }
                    }
                },
                new Student
                {
                    Id = 2,
                    Name = "Karim",
                    Age = 22,
                    Contact = new Contact
                    {
                        Email = "karim@gmail.com",
                        Phone = "01800000000"
                    },
                    Address = new Address
                    {
                        City = "Sylhet",
                        Zip = "3100"
                    },
                    Courses = new List<Course>
                    {
                        new Course { CourseId = "C101", CourseName = "Math", Marks = 90 },
                        new Course { CourseId = "C102", CourseName = "ENG", Marks = 90 }
                    }
                }
            };

            //Get fileProvider
            var fileProvider = _themeService.GetFileProvider();

            // 1. Load home file
            var homeFile = fileProvider.GetFileInfo("index.liquid");

            if (!homeFile.Exists)
                return Content("index.liquid not found", "text/plain");

            string homeSource = await ReadFile(homeFile);

            if (!_parser.TryParse(homeSource, out var homeTemplate, out var error))
                return Content($"Parse error: {error}", "text/plain");

            // Context
            var options = _themeService.GetTemplateOptions();
            var context = new TemplateContext(new { Students = students }, options);

            context.SetValue("Request", new { Path = Request.Path.Value });

            // Render body
            var bodyContent = await homeTemplate.RenderAsync(context);

            // 2. Layout
            var layoutFile = fileProvider.GetFileInfo("layout.liquid");

            if (!layoutFile.Exists)
                return Content("layout.liquid not found", "text/plain");

            string layoutSource = await ReadFile(layoutFile);

            if (!_parser.TryParse(layoutSource, out var layoutTemplate, out var layoutError))
                return Content($"Layout parse error: {layoutError}", "text/plain");

            context.SetValue("content", bodyContent);

            var finalHtml = await layoutTemplate.RenderAsync(context);

            return Content(finalHtml, "text/html");
        }

        private async Task<string> ReadFile(IFileInfo fileInfo)
        {
            using var stream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        //[HttpGet("/index")]
        public async Task<IActionResult> Index1()
        {
            var fileProvider = _themeService.GetFileProvider(); // Get your provider
            var file = fileProvider.GetFileInfo("index.liquid");

            string source = await ReadFile(file);

            // --- CRITICAL PART ---
            var context = new TemplateContext();
            // Assign the FileProvider so Fluid can resolve includes/partials
            context.Options.FileProvider = fileProvider;

            context.SetValue("Request", new { Path = Request.Path.Value });

            // Parse and Render Index
            if (_parser.TryParse(source, out var template, out var error))
            {
                var bodyContent = await template.RenderAsync(context);

                // Render the layout
                var layoutFile = fileProvider.GetFileInfo("layout.liquid");
                string layoutSource = await ReadFile(layoutFile);

                if (_parser.TryParse(layoutSource, out var layoutTemplate))
                {
                    context.SetValue("content", bodyContent);
                    var finalHtml = await layoutTemplate.RenderAsync(context);

                    return Content(finalHtml, "text/html");
                }
            }

            return Content($"Error: {error}", "text/plain");
        }

        [HttpGet("/index2")]
        public async Task<IActionResult> Index2()
        {
            var fileProvider = _themeService.GetFileProvider(); // Get your provider
            var file = fileProvider.GetFileInfo("index2.liquid");

            string source = await ReadFile(file);

            // --- CRITICAL PART ---
            var context = new TemplateContext();
            // Assign the FileProvider so Fluid can resolve includes/partials
            context.Options.FileProvider = fileProvider;

            context.SetValue("Request", new { Path = Request.Path.Value });

            // Parse and Render Index
            if (_parser.TryParse(source, out var template, out var error))
            {
                var bodyContent = await template.RenderAsync(context);

                // Render the layout
                var layoutFile = fileProvider.GetFileInfo("layout.liquid");
                string layoutSource = await ReadFile(layoutFile);

                if (_parser.TryParse(layoutSource, out var layoutTemplate))
                {
                    context.SetValue("content", bodyContent);
                    var finalHtml = await layoutTemplate.RenderAsync(context);

                    return Content(finalHtml, "text/html");
                }
            }

            return Content($"Error: {error}", "text/plain");
        }
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

































































































