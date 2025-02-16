using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.User;
public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE_KEY = "ErrorMessage";
    private const string ERROR_MESSAGE_VALUE = "Sua senha deve ter no mínimo 8 caracteres, contendo pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial(por exemplo, !, ?, *, .)";
    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE_KEY}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ERROR_MESSAGE_VALUE);
            return false;
        }

        if (value.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ERROR_MESSAGE_VALUE);
            return false;
        }

        if (UpperCaseLetter().IsMatch(value) == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ERROR_MESSAGE_VALUE);
            return false;
        }

        if (LowerCaseLetter().IsMatch(value) == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ERROR_MESSAGE_VALUE);
            return false;
        }

        if (Numbers().IsMatch(value) == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ERROR_MESSAGE_VALUE);
            return false;
        }

        if (SpecialSymbols().IsMatch(value) == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ERROR_MESSAGE_VALUE);
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();

    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();

    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Numbers();

    [GeneratedRegex(@"[\!\?\*\.\#\@\%\&]+")]
    private static partial Regex SpecialSymbols();
}
