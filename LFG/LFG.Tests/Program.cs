using Microsoft.AspNetCore.Builder;
using LFG;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("LFG.csproj");
await builder.RunAbpModuleAsync<LFGTestModule>(applicationName: "LFG");
namespace LFG
{
    public partial class Program
    {
    }
}
