using AutoMapper;
using Data;
using Domain;
using Services.DTO;
using Services.Validators;

namespace Services
{
    public class ReceivableService : IReceivableService
    {
        private readonly IMapper _mapper;
        private readonly IReceivableRepository _receivableRepository;

        public ReceivableService(IMapper mapper, IReceivableRepository receivableRepository)
        {
            _mapper = mapper;
            _receivableRepository = receivableRepository;
        }

        public async Task AddReceivables(IEnumerable<ReceivableDTO> receivablesDTO)
        {
            try
            {
                foreach (var receivableDTO in receivablesDTO)
                {
                    ReceivableDTOValidator.ValidateReceivableDTO(receivableDTO);

                    var debtor = await _receivableRepository.GetDebtorByReference(receivableDTO.DebtorReference);

                    if (debtor == null)
                    {
                        debtor = _mapper.Map<ReceivableDTO, Debtor>(receivableDTO);
                        debtor.AddReceivable(_mapper.Map<ReceivableDTO, Receivable>(receivableDTO));
                        await _receivableRepository.AddDebtor(debtor);
                        continue;
                    }

                    var receivable = _mapper.Map<ReceivableDTO, Receivable>(receivableDTO);
                    var debtorExistingReceivable = debtor.Receivables.FirstOrDefault(rec => rec.Reference == receivable.Reference);

                    if(debtorExistingReceivable == null)
                    {
                        debtor.AddReceivable(receivable);
                        await _receivableRepository.SaveChangesAsync();
                    }
                    else
                    {
                        await _receivableRepository.UpdateReceivable(receivable);
                        await _receivableRepository.SaveChangesAsync();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the receivables.", ex);
            }
        }

        public async Task<SummaryStatisticsDTO> GetSummaryStatistics()
        {
            try
            {
                var closedInvoices = await _receivableRepository.GetClosedInvoices();
                var openInvoices = await _receivableRepository.GetOpenInvoices();

                return new SummaryStatisticsDTO
                {
                    ClosedInvoicesValue = closedInvoices.Sum(rec => rec.PaidValue),
                    OpenInvoicesValue = openInvoices.Sum(rec => rec.PaidValue)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching receivables.", ex);
            }
        }
    }
}