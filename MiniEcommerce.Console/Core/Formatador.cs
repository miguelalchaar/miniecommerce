using System.Globalization;

namespace MiniEcommerce.Core;

public static class Formatador
{
    // Formato brasileiro definido manualmente para não depender da cultura/ICU da máquina.
    private static readonly NumberFormatInfo FormatoBrasileiro = new()
    {
        CurrencySymbol = "R$",
        CurrencyDecimalSeparator = ",",
        CurrencyGroupSeparator = ".",
        CurrencyPositivePattern = 2, // "$ n"
        CurrencyDecimalDigits = 2
    };

    public static string Moeda(decimal valor) => valor.ToString("C", FormatoBrasileiro);
}