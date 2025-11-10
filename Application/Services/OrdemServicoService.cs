using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    // Serviço de aplicação para gerenciar Ordens de Serviço
    public class OrdemServicoService : BaseService
    {
        private readonly IOrdemServicoRepository _ordemServicoRepository;
        private readonly IVeiculoRepository _veiculoRepository;

        public OrdemServicoService(
            IOrdemServicoRepository ordemServicoRepository,
            IVeiculoRepository veiculoRepository)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _veiculoRepository = veiculoRepository;
        }

        public async Task<OrdemServicoDTO> CriarOrdemServicoAsync(OrdemServicoDTO dto)
        {
            // Validar se o veículo existe
            var veiculo = await _veiculoRepository.GetByIdAsync(dto.VeiculoId);
            if (veiculo == null)
            {
                throw new Exceptions.BusinessException("Veículo não encontrado.");
            }

            // Validar valor
            if (dto.ValorTotal < 0)
            {
                throw new Exceptions.BusinessException("Valor total não pode ser negativo.");
            }

            // Determinar status (usar o fornecido ou Aberta por padrão)
            StatusOrdemServico status = StatusOrdemServico.Aberta;
            if (!string.IsNullOrEmpty(dto.Status))
            {
                if (Enum.TryParse<StatusOrdemServico>(dto.Status, out var statusParsed))
                {
                    status = statusParsed;
                }
            }

            // Criar entidade
            var ordemServico = new OrdemServico
            {
                DataAbertura = DateTime.Now,
                Descricao = dto.Descricao ?? string.Empty,
                Status = status,
                VeiculoId = dto.VeiculoId,
                ValorTotal = dto.ValorTotal,
                Observacoes = dto.Observacoes ?? string.Empty
            };

            // Se status é Concluida, definir data de fechamento
            if (status == StatusOrdemServico.Concluida)
            {
                ordemServico.DataFechamento = DateTime.Now;
            }

            // Salvar
            var resultado = await _ordemServicoRepository.AddAsync(ordemServico);

            // Retornar DTO
            return new OrdemServicoDTO
            {
                Id = resultado.Id,
                DataAbertura = resultado.DataAbertura,
                DataFechamento = resultado.DataFechamento,
                Descricao = resultado.Descricao,
                Status = resultado.Status.ToString(),
                VeiculoId = resultado.VeiculoId,
                ValorTotal = resultado.ValorTotal,
                Observacoes = resultado.Observacoes ?? string.Empty,
                PlacaVeiculo = veiculo.Placa,
                ModeloVeiculo = veiculo.Modelo,
                Servicos = new List<ServicoDTO>()
            };
        }

        public async Task<OrdemServicoDTO> ObterPorIdAsync(int id)
        {
            var ordemServico = await _ordemServicoRepository.GetByIdAsync(id);
            if (ordemServico == null)
            {
                throw new Exceptions.BusinessException("Ordem de Serviço não encontrada.");
            }

            return ConverterParaDTO(ordemServico);
        }

        public async Task<OrdemServicoDTO> AtualizarOrdemServicoAsync(OrdemServicoDTO dto)
        {
            var ordemServico = await _ordemServicoRepository.GetByIdAsync(dto.Id);
            if (ordemServico == null)
            {
                throw new Exceptions.BusinessException("Ordem de Serviço não encontrada.");
            }

            // Validar se o veículo existe
            var veiculo = await _veiculoRepository.GetByIdAsync(dto.VeiculoId);
            if (veiculo == null)
            {
                throw new Exceptions.BusinessException("Veículo não encontrado.");
            }

            // Validar valor
            if (dto.ValorTotal < 0)
            {
                throw new Exceptions.BusinessException("Valor total não pode ser negativo.");
            }

            // Atualizar campos
            ordemServico.Descricao = dto.Descricao ?? ordemServico.Descricao;
            ordemServico.ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : ordemServico.ValorTotal;
            ordemServico.Observacoes = dto.Observacoes ?? ordemServico.Observacoes;
            ordemServico.VeiculoId = dto.VeiculoId;

            // Atualizar status se foi fornecido
            if (!string.IsNullOrEmpty(dto.Status))
            {
                if (Enum.TryParse<StatusOrdemServico>(dto.Status, out var statusParsed))
                {
                    ordemServico.Status = statusParsed;
                    
                    // Se status é Concluida e não tem data de fechamento, definir
                    if (statusParsed == StatusOrdemServico.Concluida && !ordemServico.DataFechamento.HasValue)
                    {
                        ordemServico.DataFechamento = DateTime.Now;
                    }
                }
            }

            // Salvar
            await _ordemServicoRepository.UpdateAsync(ordemServico);

            return ConverterParaDTO(ordemServico);
        }

        public async Task<IEnumerable<OrdemServicoDTO>> ListarTodasAsync()
        {
            var ordensServico = await _ordemServicoRepository.GetAllAsync();
            return ordensServico.Select(os => ConverterParaDTO(os));
        }

        public async Task<PagedResultDTO<OrdemServicoDTO>> BuscarOrdensServicoAsync(PaginationDTO paginationDTO)
        {
            var ordensServico = await _ordemServicoRepository.GetAllAsync();

            // Aplicar filtro de busca
            if (!string.IsNullOrWhiteSpace(paginationDTO.SearchTerm))
            {
                ordensServico = ordensServico.Where(os =>
                    os.Descricao.Contains(paginationDTO.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    os.Status.ToString().Contains(paginationDTO.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    os.Veiculo?.Placa.Contains(paginationDTO.SearchTerm, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
            }

            int totalCount = ordensServico.Count();

            // Aplicar ordenação
            var ordensOrdenadas = paginationDTO.OrderBy?.ToLower() switch
            {
                "dataabertura" => paginationDTO.Descending
                    ? ordensServico.OrderByDescending(os => os.DataAbertura).ToList()
                    : ordensServico.OrderBy(os => os.DataAbertura).ToList(),
                "status" => paginationDTO.Descending
                    ? ordensServico.OrderByDescending(os => os.Status).ToList()
                    : ordensServico.OrderBy(os => os.Status).ToList(),
                "valortotal" => paginationDTO.Descending
                    ? ordensServico.OrderByDescending(os => os.ValorTotal).ToList()
                    : ordensServico.OrderBy(os => os.ValorTotal).ToList(),
                _ => paginationDTO.Descending
                    ? ordensServico.OrderByDescending(os => os.Id).ToList()
                    : ordensServico.OrderBy(os => os.Id).ToList()
            };

            // Aplicar paginação
            var ordensPaginadas = ordensOrdenadas
                .Skip((paginationDTO.PageNumber - 1) * paginationDTO.PageSize)
                .Take(paginationDTO.PageSize)
                .ToList();

            var result = new PagedResultDTO<OrdemServicoDTO>
            {
                Items = ordensPaginadas.Select(os => ConverterParaDTO(os)).ToList(),
                TotalCount = totalCount,
                PageNumber = paginationDTO.PageNumber,
                PageSize = paginationDTO.PageSize
            };

            return result;
        }

        public async Task FecharOrdemServicoAsync(int id)
        {
            var ordemServico = await _ordemServicoRepository.GetByIdAsync(id);
            if (ordemServico == null)
            {
                throw new Exceptions.BusinessException("Ordem de Serviço não encontrada.");
            }

            ordemServico.Status = StatusOrdemServico.Concluida;
            ordemServico.DataFechamento = DateTime.Now;

            await _ordemServicoRepository.UpdateAsync(ordemServico);
        }

        private OrdemServicoDTO ConverterParaDTO(OrdemServico os)
        {
            return new OrdemServicoDTO
            {
                Id = os.Id,
                DataAbertura = os.DataAbertura,
                DataFechamento = os.DataFechamento,
                Descricao = os.Descricao,
                Status = os.Status.ToString(),
                ValorTotal = os.ValorTotal,
                Observacoes = os.Observacoes ?? string.Empty,
                VeiculoId = os.VeiculoId,
                PlacaVeiculo = os.Veiculo?.Placa ?? string.Empty,
                ModeloVeiculo = os.Veiculo?.Modelo ?? string.Empty,
                Servicos = new List<ServicoDTO>()
            };
        }
    }
}
