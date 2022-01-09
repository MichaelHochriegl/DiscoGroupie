// ReSharper disable once CheckNamespace

using Discord;

namespace Ardalis.Result;

public static class DiscoGroupieResultExtensions
{
    public static Embed ToEmbed(this IResult result, string commandName)
    {
        var builder = new EmbedBuilder();
        builder.WithAuthor("DiscoGroupie")
            .WithFooter($"Response for command '{commandName}'");

        switch (result.Status)
        {
            case ResultStatus.Ok:
                builder.WithColor(Color.Green);
                break;
            case ResultStatus.Error:
                builder.WithColor(Color.Red);
                break;
            case ResultStatus.Forbidden:
                builder.WithColor(Color.Orange);
                break;
            case ResultStatus.Unauthorized:
                builder.WithColor(Color.Orange);
                break;
            case ResultStatus.Invalid:
                builder.WithColor(Color.Orange);
                break;
            case ResultStatus.NotFound:
                builder.WithColor(Color.Blue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        

        foreach (var error in result.Errors)
        {
            builder.AddField("Errormessage", error);
        }

        foreach (var validationError in result.ValidationErrors)
        {
            builder.AddField(validationError.Identifier, validationError.ErrorMessage);
        }

        return builder.Build();
    }
}