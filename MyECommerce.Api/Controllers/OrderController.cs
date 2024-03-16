using MyECommerce.Infrastructure;

namespace MyECommerce.Api;

public class OrderController
{
    private readonly ApplicationContext _context;

    public OrderController(ApplicationContext context)
    {
        _context = context;
    }
    
}