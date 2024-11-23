using AutoMapper;
//using Microsoft.AspNetCore.JsonPatch;
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
		public ActionResult<RemainReadDto> GetRemainByAbonentId(int id)
		{
			var remainItem = _repository.GetRemainByAbonentId(id);
			if (remainItem != null)
				return Ok(_mapper.Map<RemainReadDto>(remainItem));
            return NotFound();
		}

		[HttpPut]
		public ActionResult UpdatePut (int clientId, RemainUpdateDto updateRemain)
		{
			var remain = _repository.GetRemainByAbonentId(clientId);

			if (updateRemain.RemainGb != 0)
				remain.ReaminGb = updateRemain.RemainGb;

			if (updateRemain.RemainMin != 0)
				remain.RemainMin = updateRemain.RemainMin;

			if (updateRemain.RemainSMS != 0)
				remain.RemainSMS = updateRemain.RemainSMS;

			_repository.Update(remain);
			return Ok();
		}

		//Для наших задач больше подходит put
		//[HttpPatch]
  //      [Route("{clientId::int}/Update")]
		//public ActionResult Update (int clientId, [FromBody] JsonPatchDocument<Remain> patchDoc)
		//{
		//	if (patchDoc == null)
		//		return BadRequest();

		//	var remain = _repository.GetRemainByAbonentId(clientId);

		//	patchDoc.ApplyTo(remain, ModelState);

		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	_repository.Update(remain);
		//	_repository.SaveChanges();

		//	return Ok();
		//}
    }
}