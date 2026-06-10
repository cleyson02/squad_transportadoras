# Gestão de Coletas para Transportadora

Aplicação web para gerenciar solicitações de coleta de uma transportadora:
login, cadastro de coletas, controle de status, atribuição de motorista e
veículo, registro de ocorrências e filtros de listagem.

## Tecnologias

- **Backend:** C# / .NET 8, ASP.NET Core Web API, EF Core (Code First), SQL Server, ASP.NET Core Identity + JWT, Swagger
- **Frontend:** React, React Router, Axios, react-icons
- **Banco:** SQL Server 2022 (Docker)

## Pré-requisitos

- .NET SDK 8.0
- Node.js 18+ e npm
- Docker Desktop
- (opcional) ferramenta EF Core: `dotnet tool install --global dotnet-ef`

Detalhes completos em [`dependencies.txt`](./dependencies.txt).

## Como rodar

### 1. Subir o banco (Docker)

```bash
docker compose up -d
```

O SQL Server sobe na porta `1433`. Senha do usuário `sa`: `SenhaForte@123`
(definida em `docker-compose.yml`).

### 2. Backend (API)

```bash
cd backend/backend
dotnet restore
dotnet ef migrations add Inicial   # cria a migration inicial
dotnet run
```

- API: `http://localhost:5256`
- Swagger: `http://localhost:5256/swagger`

Ao iniciar, a aplicação aplica as migrations automaticamente e executa o
**seed** (cria o usuário admin e dados de exemplo).

> Se preferir, é possível aplicar a migration com `dotnet ef database update`,
> mas não é obrigatório: o `dotnet run` já faz isso ao subir.

### 3. Frontend

```bash
cd frontend
npm install
npm start
```

Frontend: `http://localhost:3000`

Lista completa de comandos em [`commands.txt`](./commands.txt).

## Variáveis de ambiente / configuração

A configuração fica em `backend/backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TransportadoraDb;User Id=sa;Password=SenhaForte@123;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "chave-secreta-do-case-transportadora-troque-em-producao",
    "Issuer": "GestaoColetas",
    "Audience": "GestaoColetasFrontend",
    "ExpiresHours": "8"
  }
}
```

> Em produção, troque a `Jwt:Key` por uma chave forte e mantenha-a fora do
> repositório.

A URL da API usada pelo frontend está em `frontend/src/services/api.js`
(`http://localhost:5256/api`).

## Usuário padrão (seed)

| Usuário | Senha     |
|---------|-----------|
| admin   | Admin@123 |

O seed também cria 2 clientes, 2 motoristas e 2 veículos de exemplo.

## Como validar o sistema

1. Acesse `http://localhost:3000` e faça login com `admin` / `Admin@123`.
2. Crie uma nova coleta em **Nova coleta**.
3. Abra os **detalhes** da coleta.
4. **Atribua** um motorista e um veículo.
5. Tente **marcar como coletado** sem motorista/veículo — deve ser bloqueado.
6. Com motorista e veículo, mude para **Em coleta** e depois **Coletado**.
7. **Registre uma ocorrência** e confira data/hora e usuário.
8. Crie outra coleta, **cancele** e tente coletá-la — deve ser bloqueado.
9. Use os **filtros** (situação, cliente, período) na listagem.

## Fluxo de status

```
Aberta -> Atribuída -> Em Coleta -> Coletada
                  \-> Cancelada (enquanto não estiver Coletada)
```

Regras aplicadas:

- Coleta **cancelada** não volta para *Em Coleta* nem *Coletada*.
- Não é possível marcar como **Coletada** sem motorista **e** veículo.
- Toda **ocorrência** guarda data/hora e o usuário responsável (do token JWT).
- Coletas de **prioridade alta** aparecem destacadas na listagem.