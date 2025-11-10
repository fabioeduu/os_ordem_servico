using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    // Controller para gerenciar Clientes
    
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Buscar clientes com paginação, filtros e ordenação
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<PagedResultDTO<ClienteDTO>>> Search(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string orderBy = "Id",
            [FromQuery] bool descending = false,
            [FromQuery] string searchTerm = "")
        {
            try
            {
                var pagination = new PaginationDTO
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Descending = descending,
                    SearchTerm = searchTerm
                };

                var result = await _clienteService.BuscarClientesAsync(pagination);

                // Adicionar links HATEOAS
                result.Links = new List<LinkDTO>
                {
                    new() { Rel = "self", Href = $"/api/clientes/search?pageNumber={pageNumber}&pageSize={pageSize}", Method = "GET" },
                    new() { Rel = "first", Href = $"/api/clientes/search?pageNumber=1&pageSize={pageSize}", Method = "GET" }
                };

                if (result.HasNextPage)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "next", 
                        Href = $"/api/clientes/search?pageNumber={pageNumber + 1}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                if (result.HasPreviousPage)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "previous", 
                        Href = $"/api/clientes/search?pageNumber={pageNumber - 1}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                if (result.TotalPages > 0)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "last", 
                        Href = $"/api/clientes/search?pageNumber={result.TotalPages}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar clientes", erro = ex.Message });
            }
        }




        // Listar todos os clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
        {
            try
            {
                var clientes = await _clienteService.ListarTodosAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar clientes", erro = ex.Message });
            }
        }

        // Buscar cliente por ID

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetById(int id)
        {
            try
            {
                var cliente = await _clienteService.ObterPorIdAsync(id);
                return Ok(cliente);
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar cliente", erro = ex.Message });
            }
        }



        // Criar novo cliente

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> Create([FromBody] ClienteDTO clienteDTO)
        {
            try
            {
                var cliente = await _clienteService.CriarClienteAsync(clienteDTO);
                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao criar cliente", erro = ex.Message });
            }
        }
    }
}
