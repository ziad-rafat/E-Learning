using E_Learning.Application.IService;
using E_Learning.Application.Services;
using E_Learning.Dtos.Topic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Learning.View.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }
        // GET: api/<TopicController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
          var topics=  ( await _topicService.GetAllTopicsAsync());
            if(topics!=null)
              return  Ok(topics.Entities);
            else 
                return Ok(null);
        }

        // GET api/<TopicController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var topic = (await _topicService.GetTopicAsync(id));
            if (topic != null)
                return Ok(topic);
            else
                return Ok(null);
        }

        // POST api/<TopicController>
        [HttpPost]
        public async Task<IActionResult> Post(CreateOrUpdateTopicDTO topicDTO)
        {
            if (ModelState.IsValid) 
            {
                var NewTopic= await _topicService.CreateTopicAsync(topicDTO);
                if(NewTopic.IsSuccess)
                  return Ok(NewTopic);
                else
                    return BadRequest(NewTopic.Message);
            }
            else 
                return BadRequest("InValid Data");
        }

        // PUT api/<TopicController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put( CreateOrUpdateTopicDTO topicDTO)
        {
            
            if (ModelState.IsValid)
            {
                var UpdatedTopic = await _topicService.UpdateTopicAsync(topicDTO);
                if (UpdatedTopic.IsSuccess)
                    return Ok(UpdatedTopic.Entity);
                else
                    return BadRequest(UpdatedTopic.Message);

            }
            else
                return BadRequest("InValid Data");
        }

        // DELETE api/<TopicController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var topic = await _topicService.GetTopicAsync(id);
            if (topic != null) 
            {
              var deletedTopic=  await _topicService.SoftDeleteTopicAsync(id);
                if (deletedTopic.IsSuccess)
                    return Ok(topic);
                else return BadRequest(deletedTopic.Message);
            }
            return BadRequest("topic Not found");
        }
    }
}
