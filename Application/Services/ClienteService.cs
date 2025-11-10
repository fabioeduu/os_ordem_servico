using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    // Serviço de aplicação para gerenciar Clientes
    public class ClienteService : BaseService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteDTO> CriarClienteAsync(ClienteDTO dto)
        {
            // Validar se CPF já existe
            var clienteExistente = await _clienteRepository.GetByCPFAsync(dto.CPF);
            if (clienteExistente != null)
            {
                throw new Exceptions.BusinessException("Já existe um cliente com este CPF.");
            }

            // Criar entidade
            var cliente = new Cliente
            {
                Nome = dto.Nome ?? string.Empty,
                CPF = dto.CPF ?? string.Empty,
                Telefone = dto.Telefone ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Endereco = dto.Endereco ?? string.Empty
            };

            // Salvar
            var resultado = await _clienteRepository.AddAsync(cliente);

            // Retornar DTO
            return new ClienteDTO
            {
                Id = resultado.Id,
                Nome = resultado.Nome,
                CPF = resultado.CPF,
                Telefone = resultado.Telefone,
                Email = resultado.Email,
                Endereco = resultado.Endereco
            };
        }

        public async Task<ClienteDTO> ObterPorIdAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                throw new Exceptions.BusinessException("Cliente não encontrado.");
            }

            return ConverterParaDTO(cliente);
        }

        public async Task<ClienteDTO> AtualizarClienteAsync(ClienteDTO dto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(dto.Id);
            if (cliente == null)
            {
                throw new Exceptions.BusinessException("Cliente não encontrado.");
            }

            // Atualizar campos
            cliente.Nome = dto.Nome ?? cliente.Nome;
            cliente.CPF = dto.CPF ?? cliente.CPF;
            cliente.Telefone = dto.Telefone ?? cliente.Telefone;
            cliente.Email = dto.Email ?? cliente.Email;
            cliente.Endereco = dto.Endereco ?? cliente.Endereco;

            // Salvar
            await _clienteRepository.UpdateAsync(cliente);

            return ConverterParaDTO(cliente);
        }

        public async Task<IEnumerable<ClienteDTO>> ListarTodosAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return clientes.Select(c => ConverterParaDTO(c));
        }

        public async Task<PagedResultDTO<ClienteDTO>> BuscarClientesAsync(PaginationDTO paginationDTO)
        {
            var clientes = await _clienteRepository.GetAllAsync();

            // Aplicar filtro de busca
            if (!string.IsNullOrWhiteSpace(paginationDTO.SearchTerm))
            {
                clientes = clientes.Where(c =>
                    c.Nome.Contains(paginationDTO.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.CPF.Contains(paginationDTO.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(paginationDTO.SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            int totalCount = clientes.Count();

            // Aplicar ordenação
            var clientesOrdenados = paginationDTO.OrderBy?.ToLower() switch
            {
                "nome" => paginationDTO.Descending
                    ? clientes.OrderByDescending(c => c.Nome).ToList()
                    : clientes.OrderBy(c => c.Nome).ToList(),
                "cpf" => paginationDTO.Descending
                    ? clientes.OrderByDescending(c => c.CPF).ToList()
                    : clientes.OrderBy(c => c.CPF).ToList(),
                _ => paginationDTO.Descending
                    ? clientes.OrderByDescending(c => c.Id).ToList()
                    : clientes.OrderBy(c => c.Id).ToList()
            };

            // Aplicar paginação
            var clientesPaginados = clientesOrdenados
                .Skip((paginationDTO.PageNumber - 1) * paginationDTO.PageSize)
                .Take(paginationDTO.PageSize)
                .ToList();

            var result = new PagedResultDTO<ClienteDTO>
            {
                Items = clientesPaginados.Select(c => ConverterParaDTO(c)).ToList(),
                TotalCount = totalCount,
                PageNumber = paginationDTO.PageNumber,
                PageSize = paginationDTO.PageSize
            };

            return result;
        }

        private ClienteDTO ConverterParaDTO(Cliente cliente)
        {
            return new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                Telefone = cliente.Telefone,
                Email = cliente.Email,
                Endereco = cliente.Endereco
            };
        }
    }
}
