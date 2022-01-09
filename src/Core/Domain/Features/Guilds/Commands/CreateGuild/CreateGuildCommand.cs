using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using DiscoGroupie.Core.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscoGroupie.Core.Domain.Features.Guilds.Commands.CreateGuild;

public class CreateGuildCommand : IRequest<Result<Guild>>
{
    public ulong GuildId { get; set; }
    public ulong GroupTextSectionId { get; set; }
    public ulong GroupVoiceSectionId { get; set; }
    public string Prefix { get; set; }
    public string GroupNamePrefix { get; set; }
}

public class CreateGuildCommandHandler : IRequestHandler<CreateGuildCommand, Result<Guild>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public CreateGuildCommandHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guild>> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
    {
        var isGuildPresent = await _dbContext.Guilds.AnyAsync(x => x.GuildId == request.GuildId,
            cancellationToken: cancellationToken);
        
        if (isGuildPresent) return Result<Guild>.Error("A Guild with this ID is already set up");

        var guild = new Guild(request.GuildId,
            request.Prefix,
            request.GroupNamePrefix,
            request.GroupTextSectionId,
            request.GroupVoiceSectionId);

        _dbContext.Guilds.Add(guild);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<Guild>.Success(guild);
    }
}

public class CreateGuildCommandValidator : AbstractValidator<CreateGuildCommand>
{
    public CreateGuildCommandValidator()
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