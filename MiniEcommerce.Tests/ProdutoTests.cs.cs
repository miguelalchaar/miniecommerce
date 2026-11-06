using MiniEcommerce.Core;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void Construtor_DadosValidos_CriaProduto()
    {
        var produto = new Produto("Teclado", 150.50m, 10);

        Assert.Equal("Teclado", produto.Nome);
        Assert.Equal(150.50m, produto.Preco);
        Assert.Equal(10, produto.Estoque);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Construtor_PrecoNegativo_LancaArgumentException(double preco)
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse", (decimal)preco, 5));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Construtor_EstoqueNegativo_LancaArgumentException(int estoque)
    {
        Assert.Throws<ArgumentException>(() => new Produto("Mouse", 50m, estoque));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_NomeVazio_LancaArgumentException(string? nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(nome!, 10m, 1));
    }

    [Fact]
    public void Construtor_PrecoZeroEEstoqueZero_SaoPermitidos()
    {
        var produto = new Produto("Brinde", 0m, 0);

        Assert.Equal(0m, produto.Preco);
        Assert.Equal(0, produto.Estoque);
    }

    [Theory]
    [InlineData(10, 1, true)]
    [InlineData(10, 10, true)]
    [InlineData(10, 11, false)]
    [InlineData(10, 0, false)]
    [InlineData(0, 1, false)]
    public void TemEstoque_VerificaDisponibilidade(int estoque, int quantidade, bool esperado)
    {
        var produto = new Produto("Monitor", 800m, estoque);

        Assert.Equal(esperado, produto.TemEstoque(quantidade));
    }

    [Fact]
    public void BaixarEstoque_QuantidadeValida_DecrementaEstoque()
    {
        var produto = new Produto("Monitor", 800m, 10);

        produto.BaixarEstoque(4);

        Assert.Equal(6, produto.Estoque);
    }

    [Fact]
    public void BaixarEstoque_QuantidadeMaiorQueEstoque_LancaEstoqueInsuficienteException()
    {
        var produto = new Produto("Monitor", 800m, 3);

        Assert.Throws<EstoqueInsuficienteException>(() => produto.BaixarEstoque(4));
        Assert.Equal(3, produto.Estoque);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void BaixarEstoque_QuantidadeInvalida_LancaArgumentException(int quantidade)
    {
        var produto = new Produto("Monitor", 800m, 3);

        Assert.Throws<ArgumentException>(() => produto.BaixarEstoque(quantidade));
    }
}