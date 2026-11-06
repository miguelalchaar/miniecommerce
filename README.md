# MiniEcommerce

![C#](https://img.shields.io/badge/C%23-.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![xUnit](https://img.shields.io/badge/xUnit-Testes%20Unit%C3%A1rios-5E2D91?style=for-the-badge&logo=dotnet&logoColor=white)
![Git](https://img.shields.io/badge/Git-Versionamento-orange?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-Reposit%C3%B3rio-black?style=for-the-badge&logo=github&logoColor=white)
![Status](https://img.shields.io/badge/Status-Conclu%C3%ADda-green?style=for-the-badge)

Projeto desenvolvido para a disciplina de **Garantia da Qualidade de Software**, com foco na aplicação prática de **testes unitários com xUnit**, utilizando uma solução **.NET** e ferramentas de versionamento de código.

A atividade consiste no desenvolvimento de um **mini e-commerce em console**, com funcionalidades para cadastro de produtos, gerenciamento de carrinho de compras, fechamento de pedido com baixa automática de estoque e consulta de catálogo.

Além da implementação das funcionalidades, o projeto tem como objetivo compreender a importância dos testes automatizados na validação das regras de negócio e utilizar o Git com o padrão **Conventional Commits** para registrar a evolução do software.

## ▸ Conceito da atividade

A atividade consiste na criação de um repositório chamado `MiniEcommerce` e na implementação de uma solução composta por dois projetos:

- `MiniEcommerce.Console`: aplicação de console que contém as entidades, as regras de negócio e a interface de interação.
- `MiniEcommerce.Tests`: projeto de testes unitários responsável por verificar o comportamento das funcionalidades implementadas.

O projeto de aplicação concentra as regras relacionadas à loja, enquanto o projeto de testes valida essas regras de forma automatizada.

Essa separação permite testar a lógica do sistema independentemente da interação pelo terminal, facilitando a identificação de erros e a manutenção do código.

## ▸ Camadas da solução

### `MiniEcommerce.Console` — Código de Produção

O projeto `MiniEcommerce.Console` contém a aplicação principal e as regras de negócio da loja. As regras ficam na pasta `Core` (namespace `MiniEcommerce.Core`) e não dependem de entrada ou saída de dados.

Neste projeto são implementados:

- A classe `Produto`, responsável por representar os produtos, validar seus dados e controlar o estoque.
- A classe `Catalogo`, responsável por cadastrar, buscar e listar os produtos.
- A classe `CarrinhoDeCompras`, responsável por gerenciar os itens escolhidos e verificar os limites de estoque.
- As classes `ItemCarrinho` e `ItemPedido`, que representam os itens do carrinho e os itens já comprados.
- A classe `Pedido`, responsável pelo checkout, pela baixa de estoque e pelo resumo financeiro.
- A classe `Formatador`, responsável por apresentar valores em reais (`R$ 1.234,50`).
- A exceção `EstoqueInsuficienteException`, usada quando a quantidade pedida supera o estoque.
- O arquivo `Program.cs`, responsável pelo menu interativo.

### `MiniEcommerce.Tests` — Testes Unitários

O projeto `MiniEcommerce.Tests` representa a camada de validação automatizada.

Neste projeto são implementados:

- Testes com os atributos `[Fact]` e `[Theory]` com `[InlineData]`.
- Verificações com `Assert.Equal`, `Assert.True`, `Assert.False`, `Assert.Single`, `Assert.Empty`, `Assert.Contains` e `Assert.Throws`.
- Testes para impedir o cadastro de produtos com preço negativo, estoque negativo ou nome vazio.
- Testes para impedir o cadastro de produtos com nome duplicado.
- Testes para validar a adição ao carrinho e o limite de estoque, inclusive com adições repetidas do mesmo produto.
- Testes para conferir o cálculo de subtotal e total.
- Testes para validar o checkout, a baixa de estoque e a ausência de baixa parcial.

Os testes permitem verificar se o comportamento do sistema corresponde às regras estabelecidas, inclusive após futuras alterações no código.

## ▸ Funcionalidades da loja

A aplicação disponibiliza as seguintes funcionalidades:

| Funcionalidade             | Descrição                                                                    |
| -------------------------- | ---------------------------------------------------------------------------- |
| Cadastro de produtos       | Registra produtos com nome, preço e quantidade em estoque.                   |
| Validação de produtos      | Impede preço negativo, estoque menor que zero, nome vazio e nome duplicado.  |
| Carrinho de compras        | Adiciona produtos informando a quantidade desejada e permite remover itens.  |
| Controle de estoque        | Impede adicionar ao carrinho mais unidades do que o estoque disponível.      |
| Fechamento de pedido       | Processa a compra, dá baixa no estoque e calcula o valor total.              |
| Compra sem baixa parcial   | Se algum item estiver sem estoque no checkout, nenhum estoque é alterado.    |
| Catálogo e consulta        | Exibe os produtos com preços e estoques atuais.                              |

## ▸ Regras de negócio

As classes `Catalogo`, `CarrinhoDeCompras` e `Pedido` centralizam as principais regras da loja.

| Classe              | Método                                  | Retorno                | Regra                                                                                     |
| ------------------- | --------------------------------------- | ---------------------- | ----------------------------------------------------------------------------------------- |
| `Catalogo`          | `Cadastrar(nome, preco, estoque)`       | `Produto`              | Cadastra o produto quando os dados são válidos e o nome ainda não existe.                 |
| `Catalogo`          | `BuscarPorId(id)`                       | `Produto?`             | Retorna o produto com o código informado ou `null`.                                       |
| `Catalogo`          | `Listar()`                              | `IReadOnlyList<Produto>` | Retorna todos os produtos cadastrados.                                                  |
| `Catalogo`          | `ListarDisponiveis()`                   | `IReadOnlyList<Produto>` | Retorna apenas os produtos com estoque maior que zero.                                  |
| `Produto`           | `TemEstoque(quantidade)`                | `bool`                 | Indica se há estoque suficiente para a quantidade informada.                              |
| `Produto`           | `BaixarEstoque(quantidade)`             | `void`                 | Decrementa o estoque; lança exceção se a quantidade for inválida ou maior que o estoque.  |
| `CarrinhoDeCompras` | `Adicionar(produto, quantidade)`        | `void`                 | Adiciona o item somando quantidades e respeitando o estoque disponível.                   |
| `CarrinhoDeCompras` | `Remover(produto)`                      | `bool`                 | Remove o item do carrinho e informa se ele existia.                                       |
| `CarrinhoDeCompras` | `Total`                                 | `decimal`              | Soma os subtotais (preço × quantidade) de todos os itens.                                 |
| `Pedido`            | `Finalizar(carrinho)`                   | `Pedido`               | Valida o estoque, dá baixa, gera o pedido e esvazia o carrinho.                           |
| `Pedido`            | `GerarResumo()`                         | `string`               | Gera o resumo financeiro com itens, quantidades e total.                                  |

### Exemplo de funcionamento

Considere uma loja com dois produtos e um carrinho com três unidades no total:

```text
Produto 1 - Mouse    - R$ 50,00  - estoque 10
Produto 2 - Teclado  - R$ 120,00 - estoque 5

Carrinho: 2x Mouse e 1x Teclado.
```

O resumo esperado ao finalizar o pedido será semelhante a:

```text
=== Resumo do Pedido (05/11/2026 20:30) ===
2x Mouse - R$ 50,00 = R$ 100,00
1x Teclado - R$ 120,00 = R$ 120,00
--------------------------------
Total: R$ 220,00
```

Após a compra, o estoque do Mouse passa a ser 8, o do Teclado passa a ser 4 e o carrinho fica vazio.

O exemplo é ilustrativo e representa o resultado esperado para os dados apresentados.

## ▸ Testes unitários realizados

Os testes estão organizados em classes de acordo com as funcionalidades verificadas.

| Classe de teste           | Cenários validados                                                                                                  | Objetivo                                         |
| ------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------ |
| `ProdutoTests`            | Dados válidos, preço negativo, estoque negativo, nome vazio, `TemEstoque` e `BaixarEstoque`                          | Validar as regras da entidade `Produto`.         |
| `CatalogoTests`           | Cadastro válido, ids sequenciais, dados inválidos, nome duplicado, busca por id, listagem e produtos disponíveis     | Validar o cadastro e a consulta do catálogo.     |
| `CarrinhoDeComprasTests`  | Adição, soma de quantidades, limite de estoque, quantidade inválida, subtotal, total, remoção e limpeza              | Validar o carrinho e as regras de estoque.       |
| `PedidoTests`             | Baixa de estoque, total, carrinho esvaziado, itens do pedido, carrinho vazio, estoque alterado, resumo e moeda       | Validar o checkout e o resumo financeiro.        |

### Testes de produtos

Verificam se o produto aceita dados válidos e rejeita preço negativo, estoque negativo e nome vazio. Também validam a verificação de estoque e a baixa de unidades.

### Testes do catálogo

Verificam o cadastro de produtos, a geração de códigos, a rejeição de nomes duplicados e a listagem dos produtos com preço e estoque.

### Testes do carrinho

Verificam se produtos são adicionados com sucesso, se quantidades acima do estoque são bloqueadas (inclusive somando adições repetidas) e se subtotal e total são calculados corretamente.

### Testes de pedido

Verificam se o estoque é decrementado após o checkout, se o total e o resumo financeiro estão corretos e se uma compra com item sem estoque não altera nenhum produto.

### Execução dos testes

Para executar todos os testes automatizados, utilize:

```bash
dotnet test
```

O resultado esperado é que todos os testes implementados sejam executados com sucesso, sem falhas.

## ▸ Versionamento e qualidade de software

O projeto utiliza o Git para acompanhar as alterações e o padrão **Conventional Commits** para padronizar as mensagens de commit.

| Conceito                 | Aplicação no projeto                                       |
| ------------------------ | ---------------------------------------------------------- |
| **Solução (`.sln`)**     | Agrupa os projetos de aplicação e de testes.               |
| **Teste unitário**       | Valida individualmente as regras de negócio.               |
| **xUnit**                | Framework utilizado para implementar e executar os testes. |
| **`[Fact]` e `[Theory]`**| Identificam testes simples e testes com vários conjuntos de dados. |
| **`Assert`**             | Compara o comportamento obtido com o esperado.             |
| **TDD básico**           | Cada etapa começa com o commit dos testes, seguido da implementação. |
| **Repositório Git**      | Mantém o histórico de alterações do código.                |
| **Conventional Commits** | Padroniza as mensagens de commit.                          |
| **`git add`**            | Seleciona as alterações que serão registradas.             |
| **`git commit`**         | Registra um conjunto de alterações no histórico.           |
| **`git push`**           | Envia os commits para o repositório remoto.                |
| **`.gitignore`**         | Evita versionar arquivos gerados, como `bin/` e `obj/`.    |
| **README.md**            | Documenta o projeto e as instruções de execução.           |

### Padrão de commits

Os commits seguem a estrutura:

```text
tipo(escopo): descrição da alteração
```

Exemplos utilizados no desenvolvimento:

```bash
git commit -m "chore: initial project structure with console and xunit apps"

git commit -m "test(produto): add unit tests for product validation rules"

git commit -m "feat(produto): implement product entity and validation logic"

git commit -m "test(carrinho): add unit tests for cart addition and stock limits"

git commit -m "feat(carrinho): implement shopping cart logic and stock verification"

git commit -m "test(pedido): add unit tests for checkout and stock deduction"

git commit -m "feat(pedido): implement order processing and inventory update"

git commit -m "feat(console): implement interactive menu for e-commerce store"

git commit -m "refactor(core): improve code readability and organize namespaces"

git commit -m "docs: add comprehensive instructions in README.md"
```

Os commits devem ser realizados após as respectivas etapas, com as alterações correspondentes já implementadas e verificadas.

## ▸ Configuração via .NET CLI

Os comandos abaixo demonstram como criar a solução e os projetos utilizando a linha de comando.

```bash
# 1. Cria a solução
dotnet new sln -n MiniEcommerce

# 2. Cria o projeto de console
dotnet new console -n MiniEcommerce.Console

# 3. Cria o projeto de testes com xUnit
dotnet new xunit -n MiniEcommerce.Tests

# 4. Adiciona os projetos à solução
dotnet sln add MiniEcommerce.Console/MiniEcommerce.Console.csproj
dotnet sln add MiniEcommerce.Tests/MiniEcommerce.Tests.csproj

# 5. Adiciona a referência da aplicação no projeto de testes
dotnet add MiniEcommerce.Tests/MiniEcommerce.Tests.csproj reference MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

**Observação:** os comandos de criação são destinados à configuração inicial. Não é necessário executá-los novamente se a solução e os projetos já tiverem sido criados.

## ▸ Como executar

**Pré-requisito:** possuir o [SDK do .NET](https://dotnet.microsoft.com/download) instalado na máquina, na versão configurada nos projetos (`TargetFramework` dos arquivos `.csproj`).

### 1. Clonar o repositório

```bash
git clone https://github.com/miguelalchaar/MiniEcommerce
```

### 2. Acessar a pasta do projeto

```bash
cd MiniEcommerce
```

### 3. Restaurar as dependências

```bash
dotnet restore
```

### 4. Executar os testes unitários

```bash
dotnet test
```

### 5. Executar a aplicação

```bash
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

### 6. Conferir o histórico de commits

```bash
git log --oneline
```

Para verificar se existem alterações locais pendentes:

```bash
git status
```

### Menu da aplicação

| Opção | Ação                       |
| ----- | -------------------------- |
| 1     | Cadastrar produto          |
| 2     | Listar catálogo            |
| 3     | Adicionar ao carrinho      |
| 4     | Ver carrinho               |
| 5     | Remover item do carrinho   |
| 6     | Finalizar pedido           |
| 0     | Sair                       |

## ▸ Estrutura do projeto

```text
MiniEcommerce/
├── MiniEcommerce.sln
├── MiniEcommerce.Console/
│   ├── Program.cs
│   └── Core/
│       ├── Produto.cs
│       ├── Catalogo.cs
│       ├── CarrinhoDeCompras.cs
│       ├── ItemCarrinho.cs
│       ├── Pedido.cs
│       ├── ItemPedido.cs
│       ├── Formatador.cs
│       └── EstoqueInsuficienteException.cs
└── MiniEcommerce.Tests/
    ├── ProdutoTests.cs
    ├── CatalogoTests.cs
    ├── CarrinhoDeComprasTests.cs
    └── PedidoTests.cs
```

## ▸ Licença

Este projeto está destinado a fins acadêmicos e educacionais.

## ▸ Autor

**Miguel Alchaar**

Desenvolvido como atividade acadêmica da disciplina de **Garantia e Qualidade de Software**, com foco em testes unitários, regras de negócio e versionamento de código.