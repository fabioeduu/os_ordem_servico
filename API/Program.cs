using Application.Services;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("OrdemServicoDb"));

// Repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IVeiculoRepository, VeiculoRepository>();
builder.Services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();

// Serviços
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<VeiculoService>();
builder.Services.AddScoped<OrdemServicoService>();

// MVC
builder.Services.AddControllersWithViews();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

// Seeder
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        if (context.Clientes.Count() == 0)
        {
            var cliente1 = new Cliente { Nome = "João Silva", CPF = "12345678901", Telefone = "11999999999", Email = "joao@test.com", Endereco = "Rua Teste, 123", Veiculos = new List<Veiculo>() };
            var cliente2 = new Cliente { Nome = "Maria Santos", CPF = "98765432109", Telefone = "11988888888", Email = "maria@test.com", Endereco = "Av. Exemplo, 456", Veiculos = new List<Veiculo>() };
            
            context.Clientes.Add(cliente1);
            context.Clientes.Add(cliente2);
            context.SaveChanges();
            
            var veiculo1 = new Veiculo { Placa = "ABC1234", Marca = "Toyota", Modelo = "Corolla", Ano = 2022, Cor = "Branco", ClienteId = cliente1.Id, OrdensServico = new List<OrdemServico>() };
            var veiculo2 = new Veiculo { Placa = "XYZ5678", Marca = "Honda", Modelo = "Civic", Ano = 2021, Cor = "Preto", ClienteId = cliente2.Id, OrdensServico = new List<OrdemServico>() };
            
            context.Veiculos.Add(veiculo1);
            context.Veiculos.Add(veiculo2);
            context.SaveChanges();
            
            var ordem1 = new OrdemServico { Descricao = "Revisão completa", DataAbertura = DateTime.Now, DataFechamento = null, Status = StatusOrdemServico.Aguardando, ValorTotal = 450.00m, Observacoes = "Cliente aguarda orçamento", VeiculoId = veiculo1.Id, Servicos = new List<Servico>() };
            var ordem2 = new OrdemServico { Descricao = "Troca de óleo e filtro", DataAbertura = DateTime.Now.AddDays(-1), DataFechamento = DateTime.Now, Status = StatusOrdemServico.Concluida, ValorTotal = 120.00m, Observacoes = "Serviço realizado com sucesso", VeiculoId = veiculo2.Id, Servicos = new List<Servico>() };
            
            context.OrdensServico.Add(ordem1);
            context.OrdensServico.Add(ordem2);
            context.SaveChanges();
            
            Console.WriteLine("✅ Dados iniciais criados");
        }
    }
}
catch { }

app.Run();