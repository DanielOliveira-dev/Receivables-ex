using Services.DTO;
using System.Globalization;

namespace Services.Validators
{
    public static class ReceivableDTOValidator
    {
        public static void ValidateReceivableDTO(ReceivableDTO dto)
        {
            if (dto.PaidValue > dto.OpeningValue)
            {
                throw new ArgumentException("Paid value cannot be higher then opening value.");
            }

            if (DateTime.TryParseExact(dto.IssueDate, new[] { "dd/MM/yyyy", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDate) &&
                DateTime.TryParseExact(dto.DueDate, new[] { "dd/MM/yyyy", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate))
            {
                if (issueDate > dueDate)
                {
                    throw new ArgumentException("Due date cannot be prior to issue date.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid date format for IssueDate or DueDate.");
            }

            if (dto.ClosedDate != null &&
                DateTime.TryParseExact(dto.ClosedDate, new[] { "dd/MM/yyyy", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime closedDate))
            {
                if (dueDate > closedDate)
                {
                    throw new ArgumentException("Closed date cannot be prior to due date.");
                }
            }
            else
            {
                throw new ArgumentException("Invalid date format for ClosedDate");
            }
        }
    }
}
