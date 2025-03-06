public record PaymentDto(
    int Id,
    string EmployeeFirstName,
    string EmployeeLastName,
    int CashDeskNumber,
    string PaymentType,
    decimal TotalAmount
);
