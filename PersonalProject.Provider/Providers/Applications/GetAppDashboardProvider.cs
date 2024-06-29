using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;
using PersonalProject.Domain.Request;
using PersonalProject.Provider.Helpers;

namespace PersonalProject.Provider.Providers.Applications;

public interface IGetAppDashboardProvider
{
    Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter);
}

internal class GetAppDashboardProvider : IGetAppDashboardProvider
{
    private readonly ApplicationDbContext _context;
    public GetAppDashboardProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ApplicationDashboard>> GetPagedApplications(DashboardFilter dashboardFilter)
    {
        IQueryable<ApplicationDashboard>? applications = _context.ApplicationDashboards.AsQueryable();

        if (!applications.Any()) return new();

        applications = FilterByStatusCode(dashboardFilter.FilteredAppStatuses, applications);
        applications = SearchBy(dashboardFilter.SearchBy, applications);

        IOrderedQueryable<ApplicationDashboard> sortedDashboards = SortBy(dashboardFilter.SortBy, dashboardFilter.OrderByDescending, applications);

        return await sortedDashboards.GetPagedAsync(dashboardFilter.PageNum, dashboardFilter.PageSize);
    }

    private static IQueryable<ApplicationDashboard> FilterByStatusCode(List<string>? filteredAppStatuses, IQueryable<ApplicationDashboard> applications)
    {
        if (filteredAppStatuses != null)
        {
            return applications.Where(x => filteredAppStatuses.Contains(x.StatusCode!));
        }

        return applications;
    }

    private static IQueryable<ApplicationDashboard> SearchBy(string seacrchBy, IQueryable<ApplicationDashboard> applications)
    {
        if (seacrchBy.Length >= 3)
        {
            applications = applications.Where(x => x.RefNumber!.Contains(seacrchBy) || x.Postcode!.Contains(seacrchBy));
        }
        return applications;
    }

    private static IOrderedQueryable<ApplicationDashboard> SortBy(string sortBy, bool orderByDescending, IQueryable<ApplicationDashboard> applications)
    {
        return sortBy switch
        {
            "RefNumber" => orderByDescending
                ? applications.OrderByDescending(x => x.RefNumber)
                : applications.OrderBy(x => x.RefNumber),

            "Postcode" => orderByDescending
                ? applications.OrderByDescending(x => x.Postcode)
                : applications.OrderBy(x => x.Postcode),

            "StatusDescription" => orderByDescending
            ? applications.OrderByDescending(x => x.StatusDescription)
            : applications.OrderBy(x => x.StatusDescription),

            "ReviewRecommendation" => orderByDescending
                ? applications.OrderByDescending(x => x.ReviewRecommendation)
                : applications.OrderBy(x => x.ReviewRecommendation),

            "FlaggedForAudit" => orderByDescending
            ? applications.OrderByDescending(x => x.FlaggedForAudit)
            : applications.OrderBy(x => x.FlaggedForAudit),

            "LastStatusChangeDate" => orderByDescending
                ? applications.OrderByDescending(x => x.LastStatusChangeDate)
                : applications.OrderBy(x => x.LastStatusChangeDate),

            _ => applications.OrderByDescending(x => x.RefNumber)
        };
    }  
}
