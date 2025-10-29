namespace ModelSecurityRepaso.Entity.Dto.Response
{
    /// <summary>
    /// DTO de respuesta para Person - Expone solo datos públicos sin relaciones.
    /// </summary>
    public class PersonResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string LastSecondName { get; set; }
        public string TypeDocument { get; set; }
        public int NumberDocument { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
