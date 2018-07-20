using System;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Criteria.Dashboard;
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

        [HttpGet("search/{countryId}")]
        public async Task<IActionResult> Search(int countryId, DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate = startdate ?? DateTime.MinValue;
                enddate = enddate ?? DateTime.MinValue;

                var result = await _queryDispatcher.DispatchAsync(
                    _unitOfWork.DashboardElasticRepository<DailyRate>().Search,
                    new SearchCriteria {CountryId = countryId, StartDate = startdate.Value, EndDate = enddate.Value});
                
                
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("searchaggregate")]
        public async Task<IActionResult> SearchAggregate(string typeAggregation, string numberOfResults)
        {
            try
            {

                var result = await _queryDispatcher.DispatchAsync(
                    _unitOfWork.DashboardElasticRepository<ErrorRate>().SearchAggregate, 
                    new GraphCriteria { TypeAggregation = typeAggregation, NumberOfResults = Int32.Parse(numberOfResults)});
                
                
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
