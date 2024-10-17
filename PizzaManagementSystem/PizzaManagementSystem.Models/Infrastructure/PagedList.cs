using Microsoft.EntityFrameworkCore;

namespace PizzaManagementSystem.Models.Infrastructure;

public class PagedList<T>(List<T> items, int count, int pageNumber, int pageSize)
{
    public int CurrentPage { get; set; } = pageNumber;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = count;
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public List<T> Items { get; set; } = items;

    public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public PagedList<TK> Select<TK>(Func<T, TK> projection)
    {
        return new PagedList<TK>(this.Items.Select(projection).ToList(), this.TotalCount, this.CurrentPage, this.PageSize);
    }
}