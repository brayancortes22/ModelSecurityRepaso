namespace ModelSecurityRepaso.Entity.Dto.Response
{
    /// <summary>
    /// DTO de respuesta para FormModule - Expone solo datos públicos.
    /// </summary>
    public class FormModuleResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public int FormId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
