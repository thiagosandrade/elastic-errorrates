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
    public class ElasticController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public ElasticController(IUnitOfWork unitOfWork, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _unitOfWork = unitOfWork;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            
        }
        // GET api/values
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                //With CQRS
                var query = _unitOfWork.GetInstance<IElasticRepository<Shakespeare>>();

                var result = await _queryDispatcher.DispatchAsync(query.Search);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // GET api/values/5
        [HttpGet("find/{term}/{sort}/{match}")]
        public async Task<IActionResult> Find(string term, bool sort, bool match)
        {
            try
            {
                //With CQRS
                var query = _unitOfWork.GetInstance<IElasticRepository<Shakespeare>>();

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
