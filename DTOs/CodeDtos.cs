namespace EnterpriseApi.DTOs
{
    public class CodeDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CreateCodeDto
    {
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateCodeDto
    {
        public int? OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
