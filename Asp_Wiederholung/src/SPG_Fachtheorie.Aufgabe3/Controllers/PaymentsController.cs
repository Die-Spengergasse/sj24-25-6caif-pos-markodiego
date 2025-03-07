using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private static readonly List<PaymentDetailDto> Payments = new()
    {
        new (1, "John", "Doe", 5, "Credit Card", new DateTime(2024, 03, 01), new List<PaymentItemDto>
        {
            new ("Laptop", 1, 1200.00m),
            new ("Mouse", 2, 50.00m)
        }),
        
        new (2, "Jane", "Smith", 3, "Cash", new DateTime(2024, 03, 02), new List<PaymentItemDto>
        {
            new ("Keyboard", 1, 100.00m),
            new ("Monitor", 1, 300.00m)
        }),

        new (3, "Mike", "Johnson", 5, "Credit Card", new DateTime(2024, 03, 01), new List<PaymentItemDto>
        {
            new ("Headphones", 1, 150.00m)
        })
    };

    [HttpGet]
    public ActionResult<IEnumerable<PaymentDetailDto>> GetPayments([FromQuery] int? cashDesk, [FromQuery] DateTime? dateFrom)
    {
        var filteredPayments = Payments.AsQueryable();

        if (cashDesk.HasValue)
        {
            filteredPayments = filteredPayments.Where(p => p.CashDeskNumber == cashDesk.Value);
        }

        if (dateFrom.HasValue)
        {
            filteredPayments = filteredPayments.Where(p => p.Date.Date == dateFrom.Value.Date);
        }

        return Ok(filteredPayments.ToList());
    }
}
