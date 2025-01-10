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
        private readonly int _pageSize = 20;
        private readonly IAbonentRepo _repository;
        private readonly IRemainRepo _remainRepository;
        private readonly IMapper _mapper;
        private readonly ISwitchTarif _switch;

        public AbonentsController(IAbonentRepo repository, IMapper mapper, IRemainRepo remainRepo, ISwitchTarif switchTarif)
        {
            _remainRepository = remainRepo;
            _repository = repository;
            _mapper = mapper;
            _switch = switchTarif;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AbonentReadDto>> GetAbonents([FromQuery] int page = 1)
        {
            Console.WriteLine("Get Abonents");
            if (page <= 0)
                return BadRequest("Invalid page number");

            var abonentItem = _repository.GetAllAbonents();

            if (!abonentItem.Any())
                return NoContent();

            var startIndex = (page - 1) * _pageSize;

            var pagedItems = abonentItem.Skip(startIndex).Take(_pageSize).ToList();

            return Ok(_mapper.Map<IEnumerable<AbonentReadDto>>(pagedItems));
        }

        [HttpGet("{id}")]
        public ActionResult<AbonentReadDto> GetAbonentById(int id)
        {
            Console.WriteLine("Get abonent by id");
            var abonentItem = _repository.GetAbonentById(id);

            if (abonentItem != null)
                return Ok(_mapper.Map<AbonentReadDto>(abonentItem));

            return NoContent();
        }

        [HttpPost]
        public ActionResult<AbonentCreateDto> CreateAbonent(AbonentCreateDto newAbonent)
        {
            var abonent = _repository.CreateAbonent(newAbonent);
            var abonentReadDto = _mapper.Map<AbonentReadDto>(abonent);
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

        [HttpPut("SwitchTarif")]
        public ActionResult SwitchTarif(int abonentId, SwitchTarifDto newTarif)
        {
            _switch.UpdateTarif(newTarif, abonentId);
            return Ok();
        }
    }
}
