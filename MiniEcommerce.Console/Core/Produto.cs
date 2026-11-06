namespace MiniEcommerce.Core;

public class Produto
{
    public int Id { get; init; }
    public string Nome { get; }
    public decimal Preco { get; }
    public int Estoque { get; private set; }

    public Produto(string nome, decimal preco, int estoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(nome));

        if (preco < 0)
            throw new ArgumentException("O preço do produto não pode ser negativo.", nameof(preco));

        if (estoque < 0)
            throw new ArgumentException("O estoque do produto não pode ser menor que zero.", nameof(estoque));

        Nome = nome.Trim();
        Preco = preco;
        Estoque = estoque;
    }

    public bool TemEstoque(int quantidade) => quantidade > 0 && Estoque >= quantidade;

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        if (quantidade > Estoque)
            throw EstoqueInsuficienteException.Para(Nome, quantidade, Estoque);

        Estoque -= quantidade;
    }
}