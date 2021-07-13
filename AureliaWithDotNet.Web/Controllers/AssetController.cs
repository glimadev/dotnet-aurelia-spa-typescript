using AureliaWithDotNet.Domain.Domains.Assets;
using AureliaWithDotNet.Domain.Extensions;
using AureliaWithDotNet.Domain.Interfaces;
using AureliaWithDotNet.Domain.Models;
using AureliaWithDotNet.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AureliaWithDotNet.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        readonly IAssetRepository _assetRepository;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly ILogger<AssetController> _logger;

        public AssetController(IAssetRepository assetRepository,
             IHttpContextAccessor httpContextAccessor,
             ILogger<AssetController> logger)
        {
            _assetRepository = assetRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Get a Asset
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns a asset</response>
        /// <response code="400">If the asset is invalid</response>  
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultServiceDataVM<Asset>>> Get(int id)
        {
            _logger.LogInformation($"Reading asset for {id}");

            return new ResultServiceDataVM<Asset>(await _assetRepository.GetById(id)).GetResultData();
        }

        /// <summary>
        /// Creates a Asset.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /asset
        ///     {
        ///       "assetName": "Tests",
        ///       "department": 1,
        ///       "countryOfDepartment": "Brasil",
        ///       "eMailAdressOfDepartment": "teste@teste.com.br",
        ///       "purchaseDate": "2021-06-24T18:46:32.552Z",
        ///       "broken": true
        ///     }
        ///
        /// </remarks>
        /// <param name="asset"></param>
        /// <param name="_countryRepository"></param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the asset is invalid</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultServiceVM>> Post([FromBody] Asset asset,
            [FromServices] ICountryRepository _countryRepository)
        {
            _logger.LogInformation($"Trying to create a asset - {JsonSerializer.Serialize(asset)}");

            var result = new ResultServiceVM(await new AssetValidation(_countryRepository).ValidateAsync(asset));

            if (!result.Success)
            {
                _logger.LogWarning($"Data sent was invalid - {result.GetMessages()}");

                return result.GetResult();
            }

            _assetRepository.Add(asset);

            _logger.LogInformation($"Asset was created - {asset.Id}");

            return new CreatedResult(new Uri($"{_httpContextAccessor.GetURL()}/api/asset/{asset.Id}"), null);
        }

        /// <summary>
        /// Update a Asset.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /asset
        ///     {
        ///       "assetName": "Tests",
        ///       "department": 1,
        ///       "countryOfDepartment": "Brasil",
        ///       "eMailAdressOfDepartment": "teste@teste.com.br",
        ///       "purchaseDate": "2021-06-24T18:46:32.552Z",
        ///       "broken": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        /// <param name="_countryRepository"></param>
        /// <response code="200">Returns void</response>
        /// <response code="400">If the asset is invalid</response>  
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultServiceVM>> Put(int id, [FromBody] Asset asset,
            [FromServices] ICountryRepository _countryRepository)
        {
            _logger.LogInformation($"Updating value for {id} - fields {JsonSerializer.Serialize(asset)}");

            var result = new ResultServiceVM(await new AssetValidation(_countryRepository).ValidateAsync(asset));

            if (!result.Success)
            {
                _logger.LogWarning($"Data sent was invalid - {result.GetMessages()}");

                return result.GetResult();
            }

            var assetDB = await _assetRepository.Find(id);

            if (assetDB == null)
            {
                _logger.LogWarning("Asset wasnt found - {id}", id);

                return new NotFoundResult();
            }

            assetDB.AssetName = asset.AssetName;
            assetDB.Broken = asset.Broken;
            assetDB.CountryOfDepartment = asset.CountryOfDepartment;
            assetDB.Department = asset.Department;
            assetDB.EMailAdressOfDepartment = asset.EMailAdressOfDepartment;
            assetDB.PurchaseDate = asset.PurchaseDate;

            _assetRepository.Update(assetDB);

            _logger.LogInformation("Updated {id}", id);

            return result.GetResult();
        }

        /// <summary>
        /// Remove a Asset.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns void</response>
        /// <response code="400">If the asset is invalid</response>  
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting value for {id}", id);

            var asset = await _assetRepository.Find(id);

            if (asset == null)
            {
                _logger.LogWarning("Asset wasnt found - {id}", id);

                return new NotFoundResult();
            }

            _assetRepository.Delete(asset);

            _logger.LogInformation("Deleted {id}", id);

            return new NoContentResult();
        }
    }
}
