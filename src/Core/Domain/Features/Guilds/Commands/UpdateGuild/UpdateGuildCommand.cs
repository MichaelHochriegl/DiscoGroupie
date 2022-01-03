using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using DiscoGroupie.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace DiscoGroupie.Core.Domain.Features.Guilds.Commands.UpdateGuild;

public class UpdateGuildCommand : IRequest<Result<Guild>>
{
    public ulong GuildId { get; set; }
    public ulong GroupTextSectionId { get; set; }
    public ulong GroupVoiceSectionId { get; set; }
    public string Prefix { get; set; }
    public string GroupNamePrefix { get; set; }
}

public class UpdateGuildCommandHandler : IRequestHandler<UpdateGuildCommand, Result<Guild>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public UpdateGuildCommandHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guild>> Handle(UpdateGuildCommand request, CancellationToken cancellationToken)
    {
        var guild = await _dbContext.Guilds.FindAsync(request.GuildId);
        
        if (guild is null) return Result<Guild>.NotFound();
        
        guild.UpdateGuild(request.GroupTextSectionId,
            request.GroupVoiceSectionId,
            request.Prefix,
            request.GroupNamePrefix);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Guild>.Success(guild);
    }
}

public class UpdateGuildCommandValidator : AbstractValidator<UpdateGuildCommand>
{
    public UpdateGuildCommandValidator()
    {
        RuleFor(p => p.GuildId)
            .NotEmpty();
        RuleFor(p => p.GroupTextSectionId)
            .NotEmpty();
        RuleFor(p => p.GroupVoiceSectionId)
            .NotEmpty();
        RuleFor(p => p.Prefix)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(10);
        RuleFor(p => p.GroupNamePrefix)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(100);
    }
}