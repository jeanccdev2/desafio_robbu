# Desafio Robbu
## Descrição
Este projeto implementa uma API RESTful para gestão de um catálogo de produtos. Foram criados endpoints para satisfazer o formato CRUD (Create, Read, Update e Delete) utilizando o padrão CQRS (Command Query Responsibility Segregation), sendo assim separados os Commands (Create, Update e Delete) das Queries (Read).

## Linguagens/Frameworks
* C#
* .NET 8.0
* ASP.NET
* Entity Framework

## Requisitos
* C#
* .NET Framework 8.0
* PostgreSQL v17.0
* CLI Entity Framework

## Instalação
* Baixe e instale o PostgreSQL na sua máquina nesse link: <a href="https://www.enterprisedb.com/downloads/postgres-postgresql-downloads">PostgreSQL</a>.
* Substitua os dados do <i>appsettings.json</i> pelos dados do seu banco.
* Execute o comando abaixo:
```shell
dotnet ef database update
```
* Instale o CLI do Entity Framework
```shell
dotnet tool install --global dotnet-ef
```

## Execução
### Back-End
* Execute o Back-End usando a própria IDE ou através do comando:
```shell
dotnet run
```
* Abra o navegador no link <a href="https://localhost:7298">https://localhost:7298</a>