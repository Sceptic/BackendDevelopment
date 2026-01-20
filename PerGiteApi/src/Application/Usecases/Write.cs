using Application.DtoModels;
using Application.Abstractions;
using Domain.Models;
using Domain.Specs;

namespace Application.Gites.WriteQueries
{
    public sealed class GiteWritingService
    {
        private readonly IGiteRepository _repo;
        private readonly IUnitOfWork _uow;

        public GiteWritingService(IGiteRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<int> CreateAsync(GiteDto dto, CancellationToken ct)
        {
            var amenities = ToAmenitiesSpec(dto.Amenities);
            var beds = ToBedSpecs(dto.Beds);

            var gite = Gite.Create(
                dto.GiteNumber,
                dto.GitePrice,
                dto.IsAvailable,
                dto.GiteAddress,
                dto.CapacityMin,
                dto.CapacityMax,
                amenities,
                beds);

            await _repo.AddAsync(gite, ct);
            await _uow.SaveChangesAsync(ct);
            return gite.GiteId;
        }

        public async Task UpdateAsync(int id, GiteDto dto, CancellationToken ct)
        {
            //Ophalen van de volledige entiteit (aggregate) in vanuit de Db.
            var gite = await _repo.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException("Gite not found");

            //Gebruikt de ingebouwde invariants die structureel de waardes correct veranderen, Ctrl+Shift op de method om ze te bekijken
            gite.ChangePrice(dto.GitePrice);
            gite.ChangeAddress(dto.GiteAddress);
            gite.ChangeCapacity(dto.CapacityMin, dto.CapacityMax);

            if (dto.IsAvailable) gite.MarkAvailable();
            else gite.MarkUnavailable();

            //Gebruikt wederom de ingebouwde invariants specifiek voor het updaten van waardes, Ctrl+Shift op de method om ze te bekijken
            gite.SetAmenitiesFromSpec(ToAmenitiesSpec(dto.Amenities));
            gite.ReplaceBeds(ToBedSpecs(dto.Beds));

            await _repo.UpdateAsync(gite, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var gite = await _repo.GetByIdAsync(id, ct)
                ?? throw new InvalidOperationException("Gite not found");

            await _repo.DeleteAsync(gite, ct);
            await _uow.SaveChangesAsync(ct);
        }

        //Helper methods om de sub-entiteiten te vullen/aanpassen, schoner dan deze code elke keer te copy-en-pasten.
        private static GiteAmenitiesSpec ToAmenitiesSpec(GiteAmenitiesDto dto)
        {
            return new GiteAmenitiesSpec(
                dto.Wifi,
                dto.Bath,
                dto.Shower,
                dto.HairDryer,
                dto.SmallChild,
                dto.Toiletries,
                dto.Desk,
                dto.Chair,
                dto.Balcony,
                dto.Sofa,
                dto.SofaBed,
                dto.MiniFridge,
                dto.Kettle,
                dto.Cuttlery,
                dto.EatingArea,
                dto.RoomService);
        }
            
        private static IEnumerable<GiteBedSpec> ToBedSpecs(IEnumerable<GiteBedDto> beds)
        {
            return beds.Select(b => new GiteBedSpec(
                b.Amount1PrBed,
                b.Amount2PrBed,
                b.Amount3PrBed,
                b.BedSort));
        }     
    }
}
