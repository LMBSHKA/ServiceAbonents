﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Controllers
{
    [Route("v1/Abonents/")]
    [ApiController]
    public class AbonentsController : ControllerBase
    {
        private readonly int _pageSize = 20;
        private readonly IAbonentRepo _repository;
        private readonly IMapper _mapper;
        private readonly ISwitchTarif _switch;

        public AbonentsController(IAbonentRepo repository, IMapper mapper, ISwitchTarif switchTarif)
        {
            _repository = repository;
            _mapper = mapper;
            _switch = switchTarif;
        }

        /// <summary>
        /// Получение всех пользователей, есть фильтрация и пагинация
        /// </summary>
        /// <param name="filter">абонента</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        public ActionResult<GetAllAbonentsDto> GetAbonents([FromQuery] FilterDto filter,
            [FromQuery] int page = 1)
        {
            Console.WriteLine("Get Abonents");
            if (page <= 0)
                return BadRequest("Invalid page number");

            var abonentItem = _repository.GetAllAbonents(filter);
            var startIndex = (page - 1) * _pageSize;
            var pagedItems = abonentItem.Skip(startIndex).Take(_pageSize).ToList();
            var map = _mapper.Map<IEnumerable<AbonentReadDto>>(pagedItems);

            if ((double)abonentItem.Count() / _pageSize <= 1)
                return Ok(new GetAllAbonentsDto { abonentRead = map, PageCount = 1 });

            return Ok(new GetAllAbonentsDto { abonentRead = map,
                PageCount = Math.Ceiling((double)abonentItem.Count() / _pageSize) });
        }

        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <param name="id">абонента</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
        public ActionResult<AbonentReadDto> GetAbonentById(Guid id)
        {
            Console.WriteLine("Get abonent by id");
            var abonentItem = _repository.GetAbonentById(id);

            if (abonentItem != null)
                return Ok(_mapper.Map<AbonentReadDto>(abonentItem));

            return NoContent();
        }

        /// <summary>
        /// Создание абонента
        /// </summary>
        /// <param name="newAbonent"></param>
        /// <param name="temporaryId"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]
        public async Task<IActionResult> CreateAbonent(AbonentCreateDto newAbonent, Guid temporaryId)
        {
            await _repository.CreateAbonent(newAbonent, temporaryId);

            return Ok();
        }

        /// <summary>
        /// Обновление данных абонента
        /// </summary>
        /// <param name="id">абонента</param>
        /// <param name="updateAbonent"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut]
        public ActionResult UpdatePut(Guid id, AbonentsUpdateDto updateAbonent)
        {
            _repository.Update(id, updateAbonent);
            return Ok();
        }

        /// <summary>
        /// обновление тарифа абонента
        /// </summary>
        /// <param name="abonentId"></param>
        /// <param name="newTarif"></param>
        /// <param name="tarifId"></param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("SwitchTarif")]
        public ActionResult SwitchTarif(Guid abonentId, SwitchTarifDto newTariff, string tariffId, decimal tariffCost)
        {
            _switch.UpdateTarif(newTariff, abonentId, tariffId, tariffCost);
            return Ok();
        }
    }
}
