# API Produtos de Mercado AP2

## Tema

Produtos de Mercado: Sistema de Gerenciamento de Estoque.

## Objetivo

O objetivo deste projeto é dar continuidade à AP1, que utilizava uma API com dados em memória, adicionando persistência em banco de dados e uma interface web para facilitar o gerenciamento dos produtos.

## Tecnologias utilizadas

C#, ASP.NET Core, .NET 10, Minimal API, Entity Framework Core 10, SQLite, HTML, CSS, JavaScript, Bruno, Git e GitHub.

## Funcionalidades

1. Cadastro de produtos.
2. Listagem dos produtos cadastrados.
3. Pesquisa por nome, categoria ou marca.
4. Edição das informações dos produtos.
5. Exclusão de produtos com confirmação.
6. Armazenamento dos dados em banco SQLite.
7. Validação dos dados informados.
8. Interface web responsiva com notificações sobre as operações realizadas.

## Como executar o projeto

### 1. Restaurar as dependências

dotnet restore


### 2. Compilar o projeto


dotnet build


### 3. Executar a aplicação

dotnet run --urls http://localhost:5112

### 4. Acessar o sistema

Interface do sistema:

http://localhost:5112

Listagem de produtos:

http://localhost:5112/api/produtos

Busca de produto pelo ID:

http://localhost:5112/api/produtos/1

## Endpoints da API

| Método | Rota | Descrição |

| GET | /api/produtos | Lista todos os produtos |
| GET | /api/produtos/{id} | Busca um produto pelo ID |
| POST | /api/produtos | Cadastra um produto |
| PUT | /api/produtos/{id} | Atualiza um produto |
| DELETE | /api/produtos/{id} | Exclui um produto |

## Exemplo de cadastro


{
  "nome": "Café Torrado 500g",
  "categoria": "Bebidas",
  "marca": "Melitta",
  "preco": 18.90,
  "estoque": 25
}


## Testes com Bruno

Os testes foram realizados utilizando o Bruno para verificar o funcionamento da API e as operações de listagem, busca, cadastro, edição e exclusão de produtos.

Os arquivos dos testes estão na pasta bruno do projeto.

## Vídeo de demonstração

O vídeo mostra o funcionamento do sistema, incluindo o cadastro, a edição, a pesquisa e a exclusão de produtos, além dos testes realizados no Bruno.

Link para assistir:

https://drive.google.com/file/d/1tD4Zq5TxNlCXTr8GzxbmFM5B603jzMaI/view?usp=sharing

## Autor

Projeto desenvolvido para a avaliação AP2.