using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using AureliaWithDotNet.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : ControllerBase
	{
		readonly ICountryRepository _countryRepository;
		readonly ILogger<AssetController> _logger;

		public CountryController(ICountryRepository countryRepository,
				 ILogger<AssetController> logger)
		{
			_countryRepository = countryRepository;
			_logger = logger;
		}

		/// <summary>
		/// Get a Countries
		/// </summary>
		/// <response code="200">Returns a list of countries</response>
		/// <response code="400">If the not found</response>  
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ResultServiceDataVM<IEnumerable<Country>>>> Get()
		{
			_logger.LogInformation($"Reading countries");

			return new ResultServiceDataVM<IEnumerable<Country>>(await _countryRepository.GetAll()).GetResultData();
		}
	}
}
