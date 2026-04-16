using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Dtos.Category;
using E_Learning.Dtos.Topic;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;
        public TopicService(ITopicRepository topicRepository, IMapper mapper)
        {
            _mapper = mapper;
            _topicRepository = topicRepository;
        }
        public async Task<ResultView<CreateOrUpdateTopicDTO>> CreateTopicAsync(CreateOrUpdateTopicDTO topic)
        {
            ResultView<CreateOrUpdateTopicDTO> result = new();
            try { 
            var topics = await _topicRepository.GetAllAsync();
            var oldTopic = topics.Where(i => i.Name == topic.Name).FirstOrDefault();
           
            if (oldTopic != null)
            {
                result.IsSuccess = false;
                result.Entity = null;
                result.Message = "Topc is already found";
                return result;
            }
            var Topic= _mapper.Map<Topic>(topic);
            var NewTopic = await _topicRepository.CreateAsync(Topic);
             await _topicRepository.SaveChangesAsync();
            var NewTopicDto = _mapper.Map<CreateOrUpdateTopicDTO>(NewTopic);
            result.Entity=NewTopicDto;
            result.IsSuccess=true;
            result.Message = "Topic Created Successfully";
            return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Entity = null;
                result.Message = "Something went wrong";
                return result;

            }
        }

        public async Task<ResultDataList<GetAllTopicDTO>> GetAllTopicsAsync()
        {
            try {
            var Topics = await _topicRepository.GetAllAsync();

            var TopicDTO = Topics.Where(c => c.IsDeleted == false).Include(i=>i.SubCategory).Select(c => new GetAllTopicDTO
            {
                Id = c.Id,
                Name = c.Name,
                SubCategoryName=c.SubCategory.Name,
                NumberOfCourses = c.Courses.Count(),
                NumberOfStudents = c.Courses.Select(i => i.UserId).Count()
            }).ToList();

            ResultDataList<GetAllTopicDTO> result = new ResultDataList<GetAllTopicDTO>();
            result.Entities = TopicDTO;
            result.Count = TopicDTO.Count();
            return result;
            }
            catch (Exception ex) 
            {
                ResultDataList<GetAllTopicDTO> result = new ResultDataList<GetAllTopicDTO>();
                result.Entities = null;
                result.Count = 0;
                return result;
            }
        }

        public async Task<ResultView<GetAllTopicDTO>> GetTopicAsync(Guid topicId)
        {
            try { 
            var Topic = await _topicRepository.GetByIdAsync(topicId);
            if(Topic == null)
            {
                return new ResultView<GetAllTopicDTO> { Entity = null, IsSuccess = false,Message="Topic is not found" };

            }
            var TopicDto = _mapper.Map<GetAllTopicDTO>(Topic);
            return new ResultView<GetAllTopicDTO> { Entity = TopicDto, IsSuccess = true, Message="Topic found Successfully" };
            }
            catch (Exception ex) 
            {
                return new ResultView<GetAllTopicDTO> { Entity = null, IsSuccess = false, Message = "Something went wrong" };
            }
        }

        public async Task<ResultView<CreateOrUpdateTopicDTO>> HardDeleteTopicAsync(Guid topicId)
        {
            try
            {
                var Topic = await _topicRepository.GetByIdAsync(topicId);
                if (Topic == null)
                {
                    return new ResultView<CreateOrUpdateTopicDTO> { Entity = null, IsSuccess = false, Message = "Topic is not Found" };

                }
                else
                {
                    var oldTopic = _topicRepository.DeleteAsync(Topic);
                    await _topicRepository.SaveChangesAsync();
                    var TopicDto = _mapper.Map<CreateOrUpdateTopicDTO>(oldTopic);
                    return new ResultView<CreateOrUpdateTopicDTO> { Entity = TopicDto, IsSuccess = true, Message = "Deleted Successfully" };

                }
            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateTopicDTO> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }

        public async Task<ResultView<CreateOrUpdateTopicDTO>> SoftDeleteTopicAsync(Guid topicId)
        {
            try
            {
                var Topic = await _topicRepository.GetByIdAsync(topicId);
                if (Topic == null) 
                {
                    return new ResultView<CreateOrUpdateTopicDTO> { Entity = null, IsSuccess = false, Message = "This Topic is not found" };

                }
                else 
                {
                    Topic.IsDeleted = true;
                    await _topicRepository.SaveChangesAsync();

                    var TopicDto = _mapper.Map<CreateOrUpdateTopicDTO>(Topic);
                    return new ResultView<CreateOrUpdateTopicDTO> { Entity = TopicDto, IsSuccess = true, Message = "Deleted Successfully" };

                }
            }
            catch (Exception ex)
            {
                return new ResultView<CreateOrUpdateTopicDTO> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }

        public async Task<ResultView<CreateOrUpdateTopicDTO>> UpdateTopicAsync(CreateOrUpdateTopicDTO topic)
        {
            ResultView<CreateOrUpdateTopicDTO> result = new();
            try
            {
                var oldTopic=(await _topicRepository.GetAllAsync()).AsNoTracking().FirstOrDefault(t=>t.Id==topic.Id);
               // var oldTopic = (await _topicRepository.GetByIdAsync(topic.Id));
               
              
                if (oldTopic == null)
                {
                    result.IsSuccess = false;
                    result.Entity = null;
                    result.Message = "Topc is not found";
                    return result;
                }
                var Ntopic = _mapper.Map<Topic>(topic);
                var Newtopic = await _topicRepository.UpdateAsync(Ntopic);
                await _topicRepository.SaveChangesAsync();
                var NewTopicDto = _mapper.Map<CreateOrUpdateTopicDTO>(Newtopic);
                result.Entity = NewTopicDto;
                result.IsSuccess = true;
                result.Message = "Topic Updated Successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Entity = null;
                result.Message = "Something went wrong";
                return result;

            }
        }
    }
}
