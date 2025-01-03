# SalesManagement

**SalesManagement** é um sistema de gerenciamento de vendas projetado para aplicar boas práticas de desenvolvimento, como Clean Architecture, CQRS, SOLID e DDD. 
<br>
Este projeto foi criado por mim para aprendizado prático e implementação de conceitos avançados de engenharia de software.

---

## 🛠 Tecnologias e Conceitos Utilizados

- **ASP.NET Core 6**
- **xUnit** para testes unitários.
- **Clean Architecture** e **Domain-Driven Design (DDD)**.
- **Princípios SOLID** para um design de software robusto.
- **CQRS**.
- **Repository Pattern**.
- **Docker e Docker Compose**.
- **SQL Server** como banco de dados.
- **Mediatr** para a aplicação de CQRS.
- **Dependency Injection** para melhorar modularidade e testabilidade.
- **FluentValidation** para validação de regras de negócios.


---

## ⚙️ Funcionalidades

- **Gerenciamento Clientes** (cadastrar - listar - atualizar - remover - ativar e inativar)
- **Gerenciamento Produtos** (cadastrar - listar - remover - ativar e inativar - incrementar e decrementar estoque - atualizar preço)
- **Gerenciamento de Pedidos** (cadastrar - listar - remover - Controle de Pagamentos)
  
---

## 📋 Pré-requisitos

- **.NET 6 SDK**
- **Docker Desktop instalado**
- **SQL Server**
- Ferramentas como **Visual Studio** ou **Rider** são recomendadas.

---

## 🚀 Como Executar o Projeto com Docker

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/luiz-lgrp/SalesManagement.git
   cd SalesManagement
   ```
2. **Construa e rode o projeto usando Docker Compose**

   ```bash
     docker-compose up --build
      ```
2. **Acesse o Swagger**
- Após o container subir, abra o navegador e acesse:

   ```bash
     http://localhost:3000/swagger/index.html
   ```
---

## 🐳 Explicação do Docker Compose
O arquivo docker-compose.yml inicia dois containers:
- **API .NET (SalesManagement)** – Aplicação backend
- **SQL Server** – Banco de dados utilizado pela aplicação
  <br> <br>
**Ambos os containers são configurados para se comunicarem diretamente, eliminando a necessidade de instalar o SQL Server localmente.**

---

## 🗃️ Populando Banco de Dados automáticamente

**A aplicação possui um método Initialize na camada de infraestrutura que popula o banco de dados com dados iniciais para testes.**
<br>
**Isso permite que ao iniciar a aplicação o banco já contenha alguns dados, facilitando a avaliação e os testes das funcionalidades.**

---

## 🚀 Como Executar o Projeto Sem o Docker

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/luiz-lgrp/SalesManagement.git
   cd SalesManagement
   ```
2. **Configure o Banco de Dados:**
- Ajuste as configurações de conexão no arquivo **appsettings.Development.json** para apontar para o seu SQL Server.

3. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

4. **Inicie o projeto:**
   ```bash
   dotnet run
   ```

---

## 🧪 Testes
- Todos os testes estão localizados na pasta Tests/.
- Execute-os com o comando:
    ```bash
      dotnet test
   ```

---

## 📞 Contato
Você me encontra aqui: https://luiz-lgrp.github.io/portfolio/
<br><br>
Desenvolvido por Luiz Gustavo
