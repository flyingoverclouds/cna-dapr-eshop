using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using Dapr;
using Basket.Interfaces;
using orderprocessor.svc.Dapr;

namespace orderprocessor.svc.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    //[Topic("orderpubsub", "neworder")]  // <== Use this attribut in case you dont use a component subscription yaml file.
    [HttpPost("neworder")]
    public IActionResult OrderReceived([FromBody] CloudEvent<Order> neworderevent)
    {
        if (neworderevent.Data==null)
        {
            Console.WriteLine("Order event received : " + neworderevent.ToString());
            Console.WriteLine("ERROR : No data payload");
            return new UnprocessableEntityResult();
        }

        Console.WriteLine($"PROCESSING ORDER {neworderevent.Data.Id} : {neworderevent.Data.Lines.Count} order line(s) ");
        return new OkResult();
    }
}
