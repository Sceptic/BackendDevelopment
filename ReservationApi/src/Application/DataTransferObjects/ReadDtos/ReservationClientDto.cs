namespace Application.DTOs.Reservations;

public sealed record ReservationClientDto
(
    int ReservationId,
    string FirstName,
    string LastName,
    DateTime BirthDate
);
