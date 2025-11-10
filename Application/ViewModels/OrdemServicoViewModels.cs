namespace Application.ViewModels
{
    /// <summary>
    /// ViewModel para listar ordens de serviço com paginação
    /// </summary>
    public class OrdemServicoListViewModel
    {
        public List<OrdemServicoItemViewModel> OrdensServico { get; set; } = new();
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
    /// ViewModel para item de ordem de serviço na lista
    /// </summary>
    public class OrdemServicoItemViewModel
    {
        public int Id { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public decimal ValorTotal { get; set; }
        public int VeiculoId { get; set; }
        public string? PlacaVeiculo { get; set; }
        public string? ModeloVeiculo { get; set; }
    }

    /// <summary>
    /// ViewModel para criar/editar ordem de serviço
    /// </summary>
    public class OrdemServicoFormViewModel
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string Status { get; set; } = "Aberta";
        public int VeiculoId { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacoes { get; set; }
        public List<VeiculoSelectViewModel> Veiculos { get; set; } = new();
        public bool IsEdit => Id > 0;
    }

    /// <summary>
    /// ViewModel para seleção de veículo
    /// </summary>
    public class VeiculoSelectViewModel
    {
        public int Id { get; set; }
        public string? Placa { get; set; }
        public string? Modelo { get; set; }
        public string? PlacaModelo => $"{Placa} - {Modelo}";
    }
}
