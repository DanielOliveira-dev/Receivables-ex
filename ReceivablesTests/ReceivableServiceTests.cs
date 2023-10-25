using AutoFixture;
using AutoMapper;
using Data;
using Domain;
using FluentAssertions;
using Moq;
using ReceivablesAPI;
using Services;
using Services.DTO;

namespace ReceivablesTests
{
    public class ReceivableServiceTests
    {
        private readonly IFixture fixture;
        private readonly IMapper mapper;

        public ReceivableServiceTests()
        {
            fixture = new Fixture();
            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
        }

        [Fact]
        public async Task AddReceivables_WithNewDebtor_ShouldAddDebtorAndReceivable()
        {
            // Arrange
            var receivableRepositoryMock = new Mock<IReceivableRepository>();

            var service = new ReceivableService(mapper, receivableRepositoryMock.Object);
            var date = DateTime.UtcNow;
            var value = fixture.Create<double>();
            var receivableDTO = new ReceivableDTOBuilder()
                .WithOpeningValue(value + 1)
                .WithPaidValue(value)
                .WithIssueDate(date.ToString("dd-MM-yyyy"))
                .WithDueDate(date.AddDays(2).ToString("dd-MM-yyyy"))
                .WithClosedDate(date.AddDays(3).ToString("dd-MM-yyyy"))
                .Build();

            receivableRepositoryMock.Setup(repo => repo.GetDebtorByReference(receivableDTO.DebtorReference)).ReturnsAsync((Debtor)null);

            // Act
            await service.AddReceivables(new List<ReceivableDTO> { receivableDTO });

            // Assert
            receivableRepositoryMock.Verify(repo => repo.AddDebtor(It.IsAny<Debtor>()), Times.Once);
            receivableRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddReceivables_WithExistingDebtorAndNoReceivable_ShouldAddReceivable()
        {
            // Arrange
            var receivableRepositoryMock = new Mock<IReceivableRepository>();

            var service = new ReceivableService(mapper, receivableRepositoryMock.Object);

            var date = DateTime.UtcNow;
            var value = fixture.Create<double>();
            var receivableDTO = new ReceivableDTOBuilder()
                .WithOpeningValue(value + 1)
                .WithPaidValue(value)
                .WithIssueDate(date.ToString("dd-MM-yyyy"))
                .WithDueDate(date.AddDays(2).ToString("dd-MM-yyyy"))
                .WithClosedDate(date.AddDays(3).ToString("dd-MM-yyyy"))
                .Build();
            var existingDebtor = mapper.Map<ReceivableDTO, Debtor>(receivableDTO);

            receivableRepositoryMock.Setup(repo => repo.GetDebtorByReference(existingDebtor.Reference)).ReturnsAsync(existingDebtor);

            // Act
            await service.AddReceivables(new List<ReceivableDTO> { receivableDTO });

            // Assert
            receivableRepositoryMock.Verify(repo => repo.UpdateReceivable(It.IsAny<Receivable>()), Times.Never);
            receivableRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddReceivables_WithExistingDebtorAndReceivable_ShouldUpdateReceivable()
        {
            // Arrange
            var receivableRepositoryMock = new Mock<IReceivableRepository>();

            var service = new ReceivableService(mapper, receivableRepositoryMock.Object);

            var date = DateTime.UtcNow;
            var value = fixture.Create<double>();
            var receivableDTO = new ReceivableDTOBuilder()
                .WithOpeningValue(value + 1)
                .WithPaidValue(value)
                .WithIssueDate(date.ToString("dd-MM-yyyy"))
                .WithDueDate(date.AddDays(2).ToString("dd-MM-yyyy"))
                .WithClosedDate(date.AddDays(3).ToString("dd-MM-yyyy"))
                .Build();
            var existingDebtor = mapper.Map<ReceivableDTO, Debtor>(receivableDTO);
            existingDebtor.AddReceivable(mapper.Map<ReceivableDTO, Receivable>(receivableDTO));

            receivableRepositoryMock.Setup(repo => repo.GetDebtorByReference(existingDebtor.Reference)).ReturnsAsync(existingDebtor);

            // Act
            await service.AddReceivables(new List<ReceivableDTO> { receivableDTO });

            // Assert
            // Verify that UpdateReceivable and SaveChangesAsync were called as expected
            receivableRepositoryMock.Verify(repo => repo.UpdateReceivable(It.IsAny<Receivable>()), Times.Once);
            receivableRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddReceivables_ThrowException_HandleException()
        {
            // Arrange
            var receivableRepositoryMock = new Mock<IReceivableRepository>();
            receivableRepositoryMock.Setup(repo => repo.GetDebtorByReference(It.IsAny<int>())).ThrowsAsync(new Exception("Simulated exception"));

            var service = new ReceivableService(mapper, receivableRepositoryMock.Object);

            var date = DateTime.UtcNow;
            var value = fixture.Create<double>();
            var receivableDTO = new ReceivableDTOBuilder()
                .WithOpeningValue(value + 1)
                .WithPaidValue(value)
                .WithIssueDate(date.ToString("dd-MM-yyyy"))
                .WithDueDate(date.AddDays(2).ToString("dd-MM-yyyy"))
                .WithClosedDate(date.AddDays(3).ToString("dd-MM-yyyy"))
                .Build();

            // Act
            var result = () => service.AddReceivables(new List<ReceivableDTO>() { receivableDTO});

            // Assert
            await result.Should().ThrowAsync<Exception>()
                .WithMessage("An error occurred while adding the receivables.");
        }

        [Fact]
        public async Task GetSummaryStatistics_BeingCalled_ReturnSummaryStatisticsDTO()
        {
            // Arrange
            var receivableRepositoryMock = new Mock<IReceivableRepository>();

            var service = new ReceivableService(mapper, receivableRepositoryMock.Object);

            var closedInvoices = fixture.CreateMany<Receivable>().ToList();
            var openInvoices = fixture.CreateMany<Receivable>().ToList();

            receivableRepositoryMock.Setup(repo => repo.GetClosedInvoices()).ReturnsAsync(closedInvoices);
            receivableRepositoryMock.Setup(repo => repo.GetOpenInvoices()).ReturnsAsync(openInvoices);

            // Act
            var result = await service.GetSummaryStatistics();

            // Assert
            var expectedClosedInvoicesValue = closedInvoices.Sum(rec => rec.PaidValue);
            var expectedOpenInvoicesValue = openInvoices.Sum(rec => rec.PaidValue);

            receivableRepositoryMock.Verify(repo => repo.GetClosedInvoices(), Times.Once);
            receivableRepositoryMock.Verify(repo => repo.GetOpenInvoices(), Times.Once);
            result.ClosedInvoicesValue.Should().Be(expectedClosedInvoicesValue);
            result.OpenInvoicesValue.Should().Be(expectedOpenInvoicesValue);
        }

        [Fact]
        public async Task GetSummaryStatistics_ThrowException_HandleException()
        {
            // Arrange
            var receivableRepositoryMock = new Mock<IReceivableRepository>();
            receivableRepositoryMock.Setup(repo => repo.GetClosedInvoices()).ThrowsAsync(new Exception("Simulated exception"));

            var service = new ReceivableService(mapper, receivableRepositoryMock.Object);

            // Act
            var result = () => service.GetSummaryStatistics();

            // Assert
            await result.Should().ThrowAsync<Exception>()
                .WithMessage("An error occurred while fetching receivables.");
        }
    }
}
