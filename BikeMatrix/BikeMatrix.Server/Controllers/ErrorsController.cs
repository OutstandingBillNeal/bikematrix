using Microsoft.AspNetCore.Mvc;

namespace BikeMatrix.Server.Controllers;

public class ErrorsController : Controller
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();
}
