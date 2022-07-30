using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                await orderContext.Orders.AddRangeAsync(GetPreconfigurationOrders());
                await orderContext.SaveChangesAsync();

                logger.LogInformation($"Seed database associated with context {typeof(OrderContext).Name}");
            }
        }

        private static IEnumerable<Order> GetPreconfigurationOrders()
        {
            return new List<Order>
            {
                new Order { UserName="hamedbagheri", FirstName ="Hamed", LastName = "Bagheri", EmailAddress ="h.bagheri@gmail.com", AddressLine ="Vanak Sq", Country ="Iran", TotalPrice =350 }
            };
        }
    }
}
