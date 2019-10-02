using MediatR;
using SampleAPI.Contracts.DTO.Response;

namespace SampleAPI.Contracts.DTO.Request
{
    public class TestRequest : IRequest<TestResponse>
    {
        public int ID { get; set; }
    }
}
