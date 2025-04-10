# ✈️ ViajaFácil – Sistema de Reservas de Viagens

ViajaFácil é uma aplicação web desenvolvida em C# com ASP.NET Core que simula o funcionamento de uma agência de viagens. O sistema permite o cadastro de destinos, gerenciamento de reservas, e login com diferentes níveis de acesso (Admin e Usuário).

## 🧱 Tecnologias Utilizadas

### Backend
- ASP.NET Core 7 (MVC ou Web API)
- Entity Framework Core
- ASP.NET Identity
- SQL Server / SQLite
- AutoMapper
- FluentValidation
- JWT (se versão API)
- Serilog (opcional para logging)

### Frontend
- Razor Pages (MVC) ou HTML + Bootstrap
- JavaScript (validações, interações)
- (Opcional) React + Tailwind (para versão futura com API)

## 📚 Funcionalidades

### 👤 Sistema de Autenticação

| Tipo | Descrição |
|------|-----------|
| Registro/Login | Usuários podem se registrar, fazer login, e acessar áreas específicas |
| Níveis de Acesso | Admin e Usuário Comum |
| Recuperação de Senha | Implementação opcional |

### 🧑‍💼 Admin

- CRUD completo de destinos de viagem
- Visualização e gestão de usuários cadastrados
- Visualização de reservas realizadas
- Geração de relatórios simples (PDF ou tela)
- Dashboard com estatísticas (opcional)

### 🙋‍♂️ Usuário Comum

- Navegar e buscar por pacotes de viagem
- Filtros por preço, destino, data, duração
- Fazer reservas com dados pessoais e datas
- Visualizar histórico de reservas
- Avaliar uma viagem (1–5 estrelas + comentário)
- Gerar comprovante em PDF da reserva (opcional)

## 🔐 Regras de Negócio

- Usuários só podem reservar se estiverem logados
- Um usuário pode reservar o mesmo destino mais de uma vez
- Admin não pode ser excluído via painel
- Data de ida deve ser menor que a data de volta
- Viagens devem ter vagas limitadas (opcional)

## 🧩 Estrutura do Banco de Dados

Tabelas principais:

- `Users` (ID, Nome, Email, Role, etc.)
- `Destinos` (ID, Nome, País, Descrição, Preço, Imagem)
- `Reservas` (ID, UserID, DestinoID, DataIda, DataVolta, Total, QtdPessoas)
- `Avaliacoes` (ID, UserID, DestinoID, Estrelas, Comentário, Data)
- `Roles` (Admin, User)

## 🔗 Endpoints da API (se for Web API)

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | /api/auth/register | Registro de novo usuário |
| `POST` | /api/auth/login | Login e retorno do JWT |
| `GET`  | /api/destinos | Listagem de destinos (filtro opcional) |
| `POST` | /api/destinos | Criação de destino (admin) |
| `PUT`  | /api/destinos/{id} | Atualização de destino |
| `DELETE` | /api/destinos/{id} | Remoção de destino |
| `POST` | /api/reservas | Criar reserva |
| `GET` | /api/reservas | Listar reservas do usuário logado |
| `POST` | /api/avaliacoes | Avaliar viagem |
| `GET` | /api/admin/users | Listar todos usuários (admin) |
| `GET` | /api/admin/reservas | Listar todas as reservas (admin) |

## 🖼️ Telas do Frontend

- Login / Cadastro
- Página inicial com listagem de destinos
- Página de detalhes do destino
- Tela de reserva com formulário
- Dashboard Admin:
  - Lista de pacotes
  - Cadastro/edição de pacotes
  - Lista de usuários e reservas
- Histórico de reservas do usuário
- Tela de avaliação de viagem

## 📁 Estrutura de Pastas (MVC)

```
ViajaFacil/
│
├── Controllers/
│   └── DestinosController.cs
│   └── ReservasController.cs
│   └── AuthController.cs
│   └── AdminController.cs
│
├── Models/
│   └── User.cs
│   └── Destino.cs
│   └── Reserva.cs
│   └── Avaliacao.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Services/
│   └── IReservaService.cs
│   └── ReservaService.cs
│
├── Repositories/
│   └── IDestinoRepository.cs
│   └── DestinoRepository.cs
│
├── DTOs/
├── ViewModels/
├── Views/
│   └── Home/
│   └── Destinos/
│   └── Reservas/
│   └── Admin/
│
├── wwwroot/
│   └── css/
│   └── js/
│   └── images/
```

## 📌 Possíveis Melhorias Futuras

- Autenticação via redes sociais (OAuth2 Google/Facebook)
- Sistema de cupons de desconto
- Chat com atendente (simulado)
- Painel mobile com PWA
- Testes com xUnit
- CI/CD com GitHub Actions
- Deploy no Azure ou Render

## 🚀 Como Rodar o Projeto

```bash
git clone https://github.com/seunome/viajafacil.git
cd viajafacil

# Configurar string de conexão no appsettings.json

dotnet ef database update
dotnet run
```
