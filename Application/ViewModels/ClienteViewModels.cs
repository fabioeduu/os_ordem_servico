namespace Application.ViewModels
{
    /// <summary>
    /// ViewModel para listar clientes com paginação
    /// </summary>
    public class ClienteListViewModel
    {
        public List<ClienteItemViewModel> Clientes { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public string SearchTerm { get; set; } = "";
        public string OrderBy { get; set; } = "Id";
        public bool Descending { get; set; } = false;
    }

    /// <summary>
    /// ViewModel para item de cliente na lista
    /// </summary>
    public class ClienteItemViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
    }

    /// <summary>
    /// ViewModel para criar/editar cliente
    /// </summary>
    public class ClienteFormViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public bool IsEdit => Id > 0;
    }
}
