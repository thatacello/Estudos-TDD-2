using System.Linq;
using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Domain.Utils;

namespace Store.Domain.Handlers
{
    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryFeeRepository _deliveryFeeRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository; 
        public OrderHandler(
            ICustomerRepository customerRepository,
            IDeliveryFeeRepository deliveryFeeRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _deliveryFeeRepository = deliveryFeeRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }
        public ICommandResult Handle(CreateOrderCommand command)
        {
            // fail fast validation
            command.Validate();
            if(command.Invalid)
                return new GenericCommandResult(false, "Pedido inválido", null);

            // 1. recupera o cliente
            var customer = _customerRepository.Get(command.Customer);

            // 2. calcula a taxa de entrega
            var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);

            // 3. obtém o cupom de desconto
            var discount = _discountRepository.Get(command.PromoCode);

            // 4. gera o pedido
            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            var order = new Order(customer, deliveryFee, discount);
            foreach(var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.AddItem(product, item.Quantity);
            }

            // 5. agrupa as notificações
            AddNotifications(order.Notifications);

            // 6. verifica se tudo deu certo
            if(Invalid)
                return new GenericCommandResult(false, "Falha ao gerar o pedido", Notifications);
            
            // 7 . retorna o resultado
            _orderRepository.Save(order);
            return new GenericCommandResult(true, $"Pedido {order.Number} gerado com sucesso", order);
        }
    }
}