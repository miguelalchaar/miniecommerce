using System.Text;

namespace MiniEcommerce.Core;

public class Pedido
{
    public DateTime DataCriacao { get; }
    public IReadOnlyList<ItemPedido> Itens { get; }
    public decimal Total => Itens.Sum(i => i.Subtotal);

    private Pedido(IReadOnlyList<ItemPedido> itens)
    {
        Itens = itens;
        DataCriacao = DateTime.Now;
    }

    /// <summary>
    /// Fecha a compra: valida o estoque de todos os itens, dá baixa no estoque,
    /// gera o pedido e esvazia o carrinho. Se algum item não tiver estoque, nada é alterado.
    /// </summary>
    public static Pedido Finalizar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        if (carrinho.Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido com o carrinho vazio.");

        // Valida tudo antes de baixar qualquer estoque (evita baixa parcial).
        foreach (var item in carrinho.Itens)
        {
            if (!item.Produto.TemEstoque(item.Quantidade))
                throw EstoqueInsuficienteException.Para(item.Produto.Nome, item.Quantidade, item.Produto.Estoque);
        }

        var itensPedido = new List<ItemPedido>();
        foreach (var item in carrinho.Itens)
        {
            item.Produto.BaixarEstoque(item.Quantidade);
            itensPedido.Add(new ItemPedido(item.Produto.Nome, item.Produto.Preco, item.Quantidade));
        }

        carrinho.Limpar();
        return new Pedido(itensPedido.AsReadOnly());
    }

    public string GerarResumo()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"=== Resumo do Pedido ({DataCriacao:dd/MM/yyyy HH:mm}) ===");

        foreach (var item in Itens)
            sb.AppendLine($"{item.Quantidade}x {item.NomeProduto} - {Formatador.Moeda(item.PrecoUnitario)} = {Formatador.Moeda(item.Subtotal)}");

        sb.AppendLine("--------------------------------");
        sb.AppendLine($"Total: {Formatador.Moeda(Total)}");
        return sb.ToString();
    }
}