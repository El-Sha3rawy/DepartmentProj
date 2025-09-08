using ApplicationLayer.DepartmentsCqrs.Commands;
using ApplicationLayer.DepartmentsCqrs.Queries;
using AutoMapper;
using DomainLayer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace ApiLayer.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class DeptController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        

        public DeptController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDept([FromBody] DepartmentDto DeptDto)
        {
            var result = await _mediator.Send(new CreateDeptCommand(DeptDto));
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeptById(int id)
        {
            var result = await _mediator.Send(new GetDeptByIdQuery (id));
            

            return Ok(result);
        }

        [HttpGet]
        public async Task <IActionResult> GetAllDepts()
        {
            var result = await _mediator.Send (new GetAllDeptQuery());
            
            return Ok(result);
        }        

        [HttpPut("{id}")]
        public async Task <IActionResult> UpdateDept(int id , [FromBody] DepartmentDto DeptDto)
        {
            await _mediator.Send(new UpdateDeptCommand (id,DeptDto));
            
            return Ok(new { Message = $"The Department Is Updated Successfully " });
        }

        [HttpDelete("{id}")]
        public async Task <IActionResult>DeleteDept(int id)
        {
            await _mediator.Send(new DeleteDeptCommand(id));

            return Ok(new { Message = $"The Department Is Deleted Successfully " }); 
        }
    }
}
