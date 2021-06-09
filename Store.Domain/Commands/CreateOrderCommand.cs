using System.Collections.Generic;
using Flunt.Notifications;
using Flunt.Validations;

namespace Store.Domain.Commands
{
    public class CreateOrderCommand : Notifiable, ICommand
    {
        public string Customer { get; set; }
        public string ZipCode { get; set; }
        public string PromoCode { get; set; }
        public IList<CreateOrderItemCommand> Items { get; set; }
        public CreateOrderCommand(string customer, string zipCode, string promoCode, IList<CreateOrderItemCommand> items)
        {
            Customer = customer;
            ZipCode = zipCode;
            PromoCode = promoCode;
            Items = items;
        }
        public CreateOrderCommand()
        {
            Items = new List<CreateOrderItemCommand>();
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasLen(Customer, 11, "Customer", "Cliente inválido")
                .HasLen(ZipCode, 8, "ZipCode", "CEP inválido")    
            );
        }
    }
}