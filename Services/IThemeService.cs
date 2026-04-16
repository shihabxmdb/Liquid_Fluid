using Fluid;
using Microsoft.Extensions.FileProviders;

namespace Liquid_Fluid.Services
{
    public interface IThemeService
    {
        IFileProvider GetFileProvider();
        TemplateOptions GetTemplateOptions();
    }
}
