namespace BillingRadar.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool Status { get; private set; }

        // Constructor para EF Core y carga desde base de datos
        public User(int id, string name, string surname, string email, string passwordHash)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            PasswordHash = passwordHash;
            Status = true; // Por defecto activo
        }

        // Constructor para creación de nuevas entidades de dominio
        public User(string name, string surname, string email, string passwordHash)
            : this(0, name, surname, email, passwordHash)
        {
        }

        public bool VerificarPassword(string password)
            => BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}
