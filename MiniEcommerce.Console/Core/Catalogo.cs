namespace MiniEcommerce.Core;

/// <summary>
/// Gerencia o catálogo de produtos e o seu estoque.
/// </summary>
public class Catalogo
{
    private readonly List<Produto> _produtos = new();
    private int _proximoId = 1;

    public Produto Cadastrar(string nome, decimal preco, int estoque)
    {
        var produto = new Produto(nome, preco, estoque) { Id = _proximoId };

        if (_produtos.Any(p => string.Equals(p.Nome, produto.Nome, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Já existe um produto cadastrado com o nome '{produto.Nome}'.");

        _produtos.Add(produto);
        _proximoId++;
        return produto;
    }

    public Produto? BuscarPorId(int id) => _produtos.FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<Produto> Listar() => _produtos.AsReadOnly();

    public IReadOnlyList<Produto> ListarDisponiveis() =>
        _produtos.Where(p => p.Estoque > 0).ToList().AsReadOnly();
}