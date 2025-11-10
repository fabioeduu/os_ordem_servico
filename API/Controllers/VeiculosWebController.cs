using Application.DTOs;
using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller MVC para gerenciar a interface web de Veículos
    /// </summary>
    public class VeiculosWebController : Controller
    {
        private readonly VeiculoService _veiculoService;
        private readonly ClienteService _clienteService;

        public VeiculosWebController(VeiculoService veiculoService, ClienteService clienteService)
        {
            _veiculoService = veiculoService;
            _clienteService = clienteService;
        }

        /// <summary>
        /// Listar todos os veículos com paginação
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

                var result = await _veiculoService.BuscarVeiculosAsync(pagination);

                var viewModel = new VeiculoListViewModel
                {
                    Veiculos = result.Items.Select(v => new VeiculoItemViewModel
                    {
                        Id = v.Id,
                        Placa = v.Placa,
                        Marca = v.Marca,
                        Modelo = v.Modelo,
                        Ano = v.Ano,
                        Cor = v.Cor,
                        ClienteId = v.ClienteId,
                        NomeCliente = v.NomeCliente
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
                ViewBag.Error = $"Erro ao listar veículos: {ex.Message}";
                return View(new VeiculoListViewModel());
            }
        }

        /// <summary>
        /// Exibir formulário para criar novo veículo
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var clientes = await _clienteService.ListarTodosAsync();
                var model = new VeiculoFormViewModel
                {
                    Clientes = clientes.Select(c => new ClienteSelectViewModel
                    {
                        Id = c.Id,
                        Nome = c.Nome
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
        /// Criar novo veículo
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VeiculoFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var clientes = await _clienteService.ListarTodosAsync();
                    model.Clientes = clientes.Select(c => new ClienteSelectViewModel
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList();
                    return View(model);
                }

                var veiculoDTO = new VeiculoDTO
                {
                    Placa = model.Placa,
                    Marca = model.Marca,
                    Modelo = model.Modelo,
                    Ano = model.Ano,
                    Cor = model.Cor,
                    ClienteId = model.ClienteId
                };

                await _veiculoService.CriarVeiculoAsync(veiculoDTO);

                TempData["Success"] = "Veículo criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar veículo: {ex.Message}");
                var clientes = await _clienteService.ListarTodosAsync();
                model.Clientes = clientes.Select(c => new ClienteSelectViewModel
                {
                    Id = c.Id,
                    Nome = c.Nome
                }).ToList();
                return View(model);
            }
        }

        /// <summary>
        /// Exibir detalhes de um veículo
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var veiculo = await _veiculoService.ObterPorIdAsync(id);

                var viewModel = new VeiculoItemViewModel
                {
                    Id = veiculo.Id,
                    Placa = veiculo.Placa,
                    Marca = veiculo.Marca,
                    Modelo = veiculo.Modelo,
                    Ano = veiculo.Ano,
                    Cor = veiculo.Cor,
                    ClienteId = veiculo.ClienteId,
                    NomeCliente = veiculo.NomeCliente
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar veículo: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Exibir formulário para editar veículo
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var veiculo = await _veiculoService.ObterPorIdAsync(id);
                var clientes = await _clienteService.ListarTodosAsync();

                var viewModel = new VeiculoFormViewModel
                {
                    Id = veiculo.Id,
                    Placa = veiculo.Placa,
                    Marca = veiculo.Marca,
                    Modelo = veiculo.Modelo,
                    Ano = veiculo.Ano,
                    Cor = veiculo.Cor,
                    ClienteId = veiculo.ClienteId,
                    Clientes = clientes.Select(c => new ClienteSelectViewModel
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList()
                };

                return View("Create", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar veículo: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Atualizar veículo existente
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VeiculoFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var clientes = await _clienteService.ListarTodosAsync();
                    model.Clientes = clientes.Select(c => new ClienteSelectViewModel
                    {
                        Id = c.Id,
                        Nome = c.Nome
                    }).ToList();
                    return View("Create", model);
                }

                var veiculoDTO = new VeiculoDTO
                {
                    Id = model.Id,
                    Placa = model.Placa,
                    Marca = model.Marca,
                    Modelo = model.Modelo,
                    Ano = model.Ano,
                    Cor = model.Cor,
                    ClienteId = model.ClienteId
                };

                await _veiculoService.AtualizarVeiculoAsync(veiculoDTO);

                TempData["Success"] = "Veículo atualizado com sucesso!";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar veículo: {ex.Message}");
                var clientes = await _clienteService.ListarTodosAsync();
                model.Clientes = clientes.Select(c => new ClienteSelectViewModel
                {
                    Id = c.Id,
                    Nome = c.Nome
                }).ToList();
                return View("Create", model);
            }
        }
    }
}
