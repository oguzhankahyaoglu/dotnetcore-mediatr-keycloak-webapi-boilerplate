using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SampleAPI.Contracts.DTO.Request;
using SampleAPI.Contracts.DTO.Response;
using SampleAPI.Data;

namespace SampleAPI.Business.Implementations
{
    public class TestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        private readonly SampleAPIContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestHandler(SampleAPIContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestResponse
            {
                Result = new TestDto
                {
                    Date = DateTime.Now
                }
            });
        }
    }
}
