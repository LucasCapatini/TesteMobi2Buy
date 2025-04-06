# TesteMobi2Buy
Autor: Lucas Capatini Ernandes


Este projeto é uma API desenvolvida em .NET 9 com arquitetura limpa, integração com ViaCEP, mensageria com RabbitMQ, persistência com PostgreSQL e testes automatizados com xUnit.

  

---

  

##  Estrutura do Projeto

  

```

TesteMobi2Buy.sln

├── docker-compose.yml

├── src/

│ ├── TesteMobi2Buy.API/ # Projeto Web API

│ ├── TesteMobi2Buy.Application/ # Camada de Aplicação (DTOs, Serviços)

│ ├── TesteMobi2Buy.Domain/ # Entidades e Interfaces de domínio

│ ├── TesteMobi2Buy.Infrastructure/ # Integrações (ViaCEP, RabbitMQ, Repositórios)

├── TesteMobi2Buy.Tests/ # Projeto de testes com xUnit

```

  

---

  

##  Como Executar a Aplicação

  

### Pré-requisitos

  

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

- [Docker](https://www.docker.com/)

  

### 1. Subir containers (API + PostgreSQL + RabbitMQ)

  

```bash

docker-compose up -d

```

  

RabbitMQ Management: [http://localhost:15672](http://localhost:15672)

Login: `guest` — Senha: `guest`

A API estará disponível em: [http://localhost:5000/swagger](http://localhost:5000/swagger)
  

---

##  Executando os Testes

  

### 1. Rodar todos os testes

  

Na raiz do projeto:

  

```bash

dotnet test TesteMobi2Buy.Tests

```

  

### 2. Estrutura dos testes

  

Os testes estão organizados por camada dentro da pasta `TesteMobi2Buy.Tests`:

 
- `Services/`: Testes unitários para `ClienteAppService`

  

---

  

##  Tecnologias Utilizadas

  

- .NET 9

- Entity Framework Core

- PostgreSQL

- RabbitMQ

- ViaCEP (API externa)

- Polly

- Docker

- Swagger

- xUnit

- Moq



  

---

  

##  Funcionalidades

  

- Cadastro de clientes com validação de e-mail único

- Consulta de endereço por CEP via ViaCEP

- Atualização de dados do cliente

- Publicação de evento `cliente_criado` no RabbitMQ

- Testes unitários da camada de serviço
