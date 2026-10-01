using Microsoft.EntityFrameworkCore;
using Tour_Module.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class LocationImageRepository : ILocationImageRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LocationImage>> GetImagesByLocationIdAsync(int locationId)
        {
            var images = await _context.LocationImages
                .FromSqlRaw("EXEC SP_Location_GetImagesByLocationId @p0", locationId)
                .ToListAsync();

            return images;
        }

        public async Task AddLocationImageAsync(LocationImage locationImage)
        {
            await _context.LocationImages.AddAsync(locationImage);
            await _context.SaveChangesAsync();
        }

    }
}
