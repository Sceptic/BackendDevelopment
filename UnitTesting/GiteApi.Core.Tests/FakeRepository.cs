using Application.Abstractions;
using Domain.Models;
using System.Reflection;

namespace GiteApi.Core.Tests.Application;

internal sealed class FakeGiteRepository : IGiteRepository
{
    private static void SetGiteId(Gite gite, int id)
    {
        var field = typeof(Gite).GetField("<GiteId>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Cannot set GiteId backing field.");
        field.SetValue(gite, id);
    }

    public int GetByIdCalls { get; private set; }
    public int GetAllCalls { get; private set; }
    public int AddCalls { get; private set; }
    public int UpdateCalls { get; private set; }
    public int DeleteCalls { get; private set; }

    public Gite? LastAdded { get; private set; }
    public Gite? LastUpdated { get; private set; }
    public Gite? LastDeleted { get; private set; }

    private readonly Dictionary<int, Gite> _store = new();

    public void Seed(int id, Gite gite)
    {
        SetGiteId(gite, id);
        _store[id] = gite;
    }

    public Task<Gite?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        GetByIdCalls++;
        _store.TryGetValue(id, out var gite);
        return Task.FromResult(gite);
    }

    public Task<IReadOnlyList<Gite>> GetAllAsync(CancellationToken ct = default)
    {
        GetAllCalls++;
        return Task.FromResult((IReadOnlyList<Gite>)_store.Values.ToList());
    }

    public Task AddAsync(Gite gite, CancellationToken ct = default)
    {
        AddCalls++;
        LastAdded = gite;

        if (gite.GiteId == 0)
        {
            var newId = _store.Count == 0 ? 1 : _store.Keys.Max() + 1;
            SetGiteId(gite, newId);
            _store[newId] = gite;
        }
        else
        {
            _store[gite.GiteId] = gite;
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(Gite gite, CancellationToken ct = default)
    {
        UpdateCalls++;
        LastUpdated = gite;
        _store[gite.GiteId] = gite;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Gite gite, CancellationToken ct = default)
    {
        DeleteCalls++;
        LastDeleted = gite;
        _store.Remove(gite.GiteId);
        return Task.CompletedTask;
    }
}
