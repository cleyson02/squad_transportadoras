# Gestão de Coletas para Transportadora

Aplicação web para gerenciar solicitações de coleta de uma transportadora:
login, cadastro de coletas, controle de status, atribuição de motorista e
veículo, registro de ocorrências e filtros de listagem.

## Tecnologias

- **Backend:** C# / .NET 8, ASP.NET Core Web API, EF Core (Code First), ASP.NET Core Identity + JWT, Swagger
- **Frontend:** React, React Router, Axios, react-icons
- **Banco de Dados:** SQL Server 2022
- **Orquestração:** Docker e Docker Compose (Multi-stage build)

## Pré-requisitos

Para rodar a aplicação completa, você precisa apenas do:
- **Docker Desktop** instalado e rodando.

*(Opcional para desenvolvimento local sem containers: .NET SDK 8.0 e Node.js 18+)*

## Como rodar (Via Docker)

Toda a infraestrutura (Banco, API e Frontend) foi conteinerizada para subir com um único comando.

1. Na raiz do projeto (onde está o arquivo `docker-compose.yml`), abra o terminal e rode:

```bash
docker compose up -d --build
```

2. Aguarde o processo finalizar. Na primeira execução, o Docker fará o download das imagens, o build do frontend em React, a compilação da API em .NET e aplicará automaticamente as *migrations* e o *seed* no SQL Server.

### Acessando os serviços

- **Frontend (Painel):** `http://localhost:3000`
- **Backend (Swagger):** `http://localhost:5256/swagger`
- **Backend (API):** `http://localhost:5256/api`
- **Banco de Dados:** `localhost,1433` (Usuário: `sa` | Senha: `SenhaForte@123`)

> **Para parar a aplicação:** Execute `docker compose down`. Se quiser zerar o banco de dados completamente, rode `docker compose down -v`.

## Usuário padrão (Seed)

Ao iniciar pela primeira vez, o banco de dados é populado automaticamente com os seguintes dados de teste:

| Usuário | Senha     |
|---------|-----------|
| admin   | Admin@123 |

O *seed* também cria 2 clientes, 2 motoristas e 2 veículos de exemplo para facilitar os testes.

## Variáveis de ambiente / configuração

O projeto utiliza o arquivo `backend/backend/appsettings.json` como base, porém as configurações críticas são injetadas diretamente pelo `docker-compose.yml` usando variáveis de ambiente:

- **ConnectionStrings__DefaultConnection:** Aponta a API para o container `sqlserver`.
- **REACT_APP_API_URL:** Define a rota `http://localhost:5256/api` no build do frontend.

> **Nota de Segurança:** Em produção, as chaves do JWT (`Jwt__Key`) e as senhas do banco devem ser alteradas e mantidas fora do repositório de código.

## Como validar o sistema

1. Acesse `http://localhost:3000` e faça login com `admin` / `Admin@123`.
2. Crie uma nova coleta em **Nova coleta**.
3. Abra os **detalhes** da coleta.
4. **Atribua** um motorista e um veículo.
5. Tente **marcar como coletado** sem motorista/veículo — deve ser bloqueado.
6. Com motorista e veículo atribuídos, mude o status para **Em coleta** e depois **Coletada**.
7. **Registre uma ocorrência** e confira na tabela a data/hora e o usuário responsável.
8. Crie outra coleta, clique em **Cancelar** e tente coletá-la depois — deve ser bloqueado.
9. Use os **filtros** (situação, cliente, período) na listagem inicial.

## Fluxo de status

```text
Aberta -> Atribuída -> Em Coleta -> Coletada
                  \-> Cancelada (disponível enquanto não estiver Coletada)
```

**Regras de Negócio Aplicadas:**
- Coleta **cancelada** não volta para *Em Coleta* nem *Coletada*.
- Não é possível marcar como **Coletada** ou ir para **Em Coleta** sem motorista **e** veículo vinculados.
- Toda **ocorrência** guarda data/hora do servidor e o usuário responsável (extraído do token JWT).
- Coletas de **prioridade alta** recebem ordenação primária no banco de dados e aparecem destacadas no topo da listagem.