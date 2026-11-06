using System.Globalization;
using MiniEcommerce.Core;

var catalogo = new Catalogo();
var carrinho = new CarrinhoDeCompras();

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
{
    ExibirMenu();
    var opcao = LerTexto("Escolha uma opção: ");

    if (opcao is null || opcao == "0")
    {
        Console.WriteLine("Até logo!");
        break;
    }

    try
    {
        switch (opcao)
        {
            case "1": CadastrarProduto(); break;
            case "2": ListarCatalogo(); break;
            case "3": AdicionarAoCarrinho(); break;
            case "4": VerCarrinho(); break;
            case "5": RemoverDoCarrinho(); break;
            case "6": FinalizarPedido(); break;
            default: Console.WriteLine("Opção inválida. Tente novamente."); break;
        }
    }
    catch (EstoqueInsuficienteException ex)
    {
        Console.WriteLine($"[Estoque] {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"[Dados inválidos] {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"[Operação inválida] {ex.Message}");
    }
}

void ExibirMenu()
{
    Console.WriteLine();
    Console.WriteLine("===== MiniEcommerce =====");
    Console.WriteLine("1 - Cadastrar produto");
    Console.WriteLine("2 - Listar catálogo");
    Console.WriteLine("3 - Adicionar ao carrinho");
    Console.WriteLine("4 - Ver carrinho");
    Console.WriteLine("5 - Remover item do carrinho");
    Console.WriteLine("6 - Finalizar pedido");
    Console.WriteLine("0 - Sair");
}

void CadastrarProduto()
{
    var nome = LerTexto("Nome: ") ?? "";
    var preco = LerDecimal("Preço (ex.: 49,90): ");
    var estoque = LerInteiro("Quantidade em estoque: ");

    var produto = catalogo.Cadastrar(nome, preco, estoque);
    Console.WriteLine($"Produto '{produto.Nome}' cadastrado com o código {produto.Id}.");
}

void ListarCatalogo()
{
    if (catalogo.Listar().Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"{"Cód.",-5}{"Produto",-25}{"Preço",-15}{"Estoque",-8}");
    foreach (var p in catalogo.Listar())
        Console.WriteLine($"{p.Id,-5}{p.Nome,-25}{Formatador.Moeda(p.Preco),-15}{p.Estoque,-8}");
}

void AdicionarAoCarrinho()
{
    var produto = SelecionarProduto("Código do produto: ", catalogo.Listar());
    if (produto is null) return;

    var quantidade = LerInteiro("Quantidade desejada: ");
    carrinho.Adicionar(produto, quantidade);
    Console.WriteLine($"{quantidade}x {produto.Nome} adicionado(s) ao carrinho.");
}

void VerCarrinho()
{
    if (CarrinhoVazio()) return;

    Console.WriteLine();
    foreach (var item in carrinho.Itens)
        Console.WriteLine($"[{item.Produto.Id}] {item.Quantidade}x {item.Produto.Nome} - {Formatador.Moeda(item.Produto.Preco)} = {Formatador.Moeda(item.Subtotal)}");
    Console.WriteLine($"Total parcial: {Formatador.Moeda(carrinho.Total)}");
}

void RemoverDoCarrinho()
{
    if (CarrinhoVazio()) return;

    VerCarrinho();
    var produto = SelecionarProduto("Código do item a remover: ", carrinho.Itens.Select(i => i.Produto).ToList());
    if (produto is null) return;

    Console.WriteLine(carrinho.Remover(produto) ? "Item removido do carrinho." : "Item não encontrado no carrinho.");
}

void FinalizarPedido()
{
    var pedido = Pedido.Finalizar(carrinho);
    Console.WriteLine();
    Console.WriteLine(pedido.GerarResumo());
    Console.WriteLine("Compra realizada com sucesso! Estoque atualizado.");
}

bool CarrinhoVazio()
{
    if (carrinho.Itens.Count > 0) return false;

    Console.WriteLine("O carrinho está vazio.");
    return true;
}

Produto? SelecionarProduto(string mensagem, IReadOnlyList<Produto> origem)
{
    var id = LerInteiro(mensagem);
    var produto = origem.FirstOrDefault(p => p.Id == id);

    if (produto is null)
        Console.WriteLine("Produto não encontrado.");

    return produto;
}

string? LerTexto(string mensagem)
{
    Console.Write(mensagem);
    return Console.ReadLine()?.Trim();
}

int LerInteiro(string mensagem)
{
    while (true)
    {
        var entrada = LerTexto(mensagem);
        if (entrada is null) throw new InvalidOperationException("Entrada encerrada.");
        if (int.TryParse(entrada, NumberStyles.Integer, CultureInfo.InvariantCulture, out var valor))
            return valor;
        Console.WriteLine("Valor inválido. Digite um número inteiro.");
    }
}

decimal LerDecimal(string mensagem)
{
    while (true)
    {
        var entrada = LerTexto(mensagem);
        if (entrada is null) throw new InvalidOperationException("Entrada encerrada.");
        // aceita "49,90" e "49.90"
        if (decimal.TryParse(entrada.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var valor))
            return valor;
        Console.WriteLine("Valor inválido. Digite um número (ex.: 49,90).");
    }
}