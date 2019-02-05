using System;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticErrorRates.API.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public LogController(IUnitOfWork unitOfWork, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
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
                var extractedResult = await _queryDispatcher.DispatchAsync(_unitOfWork.GenericRepository<Log>().GetAllAsync);

                //Insert into ELASTIC
                await _commandDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<Log>().Bulk, extractedResult);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("searchlogsaggregate")]
        public async Task<IActionResult> SearchLogsAggregate(DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate = startdate ?? DateTime.MinValue;
                enddate = enddate ?? DateTime.MinValue;

                var result = await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<LogSummary>().SearchLogsAggregate,
                    new SearchAgreggateCriteria()
                    {
                        StartDateTimeLogged = startdate.Value,
                        EndDateTimeLogged = enddate.Value
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("searchlogsdetailed/{page}/{pageSize}")]
        public async Task<IActionResult> SearchLogsDetailed(int page, int pageSize, string httpUrl, string term, DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate = startdate ?? DateTime.MinValue;
                enddate = enddate ?? DateTime.MinValue;

                var result = await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<Log>().SearchLogsDetailed, 
                    new LogSearchCriteria
                    {
                        HttpUrl = httpUrl, 
                        Page = page, 
                        PageSize = pageSize,
                        StartDateTimeLogged = startdate.Value,
                        EndDateTimeLogged = enddate.Value
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string columnField, string httpUrl, string term, DateTime? startdate = null, DateTime? enddate = null)
        {
            try
            {
                startdate = startdate ?? DateTime.MinValue;
                enddate = enddate ?? DateTime.MinValue;

                var criteria = new LogSearchCriteria()
                {
                    ColumnField = columnField, 
                    HttpUrl = httpUrl, 
                    Term = term,
                    StartDateTimeLogged = startdate.Value,
                    EndDateTimeLogged = enddate.Value
                };

                switch (columnField)
                {
                    case "exception":
                        return Ok(await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<Log>().Find, criteria));

                    case "httpUrl":
                        return Ok(await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<LogSummary>().Find, criteria));
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
