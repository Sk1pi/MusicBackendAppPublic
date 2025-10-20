namespace MusicBackendApp.Application.Common.Parameters;

public class PageParams
{
    public int? Page { get; set; } = 1; // Номер сторінки
    public int? PageSize { get; set; } = 10; // Розмір сторінки
}