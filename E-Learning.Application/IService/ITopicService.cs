using E_Learning.Dtos.Topic;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.IService
{
    public interface ITopicService
    {
        Task<ResultView<CreateOrUpdateTopicDTO>> CreateTopicAsync(CreateOrUpdateTopicDTO topic);
        Task<ResultView<CreateOrUpdateTopicDTO>>UpdateTopicAsync(CreateOrUpdateTopicDTO topic);
        Task<ResultView<CreateOrUpdateTopicDTO>> HardDeleteTopicAsync(Guid topicId);
        Task<ResultView<CreateOrUpdateTopicDTO>> SoftDeleteTopicAsync(Guid topicId);
        Task<ResultDataList<GetAllTopicDTO>> GetAllTopicsAsync();
        Task<ResultView<GetAllTopicDTO>> GetTopicAsync(Guid topicId);
        
    }
}
