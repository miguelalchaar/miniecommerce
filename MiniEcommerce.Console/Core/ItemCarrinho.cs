namespace MiniEcommerce.Core;

public class ItemCarrinho
{
    public Produto Produto { get; }
    public int Quantidade { get; internal set; }
    public decimal Subtotal => Produto.Preco * Quantidade;

    internal ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto;
        Quantidade = quantidade;
    }
}