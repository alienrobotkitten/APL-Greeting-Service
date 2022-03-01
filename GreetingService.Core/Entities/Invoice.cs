using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace GreetingService.Core.Entities;
public class Invoice
{
    public int Id { get; set; }
    public User User { get; set; }
    public List<Greeting> Greetings { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public float PricePerGreeting { get; set; }
    public float SumTotal { get; set; }
    public string Currency { get; set; }

    public Invoice()
    {

    }
}
