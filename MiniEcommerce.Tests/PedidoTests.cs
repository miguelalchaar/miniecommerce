using MiniEcommerce.Core;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    private static (CarrinhoDeCompras carrinho, Produto mouse, Produto teclado) CriarCenario()
    {
        var mouse = new Produto("Mouse", 50m, 10) { Id = 1 };
        var teclado = new Produto("Teclado", 120m, 5) { Id = 2 };
        var carrinho = new CarrinhoDeCompras();
        carrinho.Adicionar(mouse, 2);    // 100
        carrinho.Adicionar(teclado, 1);  // 120
        return (carrinho, mouse, teclado);
    }

    [Fact]
    public void Finalizar_CarrinhoValido_DecrementaEstoqueDosProdutos()
    {
        var (carrinho, mouse, teclado) = CriarCenario();

        Pedido.Finalizar(carrinho);

        Assert.Equal(8, mouse.Estoque);
        Assert.Equal(4, teclado.Estoque);
    }

    [Fact]
    public void Finalizar_CarrinhoValido_CalculaTotalDoPedido()
    {
        var (carrinho, _, _) = CriarCenario();

        var pedido = Pedido.Finalizar(carrinho);

        Assert.Equal(220m, pedido.Total);
        Assert.Equal(2, pedido.Itens.Count);
    }

    [Fact]
    public void Finalizar_CarrinhoValido_EsvaziaOCarrinho()
    {
        var (carrinho, _, _) = CriarCenario();

        Pedido.Finalizar(carrinho);

        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Finalizar_ItensDoPedido_GuardamPrecoEQuantidadeDaCompra()
    {
        var (carrinho, _, _) = CriarCenario();

        var pedido = Pedido.Finalizar(carrinho);

        var itemMouse = Assert.Single(pedido.Itens, i => i.NomeProduto == "Mouse");
        Assert.Equal(50m, itemMouse.PrecoUnitario);
        Assert.Equal(2, itemMouse.Quantidade);
        Assert.Equal(100m, itemMouse.Subtotal);
    }

    [Fact]
    public void Finalizar_CarrinhoVazio_LancaInvalidOperationException()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => Pedido.Finalizar(carrinho));
    }

    [Fact]
    public void Finalizar_CarrinhoNulo_LancaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => Pedido.Finalizar(null!));
    }

    [Fact]
    public void Finalizar_EstoqueAlteradoAposAdicionarNoCarrinho_LancaExcecaoESemBaixaParcial()
    {
        var (carrinho, mouse, teclado) = CriarCenario();
        teclado.BaixarEstoque(5); // outro cliente comprou todo o estoque do teclado

        Assert.Throws<EstoqueInsuficienteException>(() => Pedido.Finalizar(carrinho));

        Assert.Equal(10, mouse.Estoque);   // nenhuma baixa parcial
        Assert.Equal(2, carrinho.Itens.Count); // carrinho preservado
    }

    [Fact]
    public void GerarResumo_ContemItensQuantidadesETotal()
    {
        var (carrinho, _, _) = CriarCenario();
        var pedido = Pedido.Finalizar(carrinho);

        var resumo = pedido.GerarResumo();

        Assert.Contains("Mouse", resumo);
        Assert.Contains("Teclado", resumo);
        Assert.Contains("2x", resumo);
        Assert.Contains(Formatador.Moeda(220m), resumo);
    }

    [Theory]
    [InlineData(75, "R$ 75,00")]
    [InlineData(0, "R$ 0,00")]
    [InlineData(1234.5, "R$ 1.234,50")]
    public void Moeda_FormataNoPadraoBrasileiro(double valor, string esperado)
    {
        Assert.Equal(esperado, Formatador.Moeda((decimal)valor));
    }
}