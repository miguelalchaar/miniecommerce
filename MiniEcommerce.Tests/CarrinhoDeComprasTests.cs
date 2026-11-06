using MiniEcommerce.Core;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    private static Produto CriarProduto(string nome = "Mouse", decimal preco = 50m, int estoque = 10)
        => new(nome, preco, estoque) { Id = 1 };

    [Fact]
    public void Adicionar_QuantidadeValida_AdicionaItemAoCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();

        carrinho.Adicionar(produto, 2);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(produto, item.Produto);
        Assert.Equal(2, item.Quantidade);
    }

    [Fact]
    public void Adicionar_MesmoProdutoDuasVezes_SomaQuantidades()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();

        carrinho.Adicionar(produto, 2);
        carrinho.Adicionar(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
    }

    [Fact]
    public void Adicionar_QuantidadeIgualAoEstoque_Permitido()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 4);

        carrinho.Adicionar(produto, 4);

        Assert.Equal(4, carrinho.Itens[0].Quantidade);
    }

    [Fact]
    public void Adicionar_QuantidadeSuperiorAoEstoque_LancaEstoqueInsuficienteException()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 3);

        Assert.Throws<EstoqueInsuficienteException>(() => carrinho.Adicionar(produto, 4));
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Adicionar_SomaDeAdicoesSuperiorAoEstoque_LancaExcecaoEMantemQuantidadeAnterior()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 5);
        carrinho.Adicionar(produto, 3);

        Assert.Throws<EstoqueInsuficienteException>(() => carrinho.Adicionar(produto, 3));
        Assert.Equal(3, carrinho.Itens[0].Quantidade);
    }

    [Fact]
    public void Adicionar_ProdutoSemEstoque_LancaEstoqueInsuficienteException()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 0);

        Assert.Throws<EstoqueInsuficienteException>(() => carrinho.Adicionar(produto, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Adicionar_QuantidadeInvalida_LancaArgumentException(int quantidade)
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentException>(() => carrinho.Adicionar(CriarProduto(), quantidade));
    }

    [Fact]
    public void Adicionar_ProdutoNulo_LancaArgumentNullException()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentNullException>(() => carrinho.Adicionar(null!, 1));
    }

    [Fact]
    public void Adicionar_NaoAlteraOEstoqueDoProduto()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 10);

        carrinho.Adicionar(produto, 4);

        Assert.Equal(10, produto.Estoque);
    }

    [Fact]
    public void Subtotal_CalculaPrecoVezesQuantidade()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(CriarProduto(preco: 25.50m), 3);

        Assert.Equal(76.50m, carrinho.Itens[0].Subtotal);
    }

    [Fact]
    public void Total_SomaSubtotaisDeTodosOsItens()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Mouse", 50m, 10) { Id = 1 }, 2);   // 100
        carrinho.Adicionar(new Produto("Teclado", 120m, 10) { Id = 2 }, 1); // 120

        Assert.Equal(220m, carrinho.Total);
    }

    [Fact]
    public void Total_CarrinhoVazio_EhZero()
    {
        Assert.Equal(0m, new CarrinhoDeCompras().Total);
    }

    [Fact]
    public void Remover_ItemExistente_RetornaTrueERemoveDoCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();
        carrinho.Adicionar(produto, 2);

        var removido = carrinho.Remover(produto);

        Assert.True(removido);
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Remover_ItemInexistente_RetornaFalse()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.False(carrinho.Remover(CriarProduto()));
    }

    [Fact]
    public void Limpar_RemoveTodosOsItens()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(new Produto("Mouse", 50m, 10) { Id = 1 }, 1);
        carrinho.Adicionar(new Produto("Teclado", 120m, 10) { Id = 2 }, 1);

        carrinho.Limpar();

        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Total);
    }
}