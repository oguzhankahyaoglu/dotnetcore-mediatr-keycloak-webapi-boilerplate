using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Contracts.DTO.Request;
using SampleAPI.Contracts.DTO.Response;

namespace SampleAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("test")]
        public Task<TestResponse> Test([FromForm] TestRequest request) =>
            _mediator.Send(request);
    }
}
