using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Enums;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticErrorRates.API.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public DashboardController(IUnitOfWork unitOfWork, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _unitOfWork = unitOfWork;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;

        }

        [HttpPost("import")]
        public async Task<IActionResult> Import()
        {
            try
            {
                //Extract from SQL
                var extractedResult = await _queryDispatcher.DispatchAsync(_unitOfWork.GenericRepository<DailyRate>().GetAllAsync);

                //Insert into ELASTIC
                await _commandDispatcher.DispatchAsync(_unitOfWork.DashboardElasticRepository<DailyRate>().Bulk, extractedResult);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("getlogsquantity")]
        public async Task<IActionResult> GetLogsQuantity(DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate ??= DateTime.MinValue;
                enddate ??= DateTime.MinValue;

                string cacheKey = $"{startdate} - {enddate}";

                var result = await _queryDispatcher.DispatchAsync(
                    _unitOfWork.LogElasticRepository<Log>().GetLogsQuantity, new LogQuantityCriteria(){
                        StartDateTimeLogged = startdate.Value,
                        EndDateTimeLogged = enddate.Value
                    }, 
                    cacheKey);


                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("search/{countryId}")]
        public async Task<IActionResult> Search(int countryId, DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate = startdate ?? DateTime.MinValue;
                enddate = enddate ?? DateTime.MinValue;

                string cacheKey = $"{startdate} - {enddate} - {countryId}";

                var result = await _queryDispatcher.DispatchAsync(
                    _unitOfWork.DashboardElasticRepository<DailyRate>().Search,
                    new DashboardSearchCriteria()
                    {
                        CountryId = (Country)countryId,
                        StartDateTimeLogged = startdate.Value,
                        EndDateTimeLogged = enddate.Value
                    }, 
                    cacheKey);
                
                
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("searchaggregate")]
        public async Task<IActionResult> SearchAggregate(int countryId, string typeAggregation, string numberOfResults, DateTime? enddate = null)
        {
            try
            {
                int.TryParse(numberOfResults, out var numberOfResult);
                enddate = enddate ?? DateTime.MinValue;

                string cacheKey = $"{enddate} - {countryId}";


                var result = await _queryDispatcher.DispatchAsync(
                    _unitOfWork.DashboardElasticRepository<ErrorRate>().SearchAggregate,
                    new GraphCriteria
                    {
                        CountryId = (Country)countryId,
                        TypeAggregation = typeAggregation,
                        NumberOfResults = numberOfResult,
                        EndDateTimeLogged = enddate.Value
                    },
                    cacheKey);


                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("errorsrank/{countryId}")]
        public async Task<IActionResult> ErrorsRank(int countryId, DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate = startdate ?? DateTime.MinValue;
                enddate = enddate ?? DateTime.MinValue;

                string cacheKey = $"{startdate} - {enddate} - {countryId} - rank";

                var result = await _queryDispatcher.DispatchAsync(
                    _unitOfWork.LogElasticRepository<LogSummary>().SearchLogsAggregateByCountryId, 
                    new GraphCriteria()
                    {
                        CountryId = (Country)countryId,
                        StartDateTimeLogged = startdate.Value,
                        EndDateTimeLogged = enddate.Value
                    },
                    cacheKey);


                result.Records = result.Records.Take(5);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
