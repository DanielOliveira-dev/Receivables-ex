using AutoFixture;
using Domain;
using Services.DTO;

namespace ReceivablesTests
{
    public class ReceivableDTOBuilder
    {
        private readonly IFixture fixture = new Fixture();
        private readonly ReceivableDTO receivableDTO;

        public ReceivableDTOBuilder()
        {
            receivableDTO = new ReceivableDTO
            {
                Reference = fixture.Create<int>(),
                CurrencyCode = CurrencyCode.GBP.ToString(),
                IssueDate = DateTime.Today.ToString("dd-MM-yyyy"),
                OpeningValue = 5000,
                PaidValue = 2500,
                DueDate = DateTime.Today.AddMonths(1).ToString("dd-MM-yyyy"),
                DebtorName = fixture.Create<string>(),
                DebtorReference = fixture.Create<int>(),
                DebtorCountryCode = CountryCode.AU.ToString()
            };
        }

        public ReceivableDTOBuilder WithReference(int reference)
        {
            receivableDTO.Reference = reference;
            return this;
        }

        public ReceivableDTOBuilder WithCurrencyCode(string currencyCode)
        {
            receivableDTO.CurrencyCode = currencyCode;
            return this;
        }

        public ReceivableDTOBuilder WithIssueDate(string issueDate)
        {
            receivableDTO.IssueDate = issueDate;
            return this;
        }

        public ReceivableDTOBuilder WithOpeningValue(double openingValue)
        {
            receivableDTO.OpeningValue = openingValue;
            return this;
        }

        public ReceivableDTOBuilder WithPaidValue(double paidValue)
        {
            receivableDTO.PaidValue = paidValue;
            return this;
        }

        public ReceivableDTOBuilder WithDueDate(string dueDate)
        {
            receivableDTO.DueDate = dueDate;
            return this;
        }

        public ReceivableDTOBuilder WithClosedDate(string closedDate)
        {
            receivableDTO.ClosedDate = closedDate;
            return this;
        }

        public ReceivableDTOBuilder WithCancelled(bool? cancelled)
        {
            receivableDTO.Cancelled = cancelled;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorName(string debtorName)
        {
            receivableDTO.DebtorName = debtorName;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorReference(int debtorReference)
        {
            receivableDTO.DebtorReference = debtorReference;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorAddress1(string debtorAddress1)
        {
            receivableDTO.DebtorAddress1 = debtorAddress1;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorAddress2(string debtorAddress2)
        {
            receivableDTO.DebtorAddress2 = debtorAddress2;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorTown(string debtorTown)
        {
            receivableDTO.DebtorTown = debtorTown;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorState(string debtorState)
        {
            receivableDTO.DebtorState = debtorState;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorZip(string debtorZip)
        {
            receivableDTO.DebtorZip = debtorZip;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorCountryCode(string debtorCountryCode)
        {
            receivableDTO.DebtorCountryCode = debtorCountryCode;
            return this;
        }

        public ReceivableDTOBuilder WithDebtorRegistrationNumber(string debtorRegistrationNumber)
        {
            receivableDTO.DebtorRegistrationNumber = debtorRegistrationNumber;
            return this;
        }

        public ReceivableDTO Build()
        {
            return receivableDTO;
        }
    }
}
