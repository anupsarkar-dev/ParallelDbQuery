using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParallelDbQuery.Data;

namespace ParallelDbQuery.Controllers
{
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;
        ILogger<People2Controller> _logger;

        public PeopleController(AppDbContext context, ILogger<People2Controller> logger)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public async Task<IActionResult> Index()
        {
      

            var people = await _context.People
                //.Include(p => p.BusinessEntity)
                .ToListAsync();

            var people2 = await _context.People
                //.Include(p => p.BusinessEntity)
                .ToListAsync();

            var businessEntities = await _context.BusinessEntities 
               .AsNoTracking()
               .ToListAsync();

            var emailAddress = await _context.EmailAddresses
                //.Include(p => p.BusinessEntity)
              .ToListAsync();

            var salesOrderDetail = await _context.SalesOrderDetails.ToListAsync();

            var salesOrderHeader = await _context.SalesOrderHeaders
                .ToListAsync();

            var transactionHistory = await _context.TransactionHistories.ToListAsync();

            var transactionHistoryArchive = await _context.TransactionHistoryArchives.ToListAsync();

            var workOrderRouting = await _context.WorkOrderRoutings.ToListAsync();

            var result = new
                {
                   peoples = people.Take(100),
                   peoples2 = people2.Take(100),
                   businessEntities = businessEntities.Take(100),
                   emailAddress = emailAddress.Take(100),
                   salesOrderDetail = salesOrderDetail.Take(100),
                   salesOrderHeader = salesOrderHeader.Take(100),
                   transactionHistory = transactionHistory.Take(100),
                   transactionHistoryArchive = transactionHistoryArchive.Take(100),
                   workOrderRouting = workOrderRouting.Take(100),
             };

            _logger.LogInformation($"Request {Counter.Count}");


            Counter.Count++;

            return Ok(result );
        }

        //[HttpGet(Name = "Get")]
        //public async IAsyncEnumerable<SalesOrderDetail> Index2()
        //{
        //    await foreach(var item in  _context.SalesOrderDetails.AsAsyncEnumerable())
        //    {
        //        yield return item;
        //    } 
        //}

        //[HttpPost(Name = "Post")]
        //public async Task<IActionResult> Post()
        //{
        //    var result = await _context.SalesOrderDetails.ToListAsync();
        //    return Ok(result);
        //}

    }
}
