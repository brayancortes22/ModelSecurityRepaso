using ModelSecurityRepaso.Entity.Model.Base;

namespace ModelSecurityRepaso.Entity.Model
{
    public class Person : BaseModel
    {
        public string FirstName { get; set; }
        // segundo nombre opcional
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        // segundo apellido opcional
        public string? LastSecondName { get; set; }
        public string TypeDocument { get; set; }
        public int NumberDocument { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // Relación uno a uno con User
        public User User { get; set; }
    }
}
