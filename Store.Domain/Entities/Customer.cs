namespace Store.Domain.Entities
{
    public class Customer
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}