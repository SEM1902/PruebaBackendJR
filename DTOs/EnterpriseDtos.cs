namespace EnterpriseApi.DTOs
{
    public class EnterpriseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? Nit { get; set; }
        public long Gln { get; set; }
    }

    public class CreateEnterpriseDto
    {
        public string Name { get; set; } = string.Empty;
        public long? Nit { get; set; }
        public long Gln { get; set; }
    }

    public class UpdateEnterpriseDto
    {
        public string? Name { get; set; }
        public long? Nit { get; set; }
        public long? Gln { get; set; }
    }

    public class EnterpriseWithCodesDto : EnterpriseDto
    {
        public List<CodeDto> Codes { get; set; } = new List<CodeDto>();
    }
}
