namespace MonkeyButler.Business.Models.CharacterSearch
{
    public class Character
    {
        public string? AvatarUrl { get; set; }
        public ClassJob? CurrentClassJob { get; set; }
        public string? FreeCompany { get; set; }
        public string? Name { get; set; }
        public long Id { get; set; }
        public string? Race { get; set; }
        public string? Server { get; set; }
        public string? Tribe { get; set; }
    }
}