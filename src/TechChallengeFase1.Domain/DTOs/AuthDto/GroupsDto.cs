namespace TechChallengeFase1.Domain.DTOs.AuthDto
{
    public class GroupsDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public List<GroupsDto> SubGroups { get; set; } = new();
    }
}
