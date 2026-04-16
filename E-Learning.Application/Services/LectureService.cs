using AutoMapper;
using E_Learning.Application.Contract;
using E_Learning.Application.IService;
using E_Learning.Dtos.Lecture;
using E_Learning.Dtos.ViewResult;
using E_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Services
{
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IMapper _mapper;

        public LectureService(ILectureRepository lectureRepository, IMapper mapper)
        {
            _lectureRepository = lectureRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<CreateOrUpdateLecture>> Create(CreateOrUpdateLecture createLecture)
        {
            var lecture = _mapper.Map<Lecture>(createLecture);
            var newLecture = await _lectureRepository.CreateAsync(lecture);
            int saved = await _lectureRepository.SaveChangesAsync();
            if (saved == 0)
            {
                return new ResultView<CreateOrUpdateLecture> { Entity = null, IsSuccess = false, Message = "Create Lecture Failed" };
            }
            else
            {
                var lectureDto = _mapper.Map<CreateOrUpdateLecture>(newLecture);
                return new ResultView<CreateOrUpdateLecture> { Entity = lectureDto, IsSuccess = true, Message = "Lecture Created Successfully" };

            }
        }
        public async Task<ResultView<CreateOrUpdateLecture>> Update(CreateOrUpdateLecture updateLecture)
        {
            var existingLecture = await _lectureRepository.GetByIdAsync(updateLecture.Id);
            if (existingLecture == null)
                return new ResultView<CreateOrUpdateLecture> { Entity = null, IsSuccess = false, Message = "Lecture not Found" };

            _mapper.Map(updateLecture, existingLecture);

            await _lectureRepository.UpdateAsync(existingLecture);
            int saved = await _lectureRepository.SaveChangesAsync();
            if (saved == 0)
            {
                return new ResultView<CreateOrUpdateLecture> { Entity = null, IsSuccess = false, Message = "Update Lecture Failed" };
            }
            else
            {
                var lectureDto = _mapper.Map<CreateOrUpdateLecture>(existingLecture);
                return new ResultView<CreateOrUpdateLecture> { Entity = lectureDto, IsSuccess = true, Message = "Lecture Updated Successfully" };

            }

        }

        public async Task<ResultView<LectureDto>> HardDelete(LectureDto lectureDto)
        {
            var existingLecture = await _lectureRepository.GetByIdAsync(lectureDto.Id);
            if (existingLecture == null)
                return new ResultView<LectureDto> { Entity = null, IsSuccess = false, Message = "Lecture not Found" };
            await _lectureRepository.DeleteAsync(existingLecture);
            int saved = await _lectureRepository.SaveChangesAsync();
            if (saved == 0)
                return new ResultView<LectureDto> { Entity = null, IsSuccess = false, Message = "Delete Lecture Failed" };
            else
            {
                var deletedLecture = _mapper.Map<LectureDto>(existingLecture);
                return new ResultView<LectureDto> { Entity = deletedLecture, IsSuccess = true, Message = "Lecture Deleted Successfully" };

            }

        }

        public async Task<ResultView<LectureDto>> SoftDelete(Guid lectureId)
        {
            var existingLecture = await _lectureRepository.GetByIdAsync(lectureId);
            if (existingLecture == null)
                return new ResultView<LectureDto> { Entity = null, IsSuccess = false, Message = "Lecture not Found" };
            existingLecture.IsDeleted = true;
            int saved = await _lectureRepository.SaveChangesAsync();
            if (saved == 0)
                return new ResultView<LectureDto> { Entity = null, IsSuccess = false, Message = "Delete Lecture Failed" };
            else
            {
                var deletedLecture = _mapper.Map<LectureDto>(existingLecture);
                return new ResultView<LectureDto> { Entity = deletedLecture, IsSuccess = true, Message = "Lecture Deleted Successfully" };

            }
        }

        public async Task<ResultDataList<LectureDto>> GetAll()
        {
            var lectures = await _lectureRepository.GetAllAsync();
            var lecturesDtos = lectures.Where(lecture => lecture.IsDeleted == false)
                .Select(lecture => _mapper.Map<LectureDto>(lecture)).ToList();

            return new ResultDataList<LectureDto> { Entities = lecturesDtos, Count = lecturesDtos.Count };
        }

        public async Task<ResultView<LectureDto>> Getone(Guid id)
        {
            var lecture = await _lectureRepository.GetByIdAsync(id);
            if (lecture == null)
                return new ResultView<LectureDto> { Entity = null, IsSuccess = false, Message = "Lecture not Found" };

            return new ResultView<LectureDto> { Entity = _mapper.Map<LectureDto>(lecture), IsSuccess = true, Message = "Lecture Found Successfully" };

        }

        public async Task<ResultDataList<LectureDto>> GetSectionLecture(Guid sectionId)
        {
            var lectures = await _lectureRepository.GetAllAsync();
            var sectionLectures = lectures.Where(lec => lec.IsDeleted == false && lec.Section.Id == sectionId)
                .Select(lecture => _mapper.Map<LectureDto>(lecture)).ToList();

            return new ResultDataList<LectureDto> { Entities = sectionLectures, Count = sectionLectures.Count };
        }

        public async Task<ResultDataList<LectureDto>> GetCourseLecture(Guid courseId)
        {
            var lectures = await _lectureRepository.GetAllAsync();
            var courseLectures = lectures.Where(lec => lec.IsDeleted == false && lec.Section.Course.Id == courseId)
                .Select(lecture => _mapper.Map<LectureDto>(lecture)).ToList();

            return new ResultDataList<LectureDto> { Entities = courseLectures, Count = courseLectures.Count };
        }


    }
}
