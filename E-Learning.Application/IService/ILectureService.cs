using E_Learning.Dtos.Lecture;
using E_Learning.Dtos.ViewResult;

namespace E_Learning.Application.IService
{
    public interface ILectureService
    {
        Task<ResultView<CreateOrUpdateLecture>> Create(CreateOrUpdateLecture createLecture);
        Task<ResultDataList<LectureDto>> GetAll();
        Task<ResultDataList<LectureDto>> GetCourseLecture(Guid courseId);
        Task<ResultView<LectureDto>> Getone(Guid id);
        Task<ResultDataList<LectureDto>> GetSectionLecture(Guid sectionId);
        Task<ResultView<LectureDto>> HardDelete(LectureDto lectureDto);
        Task<ResultView<LectureDto>> SoftDelete(Guid lectureId);
        Task<ResultView<CreateOrUpdateLecture>> Update(CreateOrUpdateLecture updateLecture);
    }
}