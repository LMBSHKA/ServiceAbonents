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

		[HttpGet]
		public ActionResult<IEnumerable<RemainReadDto>> GetRemain()
		{
			var remainItem = _repository.GetAllRemains();
			return Ok(_mapper.Map<IEnumerable<RemainReadDto>>(remainItem));
		}
		
		[HttpGet("{id}")]
		public ActionResult<RemainReadDto> GetRemainByAbonentId(Guid id)
		{
			var remainItem = _repository.GetRemainByAbonentId(id);
			if (remainItem != null)
				return Ok(_mapper.Map<RemainReadDto>(remainItem));
            return NotFound();
		}

		[HttpPut]
		public ActionResult UpdatePut (Guid clientId, RemainUpdateDto updateRemain)
		{
			_repository.Update(clientId, updateRemain);
			return Ok();
		}
    }
}