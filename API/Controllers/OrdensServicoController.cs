using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{



    // Controller para gerenciar Ordens de Serviço
    [ApiController]
    [Route("api/[controller]")]
    public class OrdensServicoController : ControllerBase
    {
        private readonly OrdemServicoService _ordemServicoService;

        public OrdensServicoController(OrdemServicoService ordemServicoService)
        {
            _ordemServicoService = ordemServicoService;
        }

        /// <summary>
        /// Buscar ordens de serviço com paginação, filtros e ordenação
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<PagedResultDTO<OrdemServicoDTO>>> Search(
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

                var result = await _ordemServicoService.BuscarOrdensServicoAsync(pagination);

                // Adicionar links HATEOAS
                result.Links = new List<LinkDTO>
                {
                    new() { Rel = "self", Href = $"/api/ordensservico/search?pageNumber={pageNumber}&pageSize={pageSize}", Method = "GET" },
                    new() { Rel = "first", Href = $"/api/ordensservico/search?pageNumber=1&pageSize={pageSize}", Method = "GET" }
                };

                if (result.HasNextPage)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "next", 
                        Href = $"/api/ordensservico/search?pageNumber={pageNumber + 1}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                if (result.HasPreviousPage)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "previous", 
                        Href = $"/api/ordensservico/search?pageNumber={pageNumber - 1}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                if (result.TotalPages > 0)
                {
                    result.Links.Add(new() 
                    { 
                        Rel = "last", 
                        Href = $"/api/ordensservico/search?pageNumber={result.TotalPages}&pageSize={pageSize}", 
                        Method = "GET" 
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar ordens de serviço", erro = ex.Message });
            }
        }



        // Listar todas as ordens de serviço
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdemServicoDTO>>> GetAll()
        {
            try
            {
                var ordensServico = await _ordemServicoService.ListarTodasAsync();
                return Ok(ordensServico);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao listar ordens de serviço", erro = ex.Message });
            }
        }




        // Buscar ordem de serviço por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdemServicoDTO>> GetById(int id)
        {
            try
            {
                var ordemServico = await _ordemServicoService.ObterPorIdAsync(id);
                return Ok(ordemServico);
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar ordem de serviço", erro = ex.Message });
            }
        }

        
        

        // Criar nova ordem de serviço
        
        [HttpPost]
        public async Task<ActionResult<OrdemServicoDTO>> Create([FromBody] OrdemServicoDTO ordemServicoDTO)
        {
            try
            {
                var ordemServico = await _ordemServicoService.CriarOrdemServicoAsync(ordemServicoDTO);
                return CreatedAtAction(nameof(GetById), new { id = ordemServico.Id }, ordemServico);
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao criar ordem de serviço", erro = ex.Message });
            }
        }

        // Fechar ordem de serviço
        [HttpPut("{id}/fechar")]
        public async Task<ActionResult> FecharOrdemServico(int id)
        {
            try
            {
                await _ordemServicoService.FecharOrdemServicoAsync(id);
                return Ok(new { mensagem = "Ordem de serviço fechada com sucesso" });
            }
            catch (Application.Exceptions.BusinessException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao fechar ordem de serviço", erro = ex.Message });
            }
        }
    }
}
