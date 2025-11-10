using Application.DTOs;
using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller MVC para gerenciar a interface web de Clientes
    /// </summary>
    public class ClientesWebController : Controller
    {
        private readonly ClienteService _clienteService;

        public ClientesWebController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Listar todos os clientes com paginação
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

                var result = await _clienteService.BuscarClientesAsync(pagination);

                var viewModel = new ClienteListViewModel
                {
                    Clientes = result.Items.Select(c => new ClienteItemViewModel
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        CPF = c.CPF,
                        Telefone = c.Telefone,
                        Email = c.Email,
                        Endereco = c.Endereco
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
                ViewBag.Error = $"Erro ao listar clientes: {ex.Message}";
                return View(new ClienteListViewModel());
            }
        }

        /// <summary>
        /// Exibir formulário para criar novo cliente
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ClienteFormViewModel());
        }

        /// <summary>
        /// Criar novo cliente
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var clienteDTO = new ClienteDTO
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    Telefone = model.Telefone,
                    Email = model.Email,
                    Endereco = model.Endereco
                };

                await _clienteService.CriarClienteAsync(clienteDTO);

                TempData["Success"] = "Cliente criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar cliente: {ex.Message}");
                return View(model);
            }
        }

        /// <summary>
        /// Exibir detalhes de um cliente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.ObterPorIdAsync(id);

                var viewModel = new ClienteItemViewModel
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone,
                    Email = cliente.Email,
                    Endereco = cliente.Endereco
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Exibir formulário para editar cliente
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var cliente = await _clienteService.ObterPorIdAsync(id);

                var viewModel = new ClienteFormViewModel
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    CPF = cliente.CPF,
                    Telefone = cliente.Telefone,
                    Email = cliente.Email,
                    Endereco = cliente.Endereco
                };

                return View("Create", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erro ao carregar cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Atualizar cliente existente
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ClienteFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", model);
                }

                var clienteDTO = new ClienteDTO
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    CPF = model.CPF,
                    Telefone = model.Telefone,
                    Email = model.Email,
                    Endereco = model.Endereco
                };

                await _clienteService.AtualizarClienteAsync(clienteDTO);

                TempData["Success"] = "Cliente atualizado com sucesso!";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar cliente: {ex.Message}");
                return View("Create", model);
            }
        }
    }
}
