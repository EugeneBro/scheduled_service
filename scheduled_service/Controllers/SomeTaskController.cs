using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using scheduled_service.Database.Models;
using scheduled_service.Jobs;
using scheduled_service.Services;

namespace scheduled_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeTaskController : ControllerBase
    {
        private SomeTaskService SomeTaskService { get; }

        //static SomeTaskController()
        //{
        //    StatusScheduler.Start();
        //}

        public SomeTaskController(SomeTaskService someTaskService)
        {
            SomeTaskService = someTaskService;
        }

        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SomeTask))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<SomeTask> GetTaskAsync(Guid id)
        {
            return await SomeTaskService.GetAsync(id);
        }

        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<string> CreateTask()
        {
            return await SomeTaskService.CreateAsync();
        }
    }
}
