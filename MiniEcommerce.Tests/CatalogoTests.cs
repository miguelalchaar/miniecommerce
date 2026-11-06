using MiniEcommerce.Core;

namespace MiniEcommerce.Tests;

public class CatalogoTests
{
    [Fact]
    public void Cadastrar_DadosValidos_AdicionaProdutoAoCatalogo()
    {
        var catalogo = new Catalogo();

        var produto = catalogo.Cadastrar("Notebook", 3500m, 5);

        Assert.Single(catalogo.Listar());
        Assert.Equal("Notebook", produto.Nome);
    }

    [Fact]
    public void Cadastrar_GeraIdsSequenciais()
    {
        var catalogo = new Catalogo();

        var primeiro = catalogo.Cadastrar("Notebook", 3500m, 5);
        var segundo = catalogo.Cadastrar("Mouse", 80m, 20);

        Assert.Equal(1, primeiro.Id);
        Assert.Equal(2, segundo.Id);
    }

    [Fact]
    public void Cadastrar_PrecoNegativo_LancaExcecaoENaoCadastra()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar("Notebook", -1m, 5));
        Assert.Empty(catalogo.Listar());
    }

    [Fact]
    public void Cadastrar_EstoqueNegativo_LancaExcecaoENaoCadastra()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.Cadastrar("Notebook", 3500m, -1));
        Assert.Empty(catalogo.Listar());
    }

    [Theory]
    [InlineData("Notebook")]
    [InlineData("notebook")]
    [InlineData("  NOTEBOOK ")]
    public void Cadastrar_NomeDuplicado_LancaInvalidOperationException(string nomeDuplicado)
    {
        var catalogo = new Catalogo();
        catalogo.Cadastrar("Notebook", 3500m, 5);

        Assert.Throws<InvalidOperationException>(() => catalogo.Cadastrar(nomeDuplicado, 100m, 1));
    }

    [Fact]
    public void BuscarPorId_ProdutoExistente_RetornaProduto()
    {
        var catalogo = new Catalogo();
        var cadastrado = catalogo.Cadastrar("Mouse", 80m, 20);

        var encontrado = catalogo.BuscarPorId(cadastrado.Id);

        Assert.Same(cadastrado, encontrado);
    }

    [Fact]
    public void BuscarPorId_ProdutoInexistente_RetornaNull()
    {
        var catalogo = new Catalogo();

        Assert.Null(catalogo.BuscarPorId(99));
    }

    [Fact]
    public void Listar_RetornaTodosOsProdutosComPrecoEEstoque()
    {
        var catalogo = new Catalogo();
        catalogo.Cadastrar("Notebook", 3500m, 5);
        catalogo.Cadastrar("Mouse", 80m, 20);

        var lista = catalogo.Listar();

        Assert.Equal(2, lista.Count);
        Assert.Contains(lista, p => p.Nome == "Mouse" && p.Preco == 80m && p.Estoque == 20);
    }

    [Fact]
    public void ListarDisponiveis_NaoRetornaProdutosSemEstoque()
    {
        var catalogo = new Catalogo();
        catalogo.Cadastrar("Notebook", 3500m, 0);
        catalogo.Cadastrar("Mouse", 80m, 20);

        var disponiveis = catalogo.ListarDisponiveis();

        Assert.Single(disponiveis);
        Assert.Equal("Mouse", disponiveis[0].Nome);
    }
}