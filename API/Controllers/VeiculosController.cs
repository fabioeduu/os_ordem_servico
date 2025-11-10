using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers

{


    // Controller para gerenciar Veículos

    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly VeiculoService _veiculoService;

        public VeiculosController(VeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }

        /// <summary>
        /// Buscar veículos com paginação, filtros e ordenação
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<PagedResultDTO<VeiculoDTO>>> Search(
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

                var result = await _veiculoService.BuscarVeiculosAsync(pagination);

                // Adicionar links HATEOAS
                result.Links = new List<LinkDTO>
                {
                    new() { Rel = "self", Href = $"/api/veiculos/search?pageNumber={pageNumber}&pageSize={pageSize}", Method = "GET" },
                    new() { Rel = "first", Href = $"/api/veiculos/search?pageNumber=1&pageSize={pageSize}", Method = "GET" }
                };

                if (result.HasNextPage)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "next", 
                        Href = $"/api/veiculos/search?pageNumber={pageNumber + 1}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                if (result.HasPreviousPage)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "previous", 
                        Href = $"/api/veiculos/search?pageNumber={pageNumber - 1}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                if (result.TotalPages > 0)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "last", 
                        Href = $"/api/veiculos/search?pageNumber={result.TotalPages}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar veículos", erro = ex.Message });
            }
        }



        // Listar todos os veículos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoDTO>>> GetAll()
        {
            try
            {
                var veiculos = await _veiculoService.ListarTodosAsync();
                return Ok(veiculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar veículos", erro = ex.Message });
            }
        }

        // Buscar veículo por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoDTO>> GetById(int id)
        {
            try
            {
                var veiculo = await _veiculoService.ObterPorIdAsync(id);
                return Ok(veiculo);
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar veículo", erro = ex.Message });
            }
        }



        // Criar novo veículo
        
        [HttpPost]
        public async Task<ActionResult<VeiculoDTO>> Create([FromBody] VeiculoDTO veiculoDTO)
        {
            try
            {
                var veiculo = await _veiculoService.CriarVeiculoAsync(veiculoDTO);
                return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao criar veículo", erro = ex.Message });
            }
        }
    }
}
