namespace ModelSecurityRepaso.Entity.Dto.Response
{
    /// <summary>
    /// DTO de respuesta para Form - Expone solo datos públicos.
    /// </summary>
    public class FormResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
