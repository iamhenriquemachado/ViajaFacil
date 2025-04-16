🧳 ViajaFacil

ViajaFacil é uma aplicação Fullstack em desenvolvimento com C# e .NET, que facilita a busca e reserva de destinos turísticos. A plataforma permite que usuários explorem destinos e realizem reservas de forma prática e segura.
📌 Funcionalidades

    Cadastro e autenticação de usuários com validações básicas

    Controle de acesso baseado em perfis (futuramente com roles)

    CRUD completo de destinos turísticos

    Reservas de destinos

    API protegida com autenticação JWT

    Middleware para autenticação

    Estrutura escalável para inclusão de permissões, painel admin, etc

🧱 Estrutura do modelo User

public class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

    ⚠️ Neste momento, o controle de roles (Admin/User) ainda não está implementado no modelo, mas pode ser adicionado futuramente com uma tabela Roles e a propriedade RoleId.

🔁 Endpoints da API (sugestão com base no modelo atual)
🔐 Autenticação

    POST /api/auth/register — Registrar novo usuário

    POST /api/auth/login — Login e retorno de token JWT

    GET /api/auth/me — Obter dados do usuário autenticado

👥 Usuários

    GET /api/users — [Admin futuro] Listar todos os usuários

    GET /api/users/{id} — Detalhar usuário por ID

    PUT /api/users/{id} — Atualizar dados do usuário

    DELETE /api/users/{id} — Remover usuário

📍 Destinos

    GET /api/destinos — Listar todos os destinos

    GET /api/destinos/{id} — Detalhar um destino

    POST /api/destinos — Criar novo destino

    PUT /api/destinos/{id} — Atualizar destino

    DELETE /api/destinos/{id} — Deletar destino

📅 Reservas

    POST /api/reservas — Criar nova reserva

    GET /api/reservas — Listar reservas do usuário logado

    GET /api/reservas/all — [Admin futuro] Listar todas as reservas

    PUT /api/reservas/{id}/status — [Admin futuro] Atualizar status da reserva

🛠️ Tecnologias utilizadas

    ASP.NET Core

    Entity Framework Core

    SQL Server

    C#

    JWT para autenticação

    Swagger (Swashbuckle) para documentação

    AutoMapper (opcional)

    FluentValidation (opcional)

🚀 Como executar o projeto

# Clone o repositório
git clone https://github.com/seu-user/viajafacil.git
cd viajafacil

# Configure a string de conexão no appsettings.json

# Execute as migrações
dotnet ef database update

# Inicie a aplicação
dotnet run

📈 Melhorias futuras

    Implementação de roles (Admin, User) com tabela dedicada

    Upload de imagens para os destinos

    Interface web com Blazor ou outro framework frontend

    Sistema de avaliação e comentários em destinos

    Painel administrativo com filtros, gráficos e estatísticas

    Recuperação de senha

    Integração com serviços de pagamento online
