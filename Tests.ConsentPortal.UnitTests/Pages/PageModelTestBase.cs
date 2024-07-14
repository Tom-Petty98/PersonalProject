using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PersonalProject.Domain.Request;
using Tests.ConsentPortal.UnitTests.TestHelpers;

namespace Tests.ConsentPortal.UnitTests.Pages;

public abstract class PageModelTestBase
{
    protected static ISession Session = new FakeHttpSession();

    [SetUp]
    public virtual void TestCaseSetup()
    {
        Session = new FakeHttpSession();
    }

    protected static PageContext CreatePageContext(HttpContext httpContext)
    {
        return new PageContext(new ActionContext(httpContext,
            new Microsoft.AspNetCore.Routing.RouteData(),
            new PageActionDescriptor(),
            new ModelStateDictionary()))
        {
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        };
    }

    protected static HttpContext CreateEmptyHttpContext()
    {
        Session.Clear();
        var httpContext = new DefaultHttpContext()
        {
            Session = Session
        };
        return httpContext;
    }

    protected static ConsentDetails ConsentDetails = new ConsentDetails
    {
        HasConsented = false,
        AppRefNumber = "App10101",
        InstallerName = "Test installer",
        TechTypeDescription = "Tech Type",
        Postcode = "X111XX",
        AddressLine1 = "Line 1"
    };
}
