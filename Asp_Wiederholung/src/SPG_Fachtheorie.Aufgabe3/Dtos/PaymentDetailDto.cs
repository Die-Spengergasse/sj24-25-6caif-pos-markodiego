public record PaymentDetailDto(
    int Id,
    string EmployeeFirstName,
    string EmployeeLastName,
    int CashDeskNumber,
    string PaymentType,
    DateTime Date,  // Added date field
    List<PaymentItemDto> PaymentItems
);
