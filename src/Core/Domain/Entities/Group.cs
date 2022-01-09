namespace DiscoGroupie.Core.Domain.Entities;

public class Group
{
    public ulong GroupId { get; set; }
    public ulong GuildId { get; set; }
    public ulong GroupOwner { get; set; }
    public string GroupName { get; set; }

    public Group(ulong groupId, ulong guildId, ulong groupOwner, string groupName)
    {
        GroupId = groupId;
        GuildId = guildId;
        GroupOwner = groupOwner;
        GroupName = groupName;
    }
}