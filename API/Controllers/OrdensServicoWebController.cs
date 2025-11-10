using Application.DTOs;
using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller MVC para gerenciar a interface web de Ordens de Serviço
    /// </summary>
    public class OrdensServicoWebController : Controller
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly VeiculoService _veiculoService;

        public OrdensServicoWebController(OrdemServicoService ordemServicoService, VeiculoService veiculoService)
        {
            _ordemServicoService = ordemServicoService;
            _veiculoService = veiculoService;
        }

        /// <summary>
        /// Listar todas as ordens de serviço com paginação
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchTerm = "", string orderBy = "Id", bool descending = false)
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

                var viewModel = new OrdemServicoListViewModel
                {
                    OrdensServico = result.Items.Select(os => new OrdemServicoItemViewModel
                    {
                        Id = os.Id,
                        DataAbertura = os.DataAbertura,
                        DataFechamento = os.DataFechamento,
                        Descricao = os.Descricao,
                        Status = os.Status,
                        ValorTotal = os.ValorTotal,
                        VeiculoId = os.VeiculoId,
                        PlacaVeiculo = os.PlacaVeiculo,
                        ModeloVeiculo = os.ModeloVeiculo
                    }).ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = result.TotalCount,
                    SearchTerm = searchTerm,
                    OrderBy = orderBy,
                    Descending = descending
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao listar ordens de serviço: {ex.Message}";
                return View(new OrdemServicoListViewModel());
            }
        }

        /// <summary>
        /// Exibir formulário para criar nova ordem de serviço
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var veiculos = await _veiculoService.ListarTodosAsync();
                var model = new OrdemServicoFormViewModel
                {
                    Veiculos = veiculos.Select(v => new VeiculoSelectViewModel
                    {
                        Id = v.Id,
                        Placa = v.Placa,
                        Modelo = v.Modelo
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar formulário: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Criar nova ordem de serviço
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdemServicoFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var veiculos = await _veiculoService.ListarTodosAsync();
                    model.Veiculos = veiculos.Select(v => new VeiculoSelectViewModel
                    {
                        Id = v.Id,
                        Placa = v.Placa,
                        Modelo = v.Modelo
                    }).ToList();
                    return View(model);
                }

                var ordemDTO = new OrdemServicoDTO
                {
                    Descricao = model.Descricao,
                    Status = model.Status,
                    VeiculoId = model.VeiculoId,
                    ValorTotal = model.ValorTotal,
                    Observacoes = model.Observacoes
                };

                await _ordemServicoService.CriarOrdemServicoAsync(ordemDTO);

                TempData["Success"] = "Ordem de serviço criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar ordem de serviço: {ex.Message}");
                var veiculos = await _veiculoService.ListarTodosAsync();
                model.Veiculos = veiculos.Select(v => new VeiculoSelectViewModel
                {
                    Id = v.Id,
                    Placa = v.Placa,
                    Modelo = v.Modelo
                }).ToList();
                return View(model);
            }
        }

        /// <summary>
        /// Exibir detalhes de uma ordem de serviço
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ordemServico = await _ordemServicoService.ObterPorIdAsync(id);

                var viewModel = new OrdemServicoItemViewModel
                {
                    Id = ordemServico.Id,
                    DataAbertura = ordemServico.DataAbertura,
                    DataFechamento = ordemServico.DataFechamento,
                    Descricao = ordemServico.Descricao,
                    Status = ordemServico.Status,
                    ValorTotal = ordemServico.ValorTotal,
                    VeiculoId = ordemServico.VeiculoId,
                    PlacaVeiculo = ordemServico.PlacaVeiculo,
                    ModeloVeiculo = ordemServico.ModeloVeiculo
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar ordem de serviço: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Exibir formulário para editar ordem de serviço
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var ordemServico = await _ordemServicoService.ObterPorIdAsync(id);
                var veiculos = await _veiculoService.ListarTodosAsync();

                var viewModel = new OrdemServicoFormViewModel
                {
                    Id = ordemServico.Id,
                    Descricao = ordemServico.Descricao,
                    Status = ordemServico.Status,
                    ValorTotal = ordemServico.ValorTotal,
                    Observacoes = ordemServico.Observacoes,
                    VeiculoId = ordemServico.VeiculoId,
                    Veiculos = veiculos.Select(v => new VeiculoSelectViewModel
                    {
                        Id = v.Id,
                        Placa = v.Placa,
                        Modelo = v.Modelo
                    }).ToList()
                };

                return View("Create", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar ordem de serviço: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Atualizar ordem de serviço existente
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(OrdemServicoFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var veiculos = await _veiculoService.ListarTodosAsync();
                    model.Veiculos = veiculos.Select(v => new VeiculoSelectViewModel
                    {
                        Id = v.Id,
                        Placa = v.Placa,
                        Modelo = v.Modelo
                    }).ToList();
                    return View("Create", model);
                }

                var ordemServicoDTO = new OrdemServicoDTO
                {
                    Id = model.Id,
                    Descricao = model.Descricao,
                    Status = model.Status,
                    ValorTotal = model.ValorTotal,
                    Observacoes = model.Observacoes,
                    VeiculoId = model.VeiculoId
                };

                await _ordemServicoService.AtualizarOrdemServicoAsync(ordemServicoDTO);

                TempData["Success"] = "Ordem de serviço atualizada com sucesso!";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar ordem de serviço: {ex.Message}");
                var veiculos = await _veiculoService.ListarTodosAsync();
                model.Veiculos = veiculos.Select(v => new VeiculoSelectViewModel
                {
                    Id = v.Id,
                    Placa = v.Placa,
                    Modelo = v.Modelo
                }).ToList();
                return View("Create", model);
            }
        }

        /// <summary>
        /// Fechar uma ordem de serviço
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fechar(int id)
        {
            try
            {
                await _ordemServicoService.FecharOrdemServicoAsync(id);
                TempData["Success"] = "Ordem de serviço fechada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao fechar ordem de serviço: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}
