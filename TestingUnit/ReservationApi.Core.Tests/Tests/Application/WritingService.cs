using Application.Reservations;
using Domain.Models;

namespace ReservationApi.Core.Tests.Application;

public sealed partial class ReservationApplicationTests
{
    [Fact] //UT-RES-APP-WRITE-CREATE-002
    public async Task CommandService_CreateAsync_invokes_checks_persists_and_returns_assigned_id()
    {
        var repo = new FakeReservationRepository();
        var availability = new FakeAvailabilityChecker();
        var external = new FakeExternalPolicy();

        var service = new ReservationCommandService(repo, availability, external);

        var created = await service.CreateAsync(MakeCreateRequest(accountId: 1234, discount: 0.00m), CancellationToken.None);

        Assert.Equal(1, availability.Calls);
        Assert.Null(availability.LastExcludeReservationId);

        Assert.Equal(1, external.Calls);

        Assert.Equal(1, repo.CreateCalls);
        Assert.NotNull(repo.LastCreated);
        Assert.True(created.ReservationId > 0);
        Assert.Equal(created.ReservationId, repo.LastCreated!.ReservationId);
        Assert.Equal(1234, created.AccountId);
        Assert.Single(created.Clients);
        Assert.Single(created.Restaurants);
    }

    [Fact] //UT-RES-APP-WRITE-PATCH-003
    public async Task CommandService_PatchAsync_returns_null_when_not_found()
    {
        var repo = new FakeReservationRepository();
        var availability = new FakeAvailabilityChecker();
        var external = new FakeExternalPolicy();

        var service = new ReservationCommandService(repo, availability, external);

        var updated = await service.PatchAsync(MakePatchRequest(999, reservationStatus: "CANCELLED"), CancellationToken.None);

        Assert.Null(updated);
        Assert.Equal(1, repo.GetByIdTrackedCalls);
        Assert.Equal(0, repo.UpdateCalls);
        Assert.Equal(0, availability.Calls);
        Assert.Equal(0, external.Calls);
    }

    [Fact] //UT-RES-APP-WRITE-PATCH-004
    public async Task CommandService_PatchAsync_updates_period_safely_and_calls_conflict_checker_with_exclusion()
    {
        var repo = new FakeReservationRepository();
        var availability = new FakeAvailabilityChecker();
        var external = new FakeExternalPolicy();

        var existing = MakeReservation(accountId: 1001);
        existing.Clients.Add(new ReservationClient { FirstName = "Anna", LastName = "Peeters", BirthDate = new DateTime(1985, 03, 22) });
        repo.Seed(5, existing);

        var service = new ReservationCommandService(repo, availability, external);

        var newEnd = existing.ReservationEnd.AddDays(2);
        var updated = await service.PatchAsync(
            MakePatchRequest(5, end: newEnd, paymentStatus: "UNPAID"),
            CancellationToken.None);

        Assert.NotNull(updated);
        Assert.Equal("UNPAID", updated!.PaymentStatus);
        Assert.Equal(newEnd, updated.ReservationEnd);

        Assert.Equal(1, availability.Calls);
        Assert.Equal(5, availability.LastExcludeReservationId);

        Assert.Equal(1, external.Calls);
        Assert.Equal(1, repo.UpdateCalls);
    }

    [Fact] //UT-RES-APP-WRITE-DELETE-005
    public async Task CommandService_DeleteAsync_returns_false_when_missing_and_true_when_deleted()
    {
        var repo = new FakeReservationRepository();
        var availability = new FakeAvailabilityChecker();
        var external = new FakeExternalPolicy();
        var service = new ReservationCommandService(repo, availability, external);

        var missing = await service.DeleteAsync(123, CancellationToken.None);
        Assert.False(missing);
        Assert.Equal(1, repo.GetByIdCalls);
        Assert.Equal(0, repo.DeleteCalls);

        repo.Seed(7, MakeReservation(accountId: 1001));

        var ok = await service.DeleteAsync(7, CancellationToken.None);
        Assert.True(ok);
        Assert.Equal(2, repo.GetByIdCalls);
        Assert.Equal(1, repo.DeleteCalls);
        Assert.Equal(7, repo.LastDeletedId);
    }

    private static Reservation MakeReservation(int accountId)
    {
        var start = new DateTime(2026, 06, 01, 14, 00, 00);
        var end = new DateTime(2026, 06, 10, 10, 00, 00);

        var r = new Reservation
        {
            AccountId = accountId,
            ReservationStatus = "CONFIRMED",
            PaymentStatus = "PAID",
            ReservationPrice = 1200m,
            Discount = 0.10m,
            TouristTarif = 0.05m,
            ReservationStart = start,
            ReservationEnd = end,
        };

        return r;
    }
}
