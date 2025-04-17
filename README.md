# 🧳 ViajaFacil

ViajaFacil é uma aplicação Fullstack desenvolvida com C# e .NET, que facilita a busca e reserva de destinos turísticos. A plataforma permite que usuários explorem destinos e realizem reservas de forma prática e segura.

## 📌 Funcionalidades

- Cadastro e autenticação de usuários com validações básicas
- Controle de acesso baseado em perfis (futuramente com roles)
- CRUD completo de destinos turísticos
- Reservas de destinos
- API protegida com autenticação JWT
- Middleware para autenticação
- Estrutura escalável para inclusão de permissões, painel admin, etc

## 🔁 Endpoints da API

### 🔐 Autenticação

- `POST /api/auth/register` — Registrar novo usuário
- `POST /api/auth/login` — Login e retorno de token JWT
- `GET /api/auth/me` — Obter dados do usuário autenticado

### 👥 Usuários

- `GET /api/users` — [Admin futuro] Listar todos os usuários
- `GET /api/users/{id}` — Detalhar usuário por ID
- `PUT /api/users/{id}` — Atualizar dados do usuário
- `DELETE /api/users/{id}` — Remover usuário

### 📍 Destinos

- `GET /api/destinos` — Listar todos os destinos
- `GET /api/destinos/{id}` — Detalhar um destino
- `POST /api/destinos` — Criar novo destino
- `PUT /api/destinos/{id}` — Atualizar destino
- `DELETE /api/destinos/{id}` — Deletar destino

### 📅 Reservas

- `POST /api/reservas` — Criar nova reserva
- `GET /api/reservas` — Listar reservas do usuário logado
- `GET /api/reservas/all` — [Admin futuro] Listar todas as reservas
- `PUT /api/reservas/{id}/status` — [Admin futuro] Atualizar status da reserva

## 🛠️ Tecnologias utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- C#
- JWT para autenticação
- Swagger (Swashbuckle) par
