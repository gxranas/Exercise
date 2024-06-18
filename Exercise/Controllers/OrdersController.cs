using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exercise.Data;
using Exercise.Models;

namespace Exercise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(AppDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequest newOrder)
        {
            if (newOrder == null)
            {
                return BadRequest("Order request cannot be null.");
            }

            Order order = new Order
            {
                ShippingInfo = new ShippingInfo
                {
                    Address = newOrder.Shipping_details[0],
                    City = newOrder.Shipping_details[1],
                    PostalCode = newOrder.Shipping_details[2]
                },

                Products = newOrder.Products.Select(p => new Products
                {
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            };

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the order to the database.");
                return CreatedAtAction(nameof(GetOrder), new { id = StatusCodes.Status500InternalServerError }, order);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.Where(o => o.Id == id).Include(o => o.Products).Where(o => o.Products.Any(p => p.OrderID == o.Id)).Include(o => o.ShippingInfo).Where(o => o.ShippingInfo.Id == o.ShippingInfoId).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
