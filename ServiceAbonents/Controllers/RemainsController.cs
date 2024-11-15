using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ServiceAbonents.Data;
using ServiceAbonents.Dtos;
using ServiceAbonents.Models;

namespace ServiceAbonents.Controllers
{
	[Route("apishka/{controller}")]
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

		//[HttpPost]
		//public ActionResult<RemainCreateDto> CreateRemain(RemainCreateDto newRemain)
		//{
		//	var remainModel = _mapper.Map<Remain>(newRemain);
		//	_repository.CreateRemain(remainModel);
		//	_repository.SaveChanges();
		//	var remainReadDto = _mapper.Map<RemainReadDto>(remainModel);
		//	return CreatedAtRoute(nameof(GetRemainByAbonentId), new { CleintId = remainReadDto.ClientId }, remainReadDto);//nameof(GetRemainById), new { Id = remainReadDto.Id }, remainReadDto);
		//}

		[HttpGet("{id}")]
		public ActionResult<RemainReadDto> GetRemainByAbonentId(int id)
		{
			var remainItem = _repository.GetRemainByAbonentId(id);
			if (remainItem != null)
				return Ok(_mapper.Map<RemainReadDto>(remainItem));
            return NotFound();
		}

		[HttpPatch]
        [Route("{clientId::int}/Update")]
		public ActionResult Update (int clientId, [FromBody] JsonPatchDocument<Remain> patchDoc)
		{
			if (patchDoc == null)
				return BadRequest();

			var remain = _repository.GetRemainByAbonentId(clientId);

			patchDoc.ApplyTo(remain, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_repository.Update(remain);
			_repository.SaveChanges();

			return Ok();
		}
    }
}
