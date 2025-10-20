namespace MusicBackendApp.Application.Common.Parameters;

public class SortParams
{
    public string? OrderBy { get; set; } // Назва властивості для сортування (наприклад, "Name", "DateCreated")
    public SortVeriable? Veriable { get; set; } // Вибір за написанням(плелисти, подкасти, треки і т.д.)
}