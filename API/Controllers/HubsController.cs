using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HubsController : ControllerBase
    {
        private readonly ILogger<HubsController> _logger;
        private readonly ForgeService _forgeService;

        public HubsController(ILogger<HubsController> logger, ForgeService forgeService)
        {
            _logger = logger;
            _forgeService = forgeService;
        }

        [HttpGet()]
        public async Task<ActionResult<string>> ListHubs()
        {
            var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
            if (tokens == null)
            {
                return Unauthorized("No tokens avaliable");
            }
            var hubs = await _forgeService.GetHubs(tokens.InternalToken);
            _logger.LogInformation("Hubs lastet ned");
            return JsonConvert.SerializeObject(hubs);
        }
        
        [HttpGet("{hub}/projects")]
        public async Task<ActionResult<string>> ListProjects(string hub)
        {
            var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
            if (tokens == null)
            {
                return Unauthorized();
            }
            var projects = await _forgeService.GetProjects(hub, tokens);
            return JsonConvert.SerializeObject(projects);
        }

        [HttpGet("{hub}/projects/{project}/contents")]
        public async Task<ActionResult<string>> ListItems(string hub, string project, [FromQuery] string? folder_id)
        {
            var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
            if (tokens == null)
            {
                return Unauthorized();
            }
            var contents = await _forgeService.GetContents(hub, project, folder_id, tokens);
            return JsonConvert.SerializeObject(contents);
        }

        [HttpGet("{hub}/projects/{project}/contents/{item}/versions")]
        public async Task<ActionResult<string>> ListVersions(string hub, string project, string item)
        {
            var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
            if (tokens == null)
            {
                return Unauthorized();
            }
            var versions = await _forgeService.GetVersions(hub, project, item, tokens);
            return JsonConvert.SerializeObject(versions);
        }

        [HttpGet("{hub}/bucket/{bucketKey}/contents/{objectName}/links")]
        public async Task<ActionResult<string>> GetDownloadLinks(string bucketKey, string objectName)
        {
            var tokens = await _forgeService.PrepareTokens(Request, Response, _forgeService);
            if (tokens == null)
            {
                return Unauthorized();
            }
            var decodedBucketKey = HttpUtility.UrlDecode(bucketKey);
            var key = decodedBucketKey.Split("/").Last();
            Console.WriteLine(key);
            var versions = await _forgeService.GetFileLink(key, objectName, tokens);
            return JsonConvert.SerializeObject(versions);
        }
    }
}