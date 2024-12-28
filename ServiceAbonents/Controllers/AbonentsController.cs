using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Controllers
{
    //параметр который не дает доступ без авторизации то есть без jwt токена
    //[Authorize]
    [Route("v1/Abonents/")]
    [ApiController]
    public class AbonentsController : ControllerBase
    {
        private readonly IAbonentRepo _repository;
        private readonly IRemainRepo _remainRepository;
        private readonly IMapper _mapper;

        public AbonentsController(IAbonentRepo repository, IMapper mapper, IRemainRepo remainRepo)
        {
            _remainRepository = remainRepo;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AbonentReadDto>> GetAbonents()
        {
            Console.WriteLine("Get Abonents");
            var abonentItem = _repository.GetAllAbonents();
            return Ok(_mapper.Map<IEnumerable<AbonentReadDto>>(abonentItem));
        }

        [HttpGet("{id}")]
        public ActionResult<AbonentReadDto> GetAbonentById(int id)
        {
            Console.WriteLine("Get abonent by id");
            var abonentItem = _repository.GetAbonentById(id);

            if (abonentItem != null)
                return Ok(_mapper.Map<AbonentReadDto>(abonentItem));

            return BadRequest("User not found");
        }

        [HttpPost]
        public ActionResult<AbonentCreateDto> CreateAbonent(AbonentCreateDto abonentCreateDto)
        {
            var abonentModel = _mapper.Map<User>(abonentCreateDto);
            _repository.CreateAbonent(abonentModel);
            var abonentReadDto = _mapper.Map<AbonentReadDto>(abonentModel);
            var remain = new Remain { ClientId = abonentReadDto.Id, ReaminGb = 0, RemainMin = 0, RemainSMS = 0 };
            _remainRepository.CreateRemain(remain);
            _remainRepository.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public ActionResult UpdatePut(int id, AbonentsUpdateDto updateAbonent)
        {
            _repository.Update(id, updateAbonent);
            return Ok();
        }
    }
}
