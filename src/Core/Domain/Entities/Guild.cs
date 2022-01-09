namespace DiscoGroupie.Core.Domain.Entities;

public class Guild
{
    public ulong GuildId { get; private set; }
    public ulong GroupTextSectionId { get; private set; }
    public ulong GroupVoiceSectionId { get; private set; }
    public string Prefix { get; private set; }
    public string GroupNamePrefix { get; private set; }

    public Guild(ulong guildId,
        string prefix = "!dg ",
        string groupNamePrefix = "Group_",
        ulong groupTextSectionId = default,
        ulong groupVoiceSectionId = default)
    {
        GuildId = guildId;
        GroupTextSectionId = groupTextSectionId;
        GroupVoiceSectionId = groupVoiceSectionId;
        Prefix = prefix;
        GroupNamePrefix = groupNamePrefix;
    }

    public void UpdateGuild(ulong groupTextSectionId,
        ulong groupVoiceSectionId,
        string prefix,
        string groupNamePrefix)
    {
        GroupTextSectionId = groupTextSectionId;
        GroupVoiceSectionId = groupVoiceSectionId;
        Prefix = prefix;
        GroupNamePrefix = groupNamePrefix;
    }
    
    
}