using System.Text.RegularExpressions;

namespace PersonalProject.InternalPortal.Services.Helpers;

public class CommonValidationService
{
    public static string? DeformatPostcode(string postcode)
    {
        var deformattedPostcode = postcode.Replace(" ", "").ToUpper();

        bool isValid = Regex.Match(deformattedPostcode, "^(?i)[A-Z]{1,2}[0-9]{2,3}[ABD-HJLNP-UW-Z]{2}").Success;
        //bool isValid = Regex.Match(deformattedPostcode, "(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))) [0-9][A-Z-[CIKMOV]]{2})").Success;

        if (isValid) return deformattedPostcode;
        return null;
    }
}
