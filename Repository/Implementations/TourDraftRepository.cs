using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tour_Module.Data;
using Tour_Module.Models.Entities;
using Tour_Module.Repository.Interfaces;

namespace Tour_Module.Repository.Implementations
{
    public class TourDraftRepository : ITourDraftRepository
    {
        private readonly ApplicationDbContext _context;

        public TourDraftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateDraftAsync(int adminId)
        {
            var draftIdParam = new SqlParameter("@DraftId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_CreateTourDraft @CreatedByAdminId, @DraftId OUTPUT",
                new SqlParameter("@CreatedByAdminId", adminId),
                draftIdParam
                );

            return (int)draftIdParam.Value;
        }

        public async Task<TourDraft?> GetDraftByIdAsync(int draftId, int adminId)
        {
            var result = await _context.TourDrafts
                .FromSqlRaw("EXEC dbo.usp_GetTourDraftById @DraftId, @CreatedByAdminId",
                    new SqlParameter("@DraftId", draftId),
                    new SqlParameter("@CreatedByAdminId", adminId))
                .AsNoTracking()
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task UpdateStep1Async(int draftId, int adminId, string title, string overview)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_UpdateTourDraftStep1 @DraftId, @CreatedByAdminId, @TourTitle, @Overview",
                    new SqlParameter("@DraftId", draftId),
                    new SqlParameter("@CreatedByAdminId", adminId),
                    new SqlParameter("@TourTitle", title),
                    new SqlParameter("@Overview", overview)
                );
        }

        public async Task UpdateDraftLocationAsync(int draftId, int adminId, int locationId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_UpdateDraftLocation @DraftId, @AdminId, @LocationId",
                new SqlParameter("@DraftId", draftId),
                new SqlParameter("@AdminId", adminId),
                new SqlParameter("@LocationId", locationId)
            );
        }

        public async Task DeleteDraftImagesAsync(int draftId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_DeleteDraftImages @DraftId",
                new SqlParameter("@DraftId", draftId)
            );
        }

        public async Task AddDraftImageAsync(int draftId, int locationImageId, bool isCover)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_AddDraftImage @DraftId, @LocationImageId, @IsCover",
                new SqlParameter("@DraftId", draftId),
                new SqlParameter("@LocationImageId", locationImageId),
                new SqlParameter("@IsCover", isCover));
        }

        public async Task<List<TourDraftImage>> GetDraftImagesAsync(int draftId)
        {
            return await _context.TourDraftImages
                .FromSqlRaw("EXEC dbo.usp_GetDraftImages @DraftId",
                    new SqlParameter("@DraftId", draftId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task EnsureBaseComponentAsync(int draftId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_EnsureBaseComponent @DraftId",
                new SqlParameter("@DraftId", draftId)
            );
        }

        public async Task<List<TourDraftPricingComponent>> GetPricingComponentsAsync(int draftId)
        {
            var result = await _context.TourDraftPricingComponents
                .FromSqlRaw(
                    "EXEC dbo.usp_GetDraftPricingComponents @DraftId",
                    new SqlParameter("@DraftId", draftId))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        //public async Task<TourDraftPricingComponent?> GetComponentByIdAsync(int componentId)
        //{
        //    return await _context.TourDraftPricingComponents
        //        .FirstOrDefaultAsync(c => c.PricingComponentId == componentId);
        //}

        public async Task AddPricingComponentAsync(int draftId, int optionId, string componentName, decimal price, bool isBaseComponent)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_AddDraftPricingComponent @DraftId, @OptionId, @ComponentName, @Price, @IsBaseComponent",
                new SqlParameter("@DraftId", draftId),
                new SqlParameter("@OptionId", optionId),
                new SqlParameter("@ComponentName", componentName),
                new SqlParameter("@Price", price),
                new SqlParameter("@IsBaseComponent", isBaseComponent)
            );
        }

        public async Task RemovePricingComponentAsync(int componentId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_RemoveDraftPricingComponent @PricingComponentId",
                new SqlParameter("@PricingComponentId", componentId)
            );
        }

        public async Task UpdateComponentPriceAsync(int componentId, decimal price)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_UpdateDraftPricingComponentPrice @ComponentId, @Price",
                new SqlParameter("@ComponentId", componentId),
                new SqlParameter("@Price", price)
            );
        }

        public async Task SetDraftStepAsync(int draftId, int step)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_SetDraftStep @DraftId, @Step",
                new SqlParameter("@DraftId", draftId),
                new SqlParameter("@Step", step)
            );
        }

        public async Task UpdateStep3Async(int draftId, string duration)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_UpdateTourDraftStep3 @DraftId, @Duration",
                new SqlParameter("@DraftId", draftId),
                new SqlParameter("@Duration", duration ?? (object)DBNull.Value)
            );
        }

        public async Task UpdateDraftStatusAsync(int draftId, string status)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_UpdateDraftStatus @DraftId, @Status",
                new SqlParameter("@DraftId", draftId),
                new SqlParameter("@Status", status)
            );
        }



    }
}
