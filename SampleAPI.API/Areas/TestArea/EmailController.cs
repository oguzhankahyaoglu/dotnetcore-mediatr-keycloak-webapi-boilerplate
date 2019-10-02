using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleAPI.API.Areas.TestArea
{
    [Area("TestArea")]
    [Route("[controller]"), Authorize(Roles = "Email", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme),
     ApiController]
    public class TestAreaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestAreaController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}