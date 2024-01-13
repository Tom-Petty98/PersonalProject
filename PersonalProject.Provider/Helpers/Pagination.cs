using PersonalProject.Domain.Request;
using Microsoft.EntityFrameworkCore;
using PersonalProject.Domain.Entities;

namespace PersonalProject.Provider.Helpers;
public static class Pagination
{
    public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, 
        int page, int pageSize) where T : class
    {
        var result = new PagedResult<T>
        {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = await query.CountAsync()
        };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (page - 1) * pageSize;

        result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

        return result;
    }

    private static IQueryable<ApplicationDashboard> ApplicationSearchBy(string searchBy, IQueryable<ApplicationDashboard> applications)
    {
        if(!string.IsNullOrEmpty(searchBy) && searchBy.Length >= 3)
        {
            applications = applications.Where(c => (!string.IsNullOrEmpty(c.Postcode) && c.Postcode.Contains(searchBy))
               || (!string.IsNullOrEmpty(c.RefNumber)) && c.RefNumber.Contains(searchBy));
        }

        return applications;
    }

    public static IOrderedQueryable<ApplicationDashboard> ApplicationsSortBy(string sortBy, bool orderByDescending,
        IQueryable<ApplicationDashboard> applications)
    {
        IOrderedQueryable<ApplicationDashboard> sortedApplications = sortBy switch
        {
            "PostCode" => orderByDescending
            ? applications.OrderByDescending(x => x.Postcode)
            : applications.OrderBy(x => x.Postcode),

            "RefNumber" => orderByDescending
            ? applications.OrderByDescending(x => x.RefNumber)
            : applications.OrderBy(x => x.RefNumber),

            "StatusDescription" => orderByDescending
            ? applications.OrderByDescending(x => x.StatusDescription)
            : applications.OrderBy(x => x.StatusDescription),

            "FlaggedForAudit" => orderByDescending
            ? applications.OrderByDescending(x => x.FlaggedForAudit)
            : applications.OrderBy(x => x.FlaggedForAudit),

            _ => applications.OrderByDescending(x => x.RefNumber)
        };

        return sortedApplications;
    }
}