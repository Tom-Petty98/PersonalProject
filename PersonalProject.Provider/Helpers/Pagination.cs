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
}