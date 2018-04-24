using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Microsoft.AspNetCore.Mvc;
using Nest;

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

        [HttpGet("searchaggregate/{page}/{pageSize}")]
        public async Task<IActionResult> SearchAggregate(int page, int pageSize)
        {
            try
            {
                var elasticQuery = _unitOfWork.GetInstance<ILogElasticRepository<Log>>();

                //TODO: Use CQRS
                //var result = await _queryDispatcher.DispatchAsync(elasticQuery.SearchAggregate);
                var result = await elasticQuery.SearchAggregate(page, pageSize);

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

                //TODO: Use CQRS
                //var result = await _queryDispatcher.DispatchAsync(elasticQuery.Search, httpUrl);
                var result = await elasticQuery.Search(page, pageSize, httpUrl);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("find/{term}/{sort}/{match}")]
        public async Task<IActionResult> Find(string term, bool sort, bool match)
        {
            try
            {
                //With CQRS
                var query = _unitOfWork.GetInstance<IElasticRepository<Log>>();

                var result = await query.Find(term, sort, match);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
