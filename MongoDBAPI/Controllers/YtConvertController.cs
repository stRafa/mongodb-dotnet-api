using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDBAPI.Services;

namespace MongoDBAPI.Controllers
{
    [Route("api/convert")]
    [ApiController]
    public class YtConvertController : ControllerBase
    {
        private readonly IMongoCollection<YtAudio> _audioFilesCollection;

        private readonly YtConvertService _ytConvertService;

        public YtConvertController(YtConvertService ytConvertService)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("youtubedb");
            _audioFilesCollection = database.GetCollection<YtAudio>("audioFiles");
            _ytConvertService = ytConvertService;
        }

        [HttpPost("{ytId}")]
        public async Task<IActionResult> PostYt(string ytId)
        {
            await _ytConvertService.PostAudio(ytId);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs()
        {
            return Ok(await _ytConvertService.GetYtAudios());
        }
    }
}
