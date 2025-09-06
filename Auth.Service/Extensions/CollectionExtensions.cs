using Auth.Domain.Configurations;

namespace Auth.Service.Extensions;

public static class CollectionExtensions
{
    public static async Task<PagedResult<T>> ToPagedListAsync<T>(
       this IQueryable<T> source,
       PaginationParams @params)
    {
        // Parametrlarni tekshirish (0 yoki manfiy bo‘lsa – qabul qilmaslik yoki default o‘rnatish)
        if (@params.PageIndex <= 0) @params.PageIndex = 1;
        if (@params.PageSize <= 0) @params.PageSize = 10; // yoki xohlagan default

        // 1) Umumiy elementlar soni
        var totalItems = source.Count();

        // 2) Sahifadagi elementlarni skip/take orqali ajratib olish
        var items = source
            .Skip((@params.PageIndex - 1) * @params.PageSize)
            .Take(@params.PageSize)
            .ToList();

        // 3) Umumiy sahifalar soni
        var totalPages = (int)Math.Ceiling(
            totalItems / (double)@params.PageSize
        );

        // 4) PagedResult obyektini to‘ldirib qaytaramiz
        return new PagedResult<T>
        {
            Data = items,
            TotalItems = totalItems,
            TotalPages = totalPages,
            CurrentPage = @params.PageIndex,
            PageSize = @params.PageSize
        };
    }
}
