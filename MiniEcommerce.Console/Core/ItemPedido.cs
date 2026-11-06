namespace MiniEcommerce.Core;

/// <summary>
/// Registro imutável de um item no momento da compra (preço e quantidade não mudam depois).
/// </summary>
public record ItemPedido(string NomeProduto, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}