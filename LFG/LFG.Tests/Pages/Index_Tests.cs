using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace LFG.Web.Pages;

[Collection(LFGTestConsts.CollectionDefinitionName)]
public class Index_Tests : LFGTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
