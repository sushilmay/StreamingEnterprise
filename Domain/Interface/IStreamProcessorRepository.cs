
namespace Streaming.Producer.Domain
{
    public interface IStreamProcessorRepository
    {
        Task<Process?> GetByIdAsync(Guid id);
        Task AddAsync(Process process);
        Task UpdateAsync(Process process);
    }
}
