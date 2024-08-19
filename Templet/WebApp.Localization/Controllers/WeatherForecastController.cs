using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace WebApp.Localization.Controllers;

[ApiController]
[Route("ass")]
public class WeatherForecastController(IStringLocalizer<WeatherForecastController> localizer) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var currentCulture = CultureInfo.CurrentUICulture.Name;
        var localizedString = localizer["UserIdIsRequired"];


        var moduleName = localizer["DeviceIdIsRequired"].ToString();
        // ... 其他邏輯 ...
        return Ok(new { ModuleName = moduleName });
    }
}
