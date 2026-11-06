namespace MiniEcommerce.Core;

public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = new();

    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();

    public decimal Total => _itens.Sum(i => i.Subtotal);

    public void Adicionar(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        var itemExistente = _itens.FirstOrDefault(i => ReferenceEquals(i.Produto, produto));
        var quantidadeTotal = (itemExistente?.Quantidade ?? 0) + quantidade;

        if (!produto.TemEstoque(quantidadeTotal))
            throw EstoqueInsuficienteException.Para(produto.Nome, quantidadeTotal, produto.Estoque);

        if (itemExistente is null)
            _itens.Add(new ItemCarrinho(produto, quantidade));
        else
            itemExistente.Quantidade = quantidadeTotal;
    }

    public bool Remover(Produto produto)
    {
        ArgumentNullException.ThrowIfNull(produto);
        return _itens.RemoveAll(i => ReferenceEquals(i.Produto, produto)) > 0;
    }

    public void Limpar() => _itens.Clear();
}