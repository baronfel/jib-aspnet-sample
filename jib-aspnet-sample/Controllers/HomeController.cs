using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using jib_example.Models;

namespace jib_example.Controllers;

///<summary>A person is a wonderful, unique life form.</summary>
public record Person(string Name, int Age);

///<summary>nothing's as good as</summary>
[Route("home")]
public class HomeController : Controller
{
    private Person chet = new ("Chet", 34);

    private readonly ILogger<HomeController> _logger;

    ///<summary>everybody's got needs</summary>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    ///<summary>here we are again, my friend</summary>
    [HttpGet]
    public IActionResult Index() => View();

    ///<summary>Gets your favorite pal</summary>
    [HttpGet]
    public Person Chet() => chet;

    ///<summary>Gets your favorite pal at a different age</summary>
    [HttpGet]
    public Person AgeUp([FromQuery] int years) => chet with {Age = chet.Age + years };
}
