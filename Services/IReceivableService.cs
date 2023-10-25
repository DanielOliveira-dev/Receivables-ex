using Services.DTO;

namespace Services
{
    public interface IReceivableService
    {
        Task AddReceivables(IEnumerable<ReceivableDTO> receivablesDTO);
        Task<SummaryStatisticsDTO> GetSummaryStatistics();
    }
}
