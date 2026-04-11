using BuildingBlocks.Exceptions;

namespace Streaming.Producer.Application.Exceptions
{
    public class ProcessIdNotFoundException : NotFoundException
    {
        public ProcessIdNotFoundException(string processId) : base("ProcessId", processId)
        {

        }
    }
}
