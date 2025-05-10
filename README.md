# RestaurantesAPI
## Using Redis as Cache (Usando Redis como Cache)
<table style="border: none; border-collapse: collapse;">
  <tr>
    <td style="border: none; padding: 10px;">
<p>In contemporary APIs, two mandatory factors are performance and scalability. The exponential and volumetric growth of data, along with the need for real-time response in web applications, makes the       use of caching techniques essential to reduce database overload, thereby improving the user experience.</p>
  <p>In what follows, we will design a REST API using ASP.NET Core and Entity Framework Core to retrieve a list of restaurants, incorporating Redis as a caching mechanism. This ensures faster responses and better performance when accessing data.</p>
<p>You will learn in practice how to build a clean and reusable project that optimizes database queries, configures Redis in ASP.NET, implements cache logic transparently to the user, and fine-tunes your queries to improve performance. This article is aimed at .NET developers who want to adopt new approaches to enhance performance in Web APIs.</p>

</td>
    <td style="border: none; padding: 10px;">
      <p>Em APIs contemporâneas, dois fatores obrigatórios são o desempenho e escalabilidade. O crescimento exponencial e volumétrico de dados, assim como a necessidade de resposta em tempo real em aplicações web tornam essenciais o uso de técnicas de cache para sobrecargas de banco de dados, melhorando a experiência do usuário.</p>      
<p>No que se segue, desenharemos uma API REST com o ASP.NET Core e Entity Framework Core para restaurar uma lista de restaurantes, incorporando o Redis como um mecanismo de cache. Isso assegura respostas mais rápidas e melhor performance ao acessarem os dados.</p>

<p>Você vai na prática aprender a montar um projeto limpo e reutilizável que otimize as consultas ao banco de dados, configure o Redis no ASP.NET, implemente lógica de cache transparente ao usuário e otimize suas consultas ao banco de dados para aprimorar a performance. Este artigo é voltado para programadores em .NET que queiram incorporar novas abordagens ao rendimento em APIs Web Action</p>
</td>
  </tr>
</table>

## Technologies Used - Tecnologias Empregadas

<table style="border: none; border-collapse: collapse;">
  <tr>
    <td style="border: none; padding: 10px;">
<p>This project was developed using the .NET ecosystem with a focus on best practices in architecture, performance, and scalability. Below, we list the main technologies used:</p>

- ASP.NET Core 8 (Web API): Modern and cross-platform framework from Microsoft for building RESTful APIs. We used features from version 8.0 to create HTTP endpoints in a clean and efficient way.
- Entity Framework Core 8: Microsoft's official ORM (Object-Relational Mapper) for data access. Used to map domain entities and manage data persistence in the relational database in a fluent and object-oriented way.
- Pomelo.EntityFrameworkCore.MySql: Entity Framework Core provider for MySQL databases. It allows us to use EF Core features with a MySQL or MariaDB database natively.
- Redis: In-memory database, widely used as a distributed caching system. In this project, Redis is responsible for temporarily storing the most frequent queries, reducing the number of database accesses and significantly improving the API response time.
- StackExchange.Redis: Client library for communication with Redis in .NET. Used to implement read and write operations on the cache in a performant and secure manner.
-	Clean Architecture: The project structure follows the principles of Clean Architecture, promoting the separation of responsibilities across layers (Domain, Application, Infrastructure, and API), making the application easier to maintain, scale, and test.

<p>This set of technologies provides a solid foundation for developing modern, high-performance APIs that are easy to evolve, catering to both web and mobile applications.</p>

      
  </td>
  <td style="border: none; padding: 10px;">
    <p>Este projeto foi desenvolvido utilizando o ecossistema .NET com foco em boas práticas de arquitetura, performance e escalabilidade. Abaixo, listamos as principais tecnologias utilizadas:</p>

- ASP.NET Core 8 (Web API): Framework moderno e multiplataforma da Microsoft para construção de APIs RESTful. Utilizamos os recursos da versão 8.0 para criação dos endpoints HTTP de forma limpa e eficiente.
- Entity Framework Core 8: ORM (Object-Relational Mapper) oficial da Microsoft para acesso a dados. Usado para mapear as entidades de domínio e gerenciar a persistência de dados no banco de dados relacional de forma fluida e orientada a objetos.
- Pomelo.EntityFrameworkCore.MySql: Provedor do Entity Framework Core para bancos de dados MySQL. Ele nos permite utilizar os recursos do EF Core com um banco MySQL ou MariaDB de maneira nativa.
- Redis: Banco de dados em memória, amplamente usado como sistema de cache distribuído. Neste projeto, o Redis é responsável por armazenar as consultas mais frequentes de forma temporária, reduzindo o número de acessos ao banco de dados e melhorando significativamente o tempo de resposta da API.
-	StackExchange.Redis: Biblioteca cliente para comunicação com o Redis no .NET. Utilizada para implementar as operações de leitura e escrita no cache de forma performática e segura.
-	Clean Architecture: A estrutura do projeto segue os princípios da Arquitetura Limpa (Clean Architecture), promovendo a separação de responsabilidades entre camadas (Domínio, Aplicação, Infraestrutura e API), facilitando a manutenção, escalabilidade e testabilidade da aplicação.

<p>Esse conjunto de tecnologias oferece uma base sólida para desenvolvimento de APIs modernas, performáticas e fáceis de evoluir, atendendo tanto aplicações web quanto mobile.</p>

  </td>
  </tr>
</table>
