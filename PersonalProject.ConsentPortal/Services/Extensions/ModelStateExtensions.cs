using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PersonalProject.ConsentPortal.Services.Extensions;

public static class ModelStateExtensions
{
    public static bool HasError(this ModelStateDictionary modelState, string key)
    {
        if (modelState[key]!.Errors != null && modelState[key]!.Errors.Any())
        {
            return true;
        }

        return false;
    }
}
