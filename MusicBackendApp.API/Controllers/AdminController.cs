using Hangfire; 
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.Common.Interfaces.Search; 

namespace MusicBackendApp.Controllers;

[Route("api/[controller]")] 
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ISearchService _searchService; 

    public AdminController(
        IBackgroundJobClient backgroundJobClient,
        ISearchService searchService) 
    {
        _backgroundJobClient = backgroundJobClient;
        _searchService = searchService;
    }
    
    [HttpPost("reindex-elasticsearch")] 
    public IActionResult ReindexElasticsearch()
    {
        string jobId = _backgroundJobClient.Enqueue<ISearchService>(service => service.ReindexAllDataAsync());
        
        Console.WriteLine($"[AdminController] Started Elasticsearch reindex job with ID: {jobId}");

        return Ok($"Elasticsearch reindex job started. Job ID: {jobId}");
    }
    
     [HttpPost("schedule-daily-backup")]
     public IActionResult ScheduleDailyBackup()
     {
         RecurringJobManager recurringJobManager = new RecurringJobManager();
         recurringJobManager.AddOrUpdate("daily-backup", () => Console.WriteLine("Performing daily backup!"), Cron.Daily());
         return Ok("Daily backup scheduled.");
    }
}