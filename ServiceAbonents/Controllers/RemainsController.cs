using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceAbonents.Data;
using ServiceAbonents.Dtos;

namespace ServiceAbonents.Controllers
{
	[Route("v1/Remains/")]
	[ApiController]
	public class RemainsController : ControllerBase
	{
		private readonly IRemainRepo _repository;
		private readonly IMapper _mapper;

		public RemainsController(IRemainRepo repo, IMapper map)
		{
			_mapper = map;
			_repository = repo;
		}

        /// <summary>
        /// Получение остатков всех пользователей
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
		public ActionResult<IEnumerable<RemainReadDto>> GetRemain()
		{
			var remainItem = _repository.GetAllRemains();
			return Ok(_mapper.Map<IEnumerable<RemainReadDto>>(remainItem));
		}

        /// <summary>
        /// Получение остатков пользователя по его id
        /// </summary>
        /// <param name="id">абонента</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
		public ActionResult<RemainReadDto> GetRemainByAbonentId(Guid id)
		{
			var remainItem = _repository.GetRemainByAbonentId(id);
			if (remainItem != null)
				return Ok(_mapper.Map<RemainReadDto>(remainItem));
            return NotFound();
		}

        /// <summary>
        /// Обновление остатков пользователя
        /// </summary>
        /// <param name="clientId">абонента</param>
        /// <param name="updateRemain">абонента</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Ошибка API(скоре всего неправильные данные)</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut]
		public ActionResult UpdatePut (Guid clientId, RemainUpdateDto updateRemain)
		{
			_repository.Update(clientId, updateRemain);
			return Ok();
		}
    }
}