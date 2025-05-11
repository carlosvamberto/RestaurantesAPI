## RestaurantesAPI

## Using Redis as Cache (Usando Redis como Cache)

<table style="border-style:none;"><tbody><tr><td style="border-style:none;padding:10px;"><p>In contemporary APIs, two mandatory factors are performance and scalability. The exponential and volumetric growth of data, along with the need for real-time response in web applications, makes the use of caching techniques essential to reduce database overload, thereby improving the user experience.</p><p>In what follows, we will design a REST API using ASP.NET Core and Entity Framework Core to retrieve a list of restaurants, incorporating Redis as a caching mechanism. This ensures faster responses and better performance when accessing data.</p><p>You will learn in practice how to build a clean and reusable project that optimizes database queries, configures Redis in ASP.NET, implements cache logic transparently to the user, and fine-tunes your queries to improve performance. This article is aimed at .NET developers who want to adopt new approaches to enhance performance in Web APIs.</p></td><td style="border-style:none;padding:10px;"><p>Em APIs contemporâneas, dois fatores obrigatórios são o desempenho e escalabilidade. O crescimento exponencial e volumétrico de dados, assim como a necessidade de resposta em tempo real em aplicações web tornam essenciais o uso de técnicas de cache para sobrecargas de banco de dados, melhorando a experiência do usuário.</p><p>No que se segue, desenharemos uma API REST com o ASP.NET Core e Entity Framework Core para restaurar uma lista de restaurantes, incorporando o Redis como um mecanismo de cache. Isso assegura respostas mais rápidas e melhor performance ao acessarem os dados.</p><p>Você vai na prática aprender a montar um projeto limpo e reutilizável que otimize as consultas ao banco de dados, configure o Redis no ASP.NET, implemente lógica de cache transparente ao usuário e otimize suas consultas ao banco de dados para aprimorar a performance. Este artigo é voltado para programadores em .NET que queiram incorporar novas abordagens ao rendimento em APIs Web Action</p></td></tr></tbody></table>

![Diagrama da aplicação](images/diagram.png)

## Technologies Used - Tecnologias Empregadas

<table style="border-style:none;"><tbody><tr><td style="border-style:none;padding:10px;"><p>This project was developed using the .NET ecosystem with a focus on best practices in architecture, performance, and scalability. Below, we list the main technologies used:</p><ul><li><strong>ASP.NET Core 8 (Web API):</strong> Modern and cross-platform framework from Microsoft for building RESTful APIs. We used features from version 8.0 to create HTTP endpoints in a clean and efficient way.</li><li><strong>Entity Framework Core 8:</strong> Microsoft's official ORM (Object-Relational Mapper) for data access. Used to map domain entities and manage data persistence in the relational database in a fluent and object-oriented way.</li><li><strong>Pomelo.EntityFrameworkCore.MySql:</strong> Entity Framework Core provider for MySQL databases. It allows us to use EF Core features with a MySQL or MariaDB database natively.</li><li><strong>Redis:</strong> In-memory database, widely used as a distributed caching system. In this project, Redis is responsible for temporarily storing the most frequent queries, reducing the number of database accesses and significantly improving the API response time.</li><li><strong>StackExchange.Redis:</strong> Client library for communication with Redis in .NET. Used to implement read and write operations on the cache in a performant and secure manner.</li><li><strong>Clean Architecture:</strong> The project structure follows the principles of Clean Architecture, promoting the separation of responsibilities across layers (Domain, Application, Infrastructure, and API), making the application easier to maintain, scale, and test.</li></ul><p>This set of technologies provides a solid foundation for developing modern, high-performance APIs that are easy to evolve, catering to both web and mobile applications.</p></td><td style="border-style:none;padding:10px;"><p>Este projeto foi desenvolvido utilizando o ecossistema .NET com foco em boas práticas de arquitetura, performance e escalabilidade. Abaixo, listamos as principais tecnologias utilizadas:</p><ul><li><strong>ASP.NET Core 8 (Web API):</strong> Framework moderno e multiplataforma da Microsoft para construção de APIs RESTful. Utilizamos os recursos da versão 8.0 para criação dos endpoints HTTP de forma limpa e eficiente.</li><li><strong>Entity Framework Core 8:</strong> ORM (Object-Relational Mapper) oficial da Microsoft para acesso a dados. Usado para mapear as entidades de domínio e gerenciar a persistência de dados no banco de dados relacional de forma fluida e orientada a objetos.</li><li><strong>Pomelo.EntityFrameworkCore.MySql:</strong> Provedor do Entity Framework Core para bancos de dados MySQL. Ele nos permite utilizar os recursos do EF Core com um banco MySQL ou MariaDB de maneira nativa.</li><li><strong>Redis:</strong> Banco de dados em memória, amplamente usado como sistema de cache distribuído. Neste projeto, o Redis é responsável por armazenar as consultas mais frequentes de forma temporária, reduzindo o número de acessos ao banco de dados e melhorando significativamente o tempo de resposta da API.</li><li><strong>StackExchange.Redis:</strong> Biblioteca cliente para comunicação com o Redis no .NET. Utilizada para implementar as operações de leitura e escrita no cache de forma performática e segura.</li><li><strong>Clean Architecture:</strong> A estrutura do projeto segue os princípios da Arquitetura Limpa (Clean Architecture), promovendo a separação de responsabilidades entre camadas (Domínio, Aplicação, Infraestrutura e API), facilitando a manutenção, escalabilidade e testabilidade da aplicação.</li></ul><p>Esse conjunto de tecnologias oferece uma base sólida para desenvolvimento de APIs modernas, performáticas e fáceis de evoluir, atendendo tanto aplicações web quanto mobile.</p></td></tr></tbody></table>

## Application Setup Process - Processo de Setup da aplicação

<table><tbody><tr><td><p>Considering we are in a Windows operating system environment and Docker Desktop is already installed and running, we can proceed with the following steps.</p><h3>Installing Redis Locally with Docker Desktop - Instalando Redis localmente com Docker Desktop</h3><p>Run the command below to download and start the Redis Server container in your local Docker environment.</p><pre><code class="language-plaintext">  docker run -d --name redis-server -p 6379:6379 -v redis-data:/data redis</code></pre><h3>Service Testing</h3><p>The command below performs a test to check if Redis is running properly.</p><pre><code class="language-plaintext">docker exec -it redis-server redis-cli
&gt; SET meuTeste “Bom dia Carlos Vamberto Filho”
&gt; GET meuTest
&gt; exit</code></pre><h3>Installing MySQL Locally with Docker Desktop</h3><p>We will also run MySQL in a container for use in this application. With the command below, we will run MySQL on the default port 3306.</p><pre><code class="language-plaintext">docker run --name mysql-local -e MYSQL_ROOT_PASSWORD=Senha123  -p 3306:3306</code></pre><h3>Creating the Restaurant Table</h3><p>I used the MySQL Workbench application to connect to the MySQL server and create the restaurant table in a new database called “meubanco”.</p><pre><code class="language-plaintext">-- Cria o banco de dados se ainda não existir
CREATE DATABASE IF NOT EXISTS meubanco CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
-- Usa o banco de dados
USE meubanco;
-- Criação da tabela restaurantes
CREATE TABLE IF NOT EXISTS restaurantes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Tipo VARCHAR(100),
    Endereco VARCHAR(255),
    Cidade VARCHAR(100),
    Regiao VARCHAR(100),
    Pais VARCHAR(100)
);</code></pre><h3>Inserts records into the Restaurants table</h3><p>Here we will create records for 50 existing restaurants in the Algarve region of Portugal to demonstrate the API. Run the script below to insert the records:</p><pre><code class="language-plaintext">USE meubanco;

</code></pre><p>&nbsp;</p><p>&nbsp;</p><p><code>INSERT INTO restaurantes (Nome, Tipo, Endereco, Cidade, Regiao, Pais) VALUES</code><br><code>('Luar da Foia', 'Portuguesa tradicional', 'Estrada da Foia', 'Monchique', 'Algarve', 'Portugal'),</code><br><code>('Petisqueira 3 em Pipa', 'Petiscos', 'Rua do Prior 3', 'Faro', 'Algarve', 'Portugal'),</code><br><code>('Casa da Rocha', 'Marisqueira', 'Praia da Rocha', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('Sueste', 'Portuguesa tradicional', 'Rua Infante D. Henrique 55', 'Ferragudo', 'Algarve', 'Portugal'),</code><br><code>('Vai e Volta', 'Grelhados de peixe', 'Avenida 5 de Outubro', 'Olhao', 'Algarve', 'Portugal'),</code><br><code>('Matuya Sushi', 'Japonesa', 'Avenida Beira Mar', 'Armacao de Pera', 'Algarve', 'Portugal'),</code><br><code>('Sexy Meat', 'Churrascaria', 'Rua Almeida Garrett', 'Albufeira', 'Algarve', 'Portugal'),</code><br><code>('BJ''s Oceanside', 'Peixes e mariscos', 'Praia do Almargem', 'Quarteira', 'Algarve', 'Portugal'),</code><br><code>('Oliwander''s', 'Portuguesa contemporanea', 'Rua da Porta de Loule', 'Loule', 'Algarve', 'Portugal'),</code><br><code>('Taberna da Mare', 'Petiscos', 'Rua Direita 70', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('Tasca da Lota', 'Portuguesa tradicional', 'Avenida dos Descobrimentos', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Pomò La Pasta Italiana', 'Italiana', 'Rua 25 de Abril', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Go Bao', 'Asiatica', 'Rua Conselheiro Joaquim Machado', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Gelicia', 'Gelataria', 'Rua Candido dos Reis', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Ti Raul', 'Snacks e saudavel', 'Rua da Praia', 'Arrifana', 'Algarve', 'Portugal'),</code><br><code>('Restaurante do Cabrita', 'Marisqueira', 'Rua da Igreja', 'Carrapateira', 'Algarve', 'Portugal'),</code><br><code>('A Sereia', 'Rodizio de peixe', 'Rua Comandante Matoso', 'Sagres', 'Algarve', 'Portugal'),</code><br><code>('O Teodosio', 'Frango piri-piri', 'Rua 25 de Abril', 'Guia', 'Algarve', 'Portugal'),</code><br><code>('Casa do Polvo Tasquinha', 'Polvo e mariscos', 'Rua da Republica', 'Santa Luzia', 'Algarve', 'Portugal'),</code><br><code>('Polvo &amp; Companhia', 'Polvo e mariscos', 'Rua da Liberdade', 'Santa Luzia', 'Algarve', 'Portugal'),</code><br><code>('Ria', 'Peixes e mariscos', 'Anantara Vilamoura', 'Vilamoura', 'Algarve', 'Portugal'),</code><br><code>('Café Correia', 'Marisqueira', 'Rua 1º de Maio', 'Vila do Bispo', 'Algarve', 'Portugal'),</code><br><code>('Bon Bon', 'Gastronomia contemporanea', 'Rua do Monte Carvoeiro', 'Carvoeiro', 'Algarve', 'Portugal'),</code><br><code>('Prato Cheio', 'Portuguesa tradicional', 'Rua Dr. Francisco Sa Carneiro 23A', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Casa da Igreja', 'Marisqueira', 'Rua de Cacela Velha 2', 'Cacela Velha', 'Algarve', 'Portugal'),</code><br><code>('Vila Lisa', 'Portuguesa tradicional', 'Rua da Hortinha', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('O Tonel', 'Portuguesa tradicional', 'Rua Dr. Augusto da Silva Carvalho', 'Tavira', 'Algarve', 'Portugal'),</code><br><code>('Cha Cha Cha', 'Portuguesa contemporanea', 'Rua Vasco da Gama', 'Olhao', 'Algarve', 'Portugal'),</code><br><code>('O Cantinho da Cristina', 'Portuguesa tradicional', 'Rua do Comercio', 'Olhao', 'Algarve', 'Portugal'),</code><br><code>('Restaurante Noelia', 'Peixes e mariscos', 'Avenida Ria Formosa', 'Cabanas de Tavira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante Azenha do Mar', 'Marisqueira', 'Estrada Nacional 120', 'Aljezur', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Camilo', 'Peixes e mariscos', 'Praia do Camilo', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Marinheiro', 'Mediterranea', 'Rua da Torre Velha', 'Albufeira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pescador', 'Peixes e mariscos', 'Rua 5 de Outubro', 'Quarteira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Forja', 'Portuguesa tradicional', 'Rua da Barca', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Alambique', 'Portuguesa tradicional', 'Estrada Nacional 125', 'Almancil', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Cais', 'Peixes e mariscos', 'Avenida dos Descobrimentos', 'Vilamoura', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Galeão', 'Portuguesa tradicional', 'Rua da Praia', 'Armação de Pêra', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Tasca', 'Petiscos', 'Rua do Comércio', 'Loulé', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Farol', 'Peixes e mariscos', 'Praia do Farol', 'Ilha do Farol', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pátio', 'Portuguesa tradicional', 'Rua das Flores', 'Tavira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Barril', 'Peixes e mariscos', 'Praia do Barril', 'Tavira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Zé', 'Portuguesa tradicional', 'Rua da Alegria', 'Faro', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pescador', 'Peixes e mariscos', 'Rua da Praia', 'Quarteira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Marisqueira', 'Marisqueira', 'Avenida Marginal', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Grelhador', 'Grelhados', 'Rua do Sol', 'Portimão', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Forno', 'Portuguesa tradicional', 'Rua do Forno', 'Silves', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Taberna', 'Petiscos', 'Rua da Taberna', 'Albufeira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Barco', 'Peixes e mariscos', 'Avenida do Mar', 'Olhão', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pátio das Laranjeiras', 'Portuguesa tradicional', 'Rua das Laranjeiras', 'Loulé', 'Algarve', 'Portugal');</code></p></td><td style="width:50%;"><p>Considerando que estamos em um ambiente com sistema operacional Windows e que o Docker Desktop já está instalado e em execução, podemos seguir com os passos a seguir.</p><h3>Instalando Redis localmente com Docker Desktop</h3><p>Execute o comando abaixo para baixar e iniciar o contêiner do Redis Server no seu ambiente Docker local.</p><pre><code class="language-plaintext">  docker run -d --name redis-server -p 6379:6379 -v redis-data:/data redis</code></pre><h3>Testar o Serviço</h3><p>O camando abixo faz um teste para ver se o Redis está em funcionamento.</p><pre><code class="language-plaintext">docker exec -it redis-server redis-cli
&gt; SET meuTeste “Bom dia Carlos Vamberto Filho”
&gt; GET meuTest
&gt; exit</code></pre><h3>Installing MySQL Locally with Docker Desktop</h3><p>Vamos também rodar em um container o MySQL que usaremos nesta aplicação. Com o comando abaixo vamos rodar o MySQL na porta padrão 3306.</p><pre><code class="language-plaintext">docker run --name mysql-local -e MYSQL_ROOT_PASSWORD=Senha123  -p 3306:3306</code></pre><h3>Criando a tabela Restaurante</h3><p>Usei a aplicação MySQL Workbench para me conectar ao servidor do MySQL e criar a tabela de restaurantes em uma nova base de dados chamada “meubanco”.</p><pre><code class="language-plaintext">-- Cria o banco de dados se ainda não existir
CREATE DATABASE IF NOT EXISTS meubanco CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
-- Usa o banco de dados
USE meubanco;
-- Criação da tabela restaurantes
CREATE TABLE IF NOT EXISTS restaurantes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Tipo VARCHAR(100),
    Endereco VARCHAR(255),
    Cidade VARCHAR(100),
    Regiao VARCHAR(100),
    Pais VARCHAR(100)
);</code></pre><h3>Insere registros na tabela Restaurantes</h3><p>Aqui vamos criar registros para 50 restaurantes existentes na região do Algarve em Portugal para demonstrar a API. Execute o script abaixo para inserir os registros:</p><pre><code class="language-plaintext">USE meubanco;
</code></pre><p><code>INSERT INTO restaurantes (Nome, Tipo, Endereco, Cidade, Regiao, Pais) VALUES</code><br><code>('Luar da Foia', 'Portuguesa tradicional', 'Estrada da Foia', 'Monchique', 'Algarve', 'Portugal'),</code><br><code>('Petisqueira 3 em Pipa', 'Petiscos', 'Rua do Prior 3', 'Faro', 'Algarve', 'Portugal'),</code><br><code>('Casa da Rocha', 'Marisqueira', 'Praia da Rocha', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('Sueste', 'Portuguesa tradicional', 'Rua Infante D. Henrique 55', 'Ferragudo', 'Algarve', 'Portugal'),</code><br><code>('Vai e Volta', 'Grelhados de peixe', 'Avenida 5 de Outubro', 'Olhao', 'Algarve', 'Portugal'),</code><br><code>('Matuya Sushi', 'Japonesa', 'Avenida Beira Mar', 'Armacao de Pera', 'Algarve', 'Portugal'),</code><br><code>('Sexy Meat', 'Churrascaria', 'Rua Almeida Garrett', 'Albufeira', 'Algarve', 'Portugal'),</code><br><code>('BJ''s Oceanside', 'Peixes e mariscos', 'Praia do Almargem', 'Quarteira', 'Algarve', 'Portugal'),</code><br><code>('Oliwander''s', 'Portuguesa contemporanea', 'Rua da Porta de Loule', 'Loule', 'Algarve', 'Portugal'),</code><br><code>('Taberna da Mare', 'Petiscos', 'Rua Direita 70', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('Tasca da Lota', 'Portuguesa tradicional', 'Avenida dos Descobrimentos', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Pomò La Pasta Italiana', 'Italiana', 'Rua 25 de Abril', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Go Bao', 'Asiatica', 'Rua Conselheiro Joaquim Machado', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Gelicia', 'Gelataria', 'Rua Candido dos Reis', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Ti Raul', 'Snacks e saudavel', 'Rua da Praia', 'Arrifana', 'Algarve', 'Portugal'),</code><br><code>('Restaurante do Cabrita', 'Marisqueira', 'Rua da Igreja', 'Carrapateira', 'Algarve', 'Portugal'),</code><br><code>('A Sereia', 'Rodizio de peixe', 'Rua Comandante Matoso', 'Sagres', 'Algarve', 'Portugal'),</code><br><code>('O Teodosio', 'Frango piri-piri', 'Rua 25 de Abril', 'Guia', 'Algarve', 'Portugal'),</code><br><code>('Casa do Polvo Tasquinha', 'Polvo e mariscos', 'Rua da Republica', 'Santa Luzia', 'Algarve', 'Portugal'),</code><br><code>('Polvo &amp; Companhia', 'Polvo e mariscos', 'Rua da Liberdade', 'Santa Luzia', 'Algarve', 'Portugal'),</code><br><code>('Ria', 'Peixes e mariscos', 'Anantara Vilamoura', 'Vilamoura', 'Algarve', 'Portugal'),</code><br><code>('Café Correia', 'Marisqueira', 'Rua 1º de Maio', 'Vila do Bispo', 'Algarve', 'Portugal'),</code><br><code>('Bon Bon', 'Gastronomia contemporanea', 'Rua do Monte Carvoeiro', 'Carvoeiro', 'Algarve', 'Portugal'),</code><br><code>('Prato Cheio', 'Portuguesa tradicional', 'Rua Dr. Francisco Sa Carneiro 23A', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Casa da Igreja', 'Marisqueira', 'Rua de Cacela Velha 2', 'Cacela Velha', 'Algarve', 'Portugal'),</code><br><code>('Vila Lisa', 'Portuguesa tradicional', 'Rua da Hortinha', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('O Tonel', 'Portuguesa tradicional', 'Rua Dr. Augusto da Silva Carvalho', 'Tavira', 'Algarve', 'Portugal'),</code><br><code>('Cha Cha Cha', 'Portuguesa contemporanea', 'Rua Vasco da Gama', 'Olhao', 'Algarve', 'Portugal'),</code><br><code>('O Cantinho da Cristina', 'Portuguesa tradicional', 'Rua do Comercio', 'Olhao', 'Algarve', 'Portugal'),</code><br><code>('Restaurante Noelia', 'Peixes e mariscos', 'Avenida Ria Formosa', 'Cabanas de Tavira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante Azenha do Mar', 'Marisqueira', 'Estrada Nacional 120', 'Aljezur', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Camilo', 'Peixes e mariscos', 'Praia do Camilo', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Marinheiro', 'Mediterranea', 'Rua da Torre Velha', 'Albufeira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pescador', 'Peixes e mariscos', 'Rua 5 de Outubro', 'Quarteira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Forja', 'Portuguesa tradicional', 'Rua da Barca', 'Portimao', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Alambique', 'Portuguesa tradicional', 'Estrada Nacional 125', 'Almancil', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Cais', 'Peixes e mariscos', 'Avenida dos Descobrimentos', 'Vilamoura', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Galeão', 'Portuguesa tradicional', 'Rua da Praia', 'Armação de Pêra', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Tasca', 'Petiscos', 'Rua do Comércio', 'Loulé', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Farol', 'Peixes e mariscos', 'Praia do Farol', 'Ilha do Farol', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pátio', 'Portuguesa tradicional', 'Rua das Flores', 'Tavira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Barril', 'Peixes e mariscos', 'Praia do Barril', 'Tavira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Zé', 'Portuguesa tradicional', 'Rua da Alegria', 'Faro', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pescador', 'Peixes e mariscos', 'Rua da Praia', 'Quarteira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Marisqueira', 'Marisqueira', 'Avenida Marginal', 'Lagos', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Grelhador', 'Grelhados', 'Rua do Sol', 'Portimão', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Forno', 'Portuguesa tradicional', 'Rua do Forno', 'Silves', 'Algarve', 'Portugal'),</code><br><code>('Restaurante A Taberna', 'Petiscos', 'Rua da Taberna', 'Albufeira', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Barco', 'Peixes e mariscos', 'Avenida do Mar', 'Olhão', 'Algarve', 'Portugal'),</code><br><code>('Restaurante O Pátio das Laranjeiras', 'Portuguesa tradicional', 'Rua das Laranjeiras', 'Loulé', 'Algarve', 'Portugal');</code></p></td></tr></tbody></table>

## Clean Architecture

<table><tbody><tr><td><p><strong>Clean Architecture</strong>, proposed by Robert C. Martin (known as Uncle Bob), is an architectural style that aims to organize the code in such a way that the application's business rules are independent of frameworks, external libraries, databases, and user interfaces.</p><p><br>Its main goal is to promote <strong>separation of concerns, maintainability, testability, </strong>and<strong> scalability</strong>. To achieve this, the architecture is divided into well-defined layers, each with a specific role:</p><ul><li><strong>Domínio (Entities):</strong> contains the core business rules of the application.</li><li><strong>Casos de uso (Use Cases/Application):</strong> orchestrates the application's behavior and specific business rules.</li><li><strong>Interface de entrada (InterfaceAdapters/API):</strong> handles incoming requests (e.g., API controllers).</li><li><strong>Infraestrutura (Infrastructure):</strong> implements technical details such as database access, external services, caching, etc.</li></ul><p>One of the main advantages of Clean Architecture is <strong>technology independence</strong>. This means that, for example, it's possible to change the database (from SQL Server to MySQL, or Redis for caching) without affecting the business rules.</p><p><br>Adopting Clean Architecture helps development teams build more organized, decoupled, and easy-to-evolve systems—essential characteristics for modern applications, especially when considering scalable REST APIs, as proposed in this article.</p></td><td><p>A <strong>Clean Architecture</strong>, proposta por Robert C. Martin (conhecido como Uncle Bob), é um estilo arquitetural que busca organizar o código de forma que as regras de negócio da aplicação sejam independentes de frameworks, bibliotecas externas, banco de dados e interfaces de usuário.</p><p>&nbsp;</p><p>Seu principal objetivo é promover <strong>separação de responsabilidades</strong>, <strong>facilidade de manutenção</strong>, <strong>testabilidade</strong> e <strong>escalabilidade</strong>. Para isso, a arquitetura é dividida em camadas bem definidas, onde cada uma possui seu papel específico:</p><ul><li><strong>Domínio (Entities):</strong> contém as regras de negócio mais centrais da aplicação.</li><li><strong>Casos de uso (Use Cases/Application):</strong> orquestra o comportamento da aplicação e as regras específicas de negócio.</li><li><strong>Interface de entrada (InterfaceAdapters/API):</strong> recebe as requisições (ex: controllers da API).</li><li><strong>Infraestrutura (Infrastructure):</strong> implementa detalhes técnicos como acesso ao banco de dados, serviços externos, cache etc.</li></ul><p>Uma das principais vantagens da Clean Architecture é a <strong>independência de tecnologia</strong>. Isso significa que, por exemplo, é possível trocar o banco de dados (de SQL Server para MySQL, ou para Redis como cache) sem afetar as regras de negócio.</p><p>&nbsp;</p><p>Adotar Clean Architecture ajuda times de desenvolvimento a construírem sistemas mais organizados, desacoplados e fáceis de evoluir — características essenciais para aplicações modernas, especialmente quando pensamos em APIs REST escaláveis, como a proposta deste artigo.</p></td></tr></tbody></table>

## Visual Studio 2022

<table><tbody><tr><td><p>Now let's create a blank solution named <strong>RestaurantesAPI</strong> in Visual Studio 2022 and add the following projects:</p><ol><li><strong>Restaurantes.Application</strong> – Class Library - .NET 8</li><li><strong>Restaurantes.Domain</strong> – Class Library - .NET 8</li><li><strong>Restaurantes.Infrastructure</strong> – Class Library -&nbsp;.NET 8</li><li><strong>Restaurantes.API</strong> – ASP.NET Core Web API - .NET 8</li></ol><h2>Restaurantes.Domain</h2><p>Create the <strong>Restaurante.cs</strong> class in the <strong>Entities</strong> folder.</p><pre><code class="language-plaintext">public class Restaurante
{
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Regiao { get; set; }
        public string Pais { get; set; }
}</code></pre><p>Create the <strong>IRestauranteRepository.cs</strong> interface in the <strong>Interfaces</strong> folder.</p><pre><code class="language-plaintext">using Restaurantes.Domain.Entities;
namespace Restaurantes.Domain.Interfaces
{
    public interface IRestauranteRepository
    {
        Task&amp;lt;IEnumerable&amp;lt;Restaurante&amp;gt;&amp;gt; GetFilteredAsync(string? nome, string? tipo, string? cidade);
    }
} </code></pre><h2>Restaurantes.Application</h2><p>In <strong>Restaurantes.Application</strong>, add a <strong>Project Reference</strong> to the <strong>Restaurantes.Domain</strong> project by right-clicking on the <strong>Restaurantes.Application</strong> project and selecting <strong>Add &gt; Project Reference...</strong></p><p>&nbsp;</p><p>Then, add the following <strong>IRestauranteService.cs</strong> interface in the <strong>Services</strong> folder.</p><pre><code class="language-cs">using Restaurantes.Application.Requests;
using Restaurantes.Domain.Entities;

</code></pre><p><code>namespace Restaurantes.Application.Services</code><br><code>{</code><br><code>&nbsp; &nbsp; public interface IRestauranteService</code><br><code>&nbsp; &nbsp; {</code><br><code>&nbsp; &nbsp; &nbsp; &nbsp; Task&lt;IEnumerable&lt;Restaurante&gt;&gt; GetFilteredAsync(GetRestaurantesRequest request);</code><br><code>&nbsp; &nbsp; }</code><br><code>}</code><br>&nbsp;</p><p>Create a class named <strong>GetRestaurantesRequest.cs</strong> in the <strong>Requests</strong> folder.</p><pre><code class="language-cs">namespace Restaurantes.Application.Requests
{
    public class GetRestaurantesRequest
    {
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Cidade { get; set; }
    }
}
</code></pre><h2>Restaurantes.Infrastructure Project</h2><p>Add a <strong>Project Reference</strong> in <strong>Restaurantes.Infrastructure</strong> to the <strong>Restaurantes.Application</strong> project.</p><p>Using <strong>NuGet</strong>, add the package <strong>Microsoft.Extensions.Caching.StackExchangeRedis</strong> to the <strong>Restaurantes.Infrastructure</strong> project.</p><p>&nbsp;</p><p>Add the <strong>MyDbContext.cs</strong> class in the <strong>Context</strong> folder.</p><pre><code class="language-cs">using Microsoft.EntityFrameworkCore;
using Restaurantes.Domain.Entities;
namespace Restaurantes.Infrastructure.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions&lt;MyDbContext&gt; options) : base(options) { }
    public DbSet&amp;lt;Restaurante&amp;gt; Restaurantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity&amp;lt;Restaurante&amp;gt;().ToTable("restaurantes");
    }
}
</code></pre><p><code>}</code><br>&nbsp;</p><p>Create the <strong>RestauranteRepository.cs</strong> class that will implement the <strong>IRestauranteRepository.cs</strong> interface.</p><pre><code class="language-cs">using Microsoft.EntityFrameworkCore;
using Restaurantes.Domain.Entities;
using Restaurantes.Domain.Interfaces;
using Restaurantes.Infrastructure.Context;
namespace Restaurantes.Infrastructure.Repositories
{
    public class RestauranteRepository : IRestauranteRepository
    {
        private readonly MyDbContext _context;
    public RestauranteRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task&amp;lt;IEnumerable&amp;lt;Restaurante&amp;gt;&amp;gt; GetFilteredAsync(string? nome, string? tipo, string? cidade)
    {
        var query = _context.Restaurantes.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(r =&amp;gt; r.Nome.Contains(nome));
        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(r =&amp;gt; r.Tipo.Contains(tipo));
        if (!string.IsNullOrEmpty(cidade))
            query = query.Where(r =&amp;gt; r.Cidade.Contains(cidade));

        return await query.ToListAsync();
    }
}
</code></pre><p><code>}</code><br>&nbsp;</p><h2>Restaurantes.API Project</h2><p>Add a <strong>Project Reference</strong> in the <strong>Restaurantes.API</strong> project to the <strong>Restaurantes.Infrastructure</strong> project.</p><p>Add the following <strong>ConnectionStrings</strong> section to the <strong>appsettings.json</strong> file:</p><pre><code class="language-javascript">"ConnectionStrings": {
  "Redis": "localhost:6379",
  "DefaultConnection": "server=localhost;port=3306;database=meubanco;user=root;password=Senha123"
}
</code></pre><p>Add the following code in the <strong>Program.cs</strong> file <strong>before</strong> the line "var app = builder.Build();".</p><pre><code class="language-cs">// Configura a conexão com a base de dados MySQL
builder.Services.AddDbContext&lt;MyDbContext&gt;(options =&gt;
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);
// Configura o Redis como cache distribuído
builder.Services.AddStackExchangeRedisCache(options =&gt;
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "RestaurantesCache:";
});
</code></pre><p><code>// Configura a injeção de dependências para o repositório e serviço de restaurantes</code><br><code>builder.Services.AddScoped&lt;IRestauranteRepository, RestauranteRepository&gt;();</code><br><code>builder.Services.AddScoped&lt;IRestauranteService, RestauranteService&gt;();</code><br>&nbsp;</p><h3>Creating the Restaurantes API Endpoint.</h3><p>In the <strong>Controllers</strong> folder, create the <strong>RestaurantesController.cs</strong> controller.</p><pre><code class="language-cs">using Microsoft.AspNetCore.Mvc;
using Restaurantes.Application.Requests;
using Restaurantes.Application.Services;
using Restaurantes.Domain.Entities;
namespace Restaurantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantesController : ControllerBase
    {
        private readonly IRestauranteService _restauranteService;
    public RestaurantesController(IRestauranteService restauranteService)
    {
        _restauranteService = restauranteService;
    }

    /// &amp;lt;summary&amp;gt;
    /// Lista restaurantes com base nos filtros fornecidos (Nome, Tipo e Cidade).
    /// &amp;lt;/summary&amp;gt;
    [HttpGet]
    public async Task&amp;lt;ActionResult&amp;lt;IEnumerable&amp;lt;Restaurante&amp;gt;&amp;gt;&amp;gt; Get(
        [FromQuery] string? nome, [FromQuery] string? tipo, [FromQuery] string? cidade)
    {
        var request = new GetRestaurantesRequest
        {
            Nome = nome,
            Tipo = tipo,
            Cidade = cidade
        };

        var result = await _restauranteService.GetFilteredAsync(request);

        return Ok(result);
    }
}
</code></pre><p><code>}</code><br>&nbsp;</p><p>&nbsp;</p></td><td><p>Agora vamos criar uma solução em Branco de nome <strong>RestauratesAPI</strong> no Visual Studio 2022 e adicionar o seguintes projetos:</p><ol><li><strong>Restaurantes.Application</strong> – Class Library - .NET 8</li><li><strong>Restaurantes.Domain</strong> – Class Library - .NET 8</li><li><strong>Restaurantes.Infrastructure</strong> – Class Library -&nbsp;.NET 8</li><li><strong>Restaurantes.API</strong> – ASP.NET Core Web API - .NET 8</li></ol><h2>Restaurantes.Domain</h2><p>Crie a classe <strong>Restaurante.cs</strong> na pasta <strong>Entities</strong></p><pre><code class="language-cs">namespace Restaurantes.Domain.Entities
{
    public class Restaurante
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Regiao { get; set; }
        public string Pais { get; set; }
    }
}
</code></pre><p>Crie a Interface <strong>IrestauranteRepository.cs&nbsp;</strong>na pasta <strong>Interfaces</strong></p><pre><code class="language-cs">using Restaurantes.Domain.Entities;
</code></pre><p><code>namespace Restaurantes.Domain.Interfaces</code><br><code>{</code><br><code>&nbsp; &nbsp; public interface IRestauranteRepository</code><br><code>&nbsp; &nbsp; {</code><br><code>&nbsp; &nbsp; &nbsp; &nbsp; Task&lt;IEnumerable&lt;Restaurante&gt;&gt; GetFilteredAsync(string? nome, string? tipo, string? cidade);</code><br><code>&nbsp; &nbsp; }</code><br><code>}</code><br>&nbsp;</p><h2>Restaurantes.Application</h2><p>No Restaurantes.Application, adicione um <strong>Project Reference</strong> para o projeto <strong>Restaurantes.Domain</strong> usando o botão direito do mouse sobre o projeto <strong>Restaurantes.Application</strong> escolhendo <strong>Add&nbsp;à Project Reference...</strong></p><p>&nbsp;</p><p>Adicione a seguinte interface <strong>IrestauranteService.cs</strong> abaixo na pasta <strong>Services</strong>.</p><pre><code class="language-cs">using Restaurantes.Application.Requests;
using Restaurantes.Domain.Entities;
</code></pre><p><code>namespace Restaurantes.Application.Services</code><br><code>{</code><br><code>&nbsp; &nbsp; public interface IRestauranteService</code><br><code>&nbsp; &nbsp; {</code><br><code>&nbsp; &nbsp; &nbsp; &nbsp; Task&lt;IEnumerable&lt;Restaurante&gt;&gt; GetFilteredAsync(GetRestaurantesRequest request);</code><br><code>&nbsp; &nbsp; }</code><br><code>}</code><br>&nbsp;</p><p>Crie uma classe na pasta <strong>Requests</strong> de nome <strong>GetRestaurantesRequest.cs</strong></p><pre><code class="language-cs">namespace Restaurantes.Application.Requests
{
    public class GetRestaurantesRequest
    {
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Cidade { get; set; }
    }
}
</code></pre><h2>Projeto Restaurantes.Infrastructure</h2><p>Adicione um <strong>Project Reference</strong> no <strong>Restaurantes.Infrastructure</strong> para o projeto <strong>Restaurantes.Application</strong></p><p>Usando o <strong>Nuget</strong> adicione o pacote <strong>Microsoft.Extensions.Caching.StackExchangeRedis&nbsp;</strong>no projeto <strong>Restaurantes.Infrastructure</strong></p><p>&nbsp;</p><p>Adicione a classe <strong>MyDbContext.cs</strong> na pasta <strong>Context</strong></p><pre><code class="language-cs">using Microsoft.EntityFrameworkCore;
using Restaurantes.Domain.Entities;
namespace Restaurantes.Infrastructure.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions&lt;MyDbContext&gt; options) : base(options) { }
    public DbSet&amp;lt;Restaurante&amp;gt; Restaurantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity&amp;lt;Restaurante&amp;gt;().ToTable("restaurantes");
    }
}
</code></pre><p><code>}</code><br>&nbsp;</p><p>Crie a classe <strong>RestauranteRepository.cs</strong> que vai implementar a interface <strong>RestauranteRepository.cs</strong></p><pre><code class="language-cs">using Microsoft.EntityFrameworkCore;
using Restaurantes.Domain.Entities;
using Restaurantes.Domain.Interfaces;
using Restaurantes.Infrastructure.Context;
namespace Restaurantes.Infrastructure.Repositories
{
    public class RestauranteRepository : IRestauranteRepository
    {
        private readonly MyDbContext _context;
    public RestauranteRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task&amp;lt;IEnumerable&amp;lt;Restaurante&amp;gt;&amp;gt; GetFilteredAsync(string? nome, string? tipo, string? cidade)
    {
        var query = _context.Restaurantes.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(r =&amp;gt; r.Nome.Contains(nome));
        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(r =&amp;gt; r.Tipo.Contains(tipo));
        if (!string.IsNullOrEmpty(cidade))
            query = query.Where(r =&amp;gt; r.Cidade.Contains(cidade));

        return await query.ToListAsync();
    }
}
</code></pre><p><code>}</code><br>&nbsp;</p><p>&nbsp;</p><h2>Projeto Restaurantes.API</h2><p>Adicione uma <strong>Project Reference</strong> no projeto <strong>Restaurantes.API</strong> para o projeto <strong>Restaruantes.Infrastrucure</strong></p><p>Adicione o código abaixo relacionado a <strong>ConnectionStrings</strong> dentro do arquivo <strong>appsettings.json</strong></p><pre><code class="language-javascript">"ConnectionStrings": {
  "Redis": "localhost:6379",
  "DefaultConnection": "server=localhost;port=3306;database=meubanco;user=root;password=Senha123"
} 
</code></pre><p>Adicione o código abaixo na class <strong>Program.cs</strong> antes do código&nbsp;“var app = builder.Build();”</p><pre><code class="language-cs">// Configura a conexão com a base de dados MySQL
builder.Services.AddDbContext&lt;MyDbContext&gt;(options =&gt;
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configura o Redis como cache distribuído
builder.Services.AddStackExchangeRedisCache(options =&gt;
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "RestaurantesCache:";
});
// Configura a injeção de dependências para o repositório e serviço de restaurantes
builder.Services.AddScoped&lt;IRestauranteRepository, RestauranteRepository&gt;();
builder.Services.AddScoped&lt;IRestauranteService, RestauranteService&gt;();
</code></pre><p>&nbsp;</p><h3>Criando o EndPoint da API de Restaurantes</h3><p>Na pasta Controllers crie o controller RestaurantesController.cs</p><pre><code class="language-cs">using Microsoft.AspNetCore.Mvc;
using Restaurantes.Application.Requests;
using Restaurantes.Application.Services;
using Restaurantes.Domain.Entities;
namespace Restaurantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantesController : ControllerBase
    {
        private readonly IRestauranteService _restauranteService;
    public RestaurantesController(IRestauranteService restauranteService)
    {
        _restauranteService = restauranteService;
    }

    /// &amp;lt;summary&amp;gt;
    /// Lista restaurantes com base nos filtros fornecidos (Nome, Tipo e Cidade).
    /// &amp;lt;/summary&amp;gt;
    [HttpGet]
    public async Task&amp;lt;ActionResult&amp;lt;IEnumerable&amp;lt;Restaurante&amp;gt;&amp;gt;&amp;gt; Get(
        [FromQuery] string? nome, [FromQuery] string? tipo, [FromQuery] string? cidade)
    {
        var request = new GetRestaurantesRequest
        {
            Nome = nome,
            Tipo = tipo,
            Cidade = cidade
        };

        var result = await _restauranteService.GetFilteredAsync(request);

        return Ok(result);
    }
}
</code></pre><p><code>}</code><br>&nbsp;</p></td></tr></tbody></table>
