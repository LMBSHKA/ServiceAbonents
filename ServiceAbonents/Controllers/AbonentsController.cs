using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class AbonentsController : ControllerBase
    {
        private readonly IAbonentRepo _repository;
        private readonly IMapper _mapper;

        public AbonentsController(IAbonentRepo repository, IMapper mapper)
        {
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

        [HttpGet("{id}", Name = "GetAbonentById")]
        public ActionResult<AbonentReadDto> GetAbonentById(int id)
        {
            Console.WriteLine("Get abonent by id");
            var abonentItem = _repository.GetAbonentById(id);
            if (abonentItem != null)
                return Ok(_mapper.Map<AbonentReadDto>(abonentItem));
            return NotFound();
        }

        [HttpPost]
        public ActionResult<AbonentCreateDto> CreateAbonent(AbonentCreateDto abonentCreateDto)
        {
            var abonentModel = _mapper.Map<Abonent>(abonentCreateDto);
            _repository.CreateAbonent(abonentModel);
            _repository.SaveChange();
            var abonentReadDto = _mapper.Map<AbonentReadDto>(abonentModel);
            return CreatedAtRoute(nameof(GetAbonentById), new { Id = abonentReadDto.Id }, abonentReadDto);
        }

        [HttpPatch]
        [Route("{id::int}/Update")]
        public ActionResult Update(int id, [FromBody] JsonPatchDocument<Abonent> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var abonent = _repository.GetAbonentById(id);
            
            //patchDoc.Replace(e => e.Name, "NameUp");
            patchDoc.ApplyTo(abonent, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Update(abonent);
            _repository.SaveChange();
            return Ok();//Ok(_mapper.Map<Abonent>(abonent));
        }

        //[HttpGet("{TariffId}", Name = "GetTariffIdByAbonentId")]
        //public ActionResult<int> GetTarrifIdByAbonentId(int id)
        //{
        //    var abonentItem = _repository.GetTariffIdByAbonentId(id);
        //    return Ok(_mapper.Map<int>(abonentItem));
        //}
    }
}
