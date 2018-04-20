using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace ElasticErrorRates.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public ValuesController(IUnitOfWork unitOfWork, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _unitOfWork = unitOfWork;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }
        // GET api/values
        [HttpGet]
        public async Task<List<Product>> Get()
        {
            //NO CQRS
            var test = await _unitOfWork.GetInstance<IGenericRepository<Product>>().GetAllAsync();

            
            //With CQRS
            var query = _unitOfWork.GetInstance<IGenericRepository<Product>>();

            var result = await _queryDispatcher.DispatchAsync(query.GetAllAsync);


            return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
        {
            //NO CQRS
            var test = await _unitOfWork.GetInstance<IGenericRepository<Product>>().FindAsync(id);


            //With CQRS
            var query = _unitOfWork.GetInstance<IGenericRepository<Product>>();

            var result = await _queryDispatcher.DispatchAsync(query.FindAsync, id);

            return result;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Product value)
        {
            //NO CQRS
            var test = await _unitOfWork.GetInstance<IGenericRepository<Product>>().SaveAsync(value);
            await _unitOfWork.SaveChangesAsync();

            //With CQRS 
            var command = _unitOfWork.GetInstance<IGenericRepository<Product>>();
            await _commandDispatcher.DispatchAsync(command.SaveAsync, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Product value)
        {
            //NO CQRS
            var test = await _unitOfWork.GetInstance<IGenericRepository<Product>>().SaveAsync(new Product());

            //With CQRS
            var command = _unitOfWork.GetInstance<IGenericRepository<Product>>();
            await _commandDispatcher.DispatchAsync(command.SaveAsync, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
