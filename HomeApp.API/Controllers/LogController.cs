﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ElasticSearch.Core.CQRS.Command;
using ElasticSearch.Core.CQRS.Query;
using ElasticSearch.Core.Manager;
using ElasticSearch.Core.Models;
using ElasticSearch.Core.Persistence;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearch.API.Controllers
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

        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                var elasticQuery = _unitOfWork.GetInstance<ILogElasticRepository<Log>>();

                var result = await _queryDispatcher.DispatchAsync(elasticQuery.Search);

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
