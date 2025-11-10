# Sistema de Ordem de Serviço Automotivo

## Definição do Projeto

### Objetivo do Projeto
Esclarecer o problema de gestão de ordens de serviço em oficinas automotivas através de um sistema digital que permite o controle completo do fluxo de trabalho, desde o cadastro do cliente até o fechamento da ordem de serviço.

### Escopo
Delimitar o que será desenvolvido e as funcionalidades principais:
- Cadastro e gerenciamento de clientes com CPF único
- Cadastro e gerenciamento de veículos vinculados a clientes
- Criação e acompanhamento de ordens de serviço
- Adição de serviços às ordens de serviço
- Controle de status das ordens (Aberta, Em Andamento, Aguardando, Concluída, Cancelada)
- Cálculo automático do valor total dos serviços
- Consulta de ordens de serviço por status, período ou veículo

## Demonstração do Sistema

---

### Terminal
![Tela do terminal](img/terminal.png)


---

### Tela Web
![Interface](img/interface.png)

---

### Cadastro de Cliente
![Tela de cadastro de cliente](img/cadastro_cliente.png)

---

### Cadastro de Veículo
![Tela de cadastro de veículo](img/cadastro_veiculo.png)

---

### Cadastro de Ordem de Serviço
![Tela de cadastro de ordem de serviço](img/cadastro_ordem_servico.png)

### Camadas da Aplicação

#### Aplicação
**Serviços e casos de uso da aplicação:**
- `ClienteService` - Gerenciamento de clientes (criar, consultar, listar)
- `VeiculoService` - Gerenciamento de veículos (criar, consultar, listar)
- `OrdemServicoService` - Gerenciamento de ordens de serviço (criar, consultar, fechar)
- `BaseService` - Lógica comum aos serviços

**DTOs**
- `ClienteDTO` - Transferência de dados de clientes
- `VeiculoDTO` - Transferência de dados de veículos
- `OrdemServicoDTO` - Transferência de dados de ordens de serviço
- `ServicoDTO` - Transferência de dados de serviços

**Manipulação de Erros:**
- `BusinessException` - Exceção customizada para erros de negócio com retorno de erros apropriados

#### Domínio
**Modelos e regras de negócio:**

**Entidades:**
- `Cliente` - Representa um cliente da oficina (Nome, CPF, Telefone, Email, Endereço)
- `Veiculo` - Representa um veículo (Placa, Marca, Modelo, Ano, Cor)
- `OrdemServico` - Representa uma ordem de serviço (Data Abertura/Fechamento, Descrição, Status, Valor Total)
- `Servico` - Representa um serviço realizado (Descrição, Valor, Tempo Estimado)
- `BaseEntity` - Classe base com Id e DataCriacao

**Interfaces de Repositório:**
- `IRepository<T>` - Interface genérica base para operações CRUD
- `IClienteRepository` - Métodos específicos para consulta de clientes
- `IVeiculoRepository` - Métodos específicos para consulta de veículos
- `IOrdemServicoRepository` - Métodos específicos para consulta de ordens de serviço

**Regras de Negócio:**
- CPF do cliente deve ser único
- Placa do veículo deve ser única
- Validação de dados obrigatórios
- Status da ordem de serviço controlado por enum
- Cálculo automático do valor total da ordem de serviço


## Tecnologias Utilizadas

- .NET 9.0
- C# 13
- Entity Framework Core 9.0.10 (In-Memory Database)
- ASP.NET Core MVC + Web API
- Bootstrap 5.3 CDN
- HTML5, CSS3, JavaScript ES6+
- Poppins Font (Google Fonts)

## Estrutura do Projeto

```
sprintC#/
├── API/
│   ├── Controllers/
│   │   ├── ClientesController.cs (API REST)
│   │   ├── VeiculosController.cs (API REST)
│   │   ├── OrdensServicoController.cs (API REST)
│   │   ├── ClientesWebController.cs (Web MVC)
│   │   ├── VeiculosWebController.cs (Web MVC)
│   │   ├── OrdensServicoWebController.cs (Web MVC)
│   │   └── HomeController.cs (Web MVC)
│   ├── Views/
│   │   ├── _Layout.cshtml (Layout principal)
│   │   ├── Home/Index.cshtml (Dashboard)
│   │   ├── ClientesWeb/ (Index, Create, Details, Edit)
│   │   ├── VeiculosWeb/ (Index, Create, Details, Edit)
│   │   ├── OrdensServicoWeb/ (Index, Create, Details, Edit)
│   │   └── Shared/ (_ViewImports, _ViewStart)
│   ├── Program.cs
│   └── API.csproj
│
├── Application/
│   ├── DTOs/
│   │   ├── BaseDTO.cs
│   │   ├── ClienteDTO.cs
│   │   ├── VeiculoDTO.cs
│   │   ├── OrdemServicoDTO.cs
│   │   ├── ServicoDTO.cs
│   │   └── PaginationDTO.cs
│   ├── Services/
│   │   ├── BaseService.cs
│   │   ├── ClienteService.cs
│   │   ├── VeiculoService.cs
│   │   └── OrdemServicoService.cs
│   ├── ViewModels/
│   │   ├── ClienteViewModels.cs
│   │   ├── VeiculoViewModels.cs
│   │   └── OrdemServicoViewModels.cs
│   ├── Exceptions/
│   │   └── BusinessException.cs
│   └── Application.csproj
│
├── Domain/
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── Cliente.cs
│   │   ├── Veiculo.cs
│   │   ├── OrdemServico.cs
│   │   ├── Servico.cs
│   │   └── PagedResultDTO.cs
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── IClienteRepository.cs
│   │   ├── IVeiculoRepository.cs
│   │   └── IOrdemServicoRepository.cs
│   └── Domain.csproj
│
├── Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs (Seed de dados)
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   ├── ClienteRepository.cs
│   │   ├── VeiculoRepository.cs
│   │   └── OrdemServicoRepository.cs
│   ├── ExternalServices/
│   │   └── ExternalApiClient.cs
│   └── Infrastructure.csproj
│
├── scripts/
│   ├── start-api.bat
│   ├── run-api.bat
│   └── start-server.ps1
│
└── AdvancedBusinessDevelopment.sln
```

## Como Executar

### Pré-requisitos
- .NET 9.0 SDK instalado
- Visual Studio 2022 ou VS Code

### Passos

1. **Clone ou abra o repositório**
   ```bash
   cd sprintC
   ```

2. **Restaure os pacotes NuGet**
   ```bash
   dotnet restore
   ```

3. **Execute a API e Web MVC**
   ```bash
   cd API
   dotnet run
   ```
   
   Ou use o script fornecido:
   ```bash
   .\scripts\start-api.bat
   ```

4. **Acesse a aplicação**
   - Interface Web: `http://localhost:5192`
   - API REST: `http://localhost:5192/api`
   - Swagger (Documentação da API): `http://localhost:5192/swagger`

### URLs Principais

| Funcionalidade | URL |
|---|---|
| **Home** | `http://localhost:5192/` |
| **Gestão de Clientes** | `http://localhost:5192/ClientesWeb` |
| **Gestão de Veículos** | `http://localhost:5192/VeiculosWeb` |
| **Gestão de Ordens de Serviço** | `http://localhost:5192/OrdensServicoWeb` |
| **API - Clientes** | `http://localhost:5192/api/clientes/search` |
| **API - Veículos** | `http://localhost:5192/api/veiculos/search` |
| **API - Ordens** | `http://localhost:5192/api/ordensservico/search` |

## Funcionalidades Implementadas

### ✅ Sprint 2 - Camada Web MVC (Nova)

**Interface Web com Views Bootstrap:**
- ✅ Layout responsivo com Bootstrap 5.3 CDN
- ✅ Barra de navegação com links para todos os módulos
- ✅ Página inicial (Home) com dashboard simples
- ✅ Alertas para mensagens de sucesso, erro e aviso (TempData)
- ✅ Footer com informações do sistema
- ✅ Design responsivo para desktop e mobile
- ✅ Fonte Poppins do Google Fonts

**Controllers Web MVC :**
- ✅ `HomeController` - Página inicial com navegação
- ✅ `ClientesWebController` - CRUD de clientes (Create, Read, Update, Details)
- ✅ `VeiculosWebController` - CRUD de veículos (Create, Read, Update, Details)
- ✅ `OrdensServicoWebController` - CRUD de ordens de serviço com **ação de fechamento**

**Views Razor:**
- ✅ `_Layout.cshtml` - Layout principal com navbar, body, Scripts section e footer
- ✅ `Home/Index.cshtml` - Dashboard com 3 cards de navegação
- ✅ `ClientesWeb/Index.cshtml` - Listagem com paginação e busca
- ✅ `ClientesWeb/Create.cshtml` - Formulário com validação e botão Edit
- ✅ `ClientesWeb/Details.cshtml` - Detalhes com botão Editar
- ✅ `ClientesWeb/Edit.cshtml` - Formulário de edição
- ✅ `VeiculosWeb/Index.cshtml`, `Create.cshtml`, `Details.cshtml`, `Edit.cshtml`
- ✅ `OrdensServicoWeb/Index.cshtml`, `Create.cshtml`, `Details.cshtml`, `Edit.cshtml`

**ViewModels :**
- ✅ `ClienteViewModels` - ClienteListViewModel, ClienteItemViewModel, ClienteFormViewModel
- ✅ `VeiculoViewModels` - VeiculoListViewModel, VeiculoItemViewModel, VeiculoFormViewModel
- ✅ `OrdemServicoViewModels` - OrdemServicoListViewModel, OrdemServicoItemViewModel, OrdemServicoFormViewModel
- ✅ `ClienteSelectViewModel` - Seleção de cliente para formulários
- ✅ `VeiculoSelectViewModel` - Seleção de veículo com propriedade `PlacaModelo`

### ✅ Sprint 2 - Melhorias na API

**API REST com Paginação, Filtros e HATEOAS:**
- ✅ `ClientesController` (/api/clientes) - 4 endpoints
  - `GET /api/clientes/search` - Busca com paginação, filtros, ordenação e HATEOAS
  - `GET /api/clientes` - Listar todos
  - `GET /api/clientes/{id}` - Buscar por ID
  - `POST /api/clientes` - Criar novo cliente

- ✅ `VeiculosController` (/api/veiculos) - 4 endpoints
  - `GET /api/veiculos/search` - Busca com paginação, filtros e ordenação
  - `GET /api/veiculos` - Listar todos
  - `GET /api/veiculos/{id}` - Buscar por ID
  - `POST /api/veiculos` - Criar novo veículo

- ✅ `OrdensServicoController` (/api/ordensservico) - 5 endpoints
  - `GET /api/ordensservico/search` - Busca com paginação, filtros e ordenação
  - `GET /api/ordensservico` - Listar todas
  - `GET /api/ordensservico/{id}` - Buscar por ID
  - `POST /api/ordensservico` - Criar nova ordem
  - `PUT /api/ordensservico/{id}/fechar` - **Fechar ordem de serviço**

**Links HATEOAS:**
- ✅ Navegação entre páginas (first, last, next, previous, self)
- ✅ URLs dinâmicas com parâmetros preservados

**Parâmetros de Query:**
- ✅ `pageNumber` - Número da página (padrão: 1)
- ✅ `pageSize` - Itens por página (padrão: 10)
- ✅ `orderBy` - Campo para ordenação (padrão: Id)
- ✅ `descending` - Ordenação decrescente (padrão: false)
- ✅ `searchTerm` - Termo de busca genérico

**Classes de Suporte:**
- ✅ `PaginationDTO` - Encapsula parâmetros de paginação
- ✅ `PagedResultDTO<T>` - Retorna dados paginados com metadados e links
- ✅ `LinkDTO` - Representa links HATEOAS

**Melhorias nos Services:**
- ✅ `BuscarClientesAsync()` - Filtros, ordenação e paginação
- ✅ `BuscarVeiculosAsync()` - Filtros, ordenação e paginação
- ✅ `BuscarOrdensServicoAsync()` - Filtros, ordenação e paginação
- ✅ `FecharOrdemServicoAsync()` - Fechamento com data automática

### ✅ Sprint 1 - Funcionalidades Base

**Camada de Domínio:**
- ✅ Entidades com validações (Cliente, Veiculo, OrdemServico, Servico)
- ✅ CPF único por cliente
- ✅ Placa única por veículo
- ✅ Status controlado para ordens de serviço
- ✅ Repositórios com interfaces genéricas

**Camada de Aplicação:**
- ✅ Services para CRUD de clientes, veículos e ordens
- ✅ DTOs para transferência de dados
- ✅ BusinessException para tratamento de erros
- ✅ Cálculo automático do valor total de ordens

**Camada de Infraestrutura:**
- ✅ Entity Framework Core com banco em memória
- ✅ Seed de dados iniciais (2 clientes, 2 veículos, 2 ordens)
- ✅ Repositórios implementados

**Funcionalidades Principais:**
- ✅ Cadastro de clientes com validação de CPF único
- ✅ Cadastro de veículos vinculados a clientes
- ✅ Criação de ordens de serviço para veículos
- ✅ Adição de serviços às ordens de serviço
- ✅ Consulta de ordens por status, período ou veículo
- ✅ Fechamento de ordem de serviço com data automática
- ✅ Cálculo automático do valor total

## Exemplos de Uso

### Interface Web MVC

#### 1. Dashboard (Home)
```
URL: http://localhost:5192/
Descrição: Página inicial com 3 cards de navegação para Clientes, Veículos e Ordens de Serviço
```

#### 2. Listagem de Clientes
```
URL: http://localhost:5192/ClientesWeb
Funcionalidades:
- Pesquisa por nome, CPF ou email
- Paginação configurável
- Botão "Novo Cliente" para criar
- Links para Detalhes, Editar
```

#### 3. Criar/Editar Cliente
```
URL: http://localhost:5192/ClientesWeb/Create
URL: http://localhost:5192/ClientesWeb/Edit/{id}
Campos:
- Nome (obrigatório)
- CPF (obrigatório, formatação: XXX.XXX.XXX-XX)
- Telefone (formatação: (XX) XXXXX-XXXX)
- Email
- Endereço
```

#### 4. Listagem de Veículos
```
URL: http://localhost:5192/VeiculosWeb
Funcionalidades:
- Pesquisa por marca, modelo ou placa
- Seleção de cliente obrigatória
- Botão "Novo Veículo"
- Links para Detalhes, Editar
```

#### 5. Listagem de Ordens de Serviço
```
URL: http://localhost:5192/OrdensServicoWeb
Funcionalidades:
- Pesquisa por status
- Paginação
- Botão "Nova Ordem"
- Botão "Fechar" em cada ordem (via POST)
- Links para Detalhes, Editar
```

### API REST

#### 1. Buscar Clientes com Paginação
```
GET /api/clientes/search?pageNumber=1&pageSize=10&orderBy=nome&searchTerm=João
```

Resposta exemplo:
```json
{
  "items": [
    {
      "id": 1,
      "nome": "João Silva",
      "cpf": "123.456.789-00",
      "telefone": "(11) 98765-4321",
      "email": "joao@email.com",
      "endereco": "Rua A, 123"
    }
  ],
  "totalCount": 2,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "hasNextPage": false,
  "hasPreviousPage": false,
  "links": [
    {
      "rel": "self",
      "href": "/api/clientes/search?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "first",
      "href": "/api/clientes/search?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "last",
      "href": "/api/clientes/search?pageNumber=1&pageSize=10",
      "method": "GET"
    }
  ]
}
```

#### 2. Buscar Veículos com Filtro
```
GET /api/veiculos/search?pageNumber=1&pageSize=10&searchTerm=Toyota
```

#### 3. Buscar Ordens por Status
```
GET /api/ordensservico/search?pageNumber=1&pageSize=10&searchTerm=Concluida&orderBy=dataabertura&descending=true
```

#### 4. Criar um Cliente
```
POST /api/clientes
Content-Type: application/json

{
  "nome": "Maria Santos",
  "cpf": "987.654.321-00",
  "telefone": "(21) 99876-5432",
  "email": "maria@email.com",
  "endereco": "Av. Paulista, 1000"
}
```

#### 5. Fechar uma Ordem de Serviço
```
PUT /api/ordensservico/{id}/fechar
```

## Status de Compilação

- ✅ Todos os 4 projetos compilam sem erros
- ✅ 48 avisos de tipos anuláveis (aceitável)
- ✅ Funcionalidades testadas e operacionais
- ✅ API rodando em http://localhost:5192
- ✅ Interface Web responsiva e funcional
- ✅ CRUD completo para Clientes, Veículos e Ordens de Serviço

## Melhorias Implementadas (Sprint 2C)

- ✅ Layout com Bootstrap 5.3 CDN
- ✅ Paginação em todas as listagens
- ✅ Busca e filtros funcionais
- ✅ Formulários com validação
- ✅ Formatação automática (CPF, Telefone)
- ✅ Mensagens de sucesso/erro com TempData
- ✅ Buttons "Novo", "Editar", "Detalhes"
- ✅ Ação de fechamento de ordem de serviço
- ✅ Resposta HATEOAS na API
- ✅ Seed de dados iniciais

## Autores

- Fabio H S Eduardo - RM560416
- Gabriel Wu Castro - RM560210
- Renato Kenji Sugaki - RM559810

Projeto acadêmico desenvolvido para a disciplina de Advanced Business Development with .NET

