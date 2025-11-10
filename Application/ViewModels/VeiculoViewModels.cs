namespace Application.ViewModels
{
    /// <summary>
    /// ViewModel para listar veículos com paginação
    /// </summary>
    public class VeiculoListViewModel
    {
        public List<VeiculoItemViewModel> Veiculos { get; set; } = new();
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
    /// ViewModel para item de veículo na lista
    /// </summary>
    public class VeiculoItemViewModel
    {
        public int Id { get; set; }
        public string? Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Ano { get; set; }
        public string? Cor { get; set; }
        public int ClienteId { get; set; }
        public string? NomeCliente { get; set; }
    }

    /// <summary>
    /// ViewModel para criar/editar veículo
    /// </summary>
    public class VeiculoFormViewModel
    {
        public int Id { get; set; }
        public string? Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Ano { get; set; }
        public string? Cor { get; set; }
        public int ClienteId { get; set; }
        public List<ClienteSelectViewModel> Clientes { get; set; } = new();
        public bool IsEdit => Id > 0;
    }

    /// <summary>
    /// ViewModel para seleção de cliente
    /// </summary>
    public class ClienteSelectViewModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
    }
}
