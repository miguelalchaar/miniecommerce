namespace MiniEcommerce.Core;

/// <summary>
/// Lançada quando se tenta usar uma quantidade maior do que a disponível em estoque.
/// </summary>
public class EstoqueInsuficienteException : InvalidOperationException
{
    public EstoqueInsuficienteException(string mensagem) : base(mensagem)
    {
    }

    public static EstoqueInsuficienteException Para(string nomeProduto, int solicitado, int disponivel) =>
        new($"Estoque insuficiente para '{nomeProduto}': solicitado {solicitado}, disponível {disponivel}.");
}