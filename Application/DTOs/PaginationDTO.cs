namespace Application.DTOs
{
    /// <summary>
    /// DTO para encapsular dados de paginação
    /// </summary>
    public class PaginationDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "Id";
        public bool Descending { get; set; } = false;
        public string SearchTerm { get; set; } = "";
    }

    /// <summary>
    /// DTO para retornar dados paginados
    /// </summary>
    public class PagedResultDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public List<LinkDTO> Links { get; set; } = new();
    }

    /// <summary>
    /// DTO para representar links HATEOAS
    /// </summary>
    public class LinkDTO
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; }
    }
}
