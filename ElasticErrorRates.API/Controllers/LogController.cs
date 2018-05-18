using System;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
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
                var extractedQuery = _unitOfWork.GetInstance<IGenericRepository<Log>>();

                var extractedResult = await _queryDispatcher.DispatchAsync(extractedQuery.GetAllAsync);

                //Insert into ELASTIC
                var command = _unitOfWork.GetInstance<ILogElasticRepository<Log>>();

                await _commandDispatcher.DispatchAsync(command.Bulk, extractedResult);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("searchaggregate")]
        public async Task<IActionResult> SearchAggregate()
        {
            try
            {
                var elasticQuery = _unitOfWork.GetInstance<ILogElasticRepository<LogSummary>>();

                var result = await _queryDispatcher.DispatchAsync(elasticQuery.SearchAggregate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("search/{page}/{pageSize}")]
        public async Task<IActionResult> Search(int page, int pageSize, string httpUrl)
        {
            try
            {
                var elasticQuery = _unitOfWork.GetInstance<ILogElasticRepository<Log>>();

                var result = await _queryDispatcher.DispatchAsync(elasticQuery.Search, new SearchCriteria{HttpUrl = httpUrl, Page = page, PageSize = pageSize});

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string columnField, string httpUrl, string term)
        {
            try
            {
                switch (columnField)
                {
                    case "exception":
                        var queryLog = _unitOfWork.GetInstance<ILogElasticRepository<Log>>();
                        return Ok(await _queryDispatcher.DispatchAsync(queryLog.Find, new FindCriteria{ColumnField = columnField, HttpUrl = httpUrl, Term = term}));

                    case "httpUrl":
                        var queryLogSummary = _unitOfWork.GetInstance<ILogElasticRepository<LogSummary>>();
                        return Ok(await _queryDispatcher.DispatchAsync(queryLogSummary.Find, new FindCriteria { ColumnField = columnField, HttpUrl = httpUrl, Term = term }));
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
