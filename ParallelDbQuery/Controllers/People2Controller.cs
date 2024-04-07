using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParallelDbQuery.Data;
using ParallelDbQuery.Models;

namespace ParallelDbQuery.Controllers;

[Route("People2")]
public class People2Controller : ControllerBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    ILogger<People2Controller> _logger;
   
    public People2Controller(IServiceScopeFactory serviceScopeFactory, ILogger<People2Controller> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    [HttpGet(Name = "Get2")]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Use Task.WhenAll to await all tasks concurrently
            var peopleTask = Task.Run(() => GetPeopleAsync());
            var people2Task = Task.Run(() => GetPeople2Async());
            var businessTask = Task.Run(() => GetBusinessEntitiesAsync());
            var emailaddressTask = Task.Run(() => GetEmailAddressAsync());

            var salesOrderDetailTask = Task.Run(() => GetSalesOrderDetailsAsync());
            var salesOrderHeaderTask = Task.Run(() => GetSalesOrderHeadersAsync());
            var transactionHistoryTask = Task.Run(() => GetTransactionHistoriesAsync());
            var transactionHistoryArchives= Task.Run(() => GetTransactionHistoryArchivesAsync());
            var workOrderRoutings = Task.Run(() => GetWorkOrderRoutingsAsync());

            await Task.WhenAll(peopleTask, people2Task, businessTask, emailaddressTask, salesOrderDetailTask, salesOrderHeaderTask, transactionHistoryTask, transactionHistoryArchives, workOrderRoutings);

            var result = new
            {
                people = (await peopleTask),
                people2 = (await people2Task),
                businessEntities = (await businessTask),
                emailAddress = (await emailaddressTask),
                salesOrderDetail = (await salesOrderDetailTask),
                salesOrderHeader = (await salesOrderHeaderTask),
                transactionHistory = (await transactionHistoryTask),
                transactionHistoryArchive = (await transactionHistoryArchives),
                workOrderRouting = (await workOrderRoutings),
            };

            _logger.LogInformation($"Request {Counter.Count}");
 

            Counter.Count++;
            return Ok(result);
        }
        catch (Exception ex)
        {
         
            _logger.LogInformation($"An exception occurred: {ex.Message}");
            return BadRequest();
        }
    }

    private async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await _context.People.AsNoTracking()
                //.Include(p => p.BusinessEntity)
             .ToListAsync();
            return  result.Take(100);
        }
    }

    private async Task<IEnumerable<Person>> GetPeople2Async()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await _context.People.AsNoTracking()
             //.Include(p => p.BusinessEntity)
             .ToListAsync();
            return result.Take(100);
        }
    }

    private async Task<IEnumerable<BusinessEntity>> GetBusinessEntitiesAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await _context.BusinessEntities.AsNoTracking()
                .AsNoTracking()
             .ToListAsync();
            return  result.Take(100);
        }
    }

    private async Task<IEnumerable<EmailAddress>> GetEmailAddressAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await _context.EmailAddresses.AsNoTracking()
             //.Include(p => p.BusinessEntity)
             .ToListAsync();
            return  result.Take(100);
        }
    }

    private async Task<IEnumerable<SalesOrderDetail>> GetSalesOrderDetailsAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await dbContext.SalesOrderDetails.AsNoTracking().ToListAsync();

            return  result.Take(100);
        }
    }

    private async Task<IEnumerable<SalesOrderHeader>> GetSalesOrderHeadersAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await dbContext.SalesOrderHeaders.AsNoTracking()
                .ToListAsync();
            return result.Take(100);
        }
    }

    private async Task<IEnumerable<TransactionHistory>> GetTransactionHistoriesAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await dbContext.TransactionHistories.AsNoTracking().ToListAsync();
            return  result.Take(100);
        }
    }

    private async Task<IEnumerable<TransactionHistoryArchive>> GetTransactionHistoryArchivesAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await dbContext.TransactionHistoryArchives.AsNoTracking().ToListAsync();
            return result.Take(100);
        }
    }

    private async Task<IEnumerable<WorkOrderRouting>> GetWorkOrderRoutingsAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var result = await dbContext.WorkOrderRoutings.AsNoTracking().ToListAsync();
            return result.Take(100);
        }
    }
}


