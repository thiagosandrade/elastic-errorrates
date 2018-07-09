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

        [HttpGet("searchaggregate")]
        public async Task<IActionResult> SearchAggregate()
        {
            try
            {
                var result = await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<LogSummary>().SearchAggregate);

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
                var result = await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<Log>().Search, new SearchCriteria{HttpUrl = httpUrl, Page = page, PageSize = pageSize});

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
                        var queryLog = _unitOfWork.LogElasticRepository<Log>();
                        return Ok(await _queryDispatcher.DispatchAsync(queryLog.Find, new FindCriteria{ColumnField = columnField, HttpUrl = httpUrl, Term = term}));

                    case "httpUrl":
                        return Ok(await _queryDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<LogSummary>().Find, new FindCriteria { ColumnField = columnField, HttpUrl = httpUrl, Term = term }));
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
