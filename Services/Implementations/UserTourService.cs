using System;
using System.Threading.Tasks;
using Tour_Module.DTOs.Tours;
using Tour_Module.Repository.Interfaces;
using Tour_Module.Services.Interfaces;

namespace Tour_Module.Services.Implementations
{
    public class UserTourService : IUserTourService
    {
        private readonly IUserTourRepository _userTourRepository;

        public UserTourService(IUserTourRepository userTourRepository)
        {
            _userTourRepository = userTourRepository
                ?? throw new ArgumentNullException(nameof(userTourRepository));
        }

        public async Task<UserTourDetailsDto?> GetUserTourDetailsAsync(int tourId)
        {
            // -------------------------------------------------
            // 1️⃣ Guard clause (business-level validation)
            // -------------------------------------------------
            if (tourId <= 0)
                return null;

            // -------------------------------------------------
            // 2️⃣ Fetch user-safe read model
            // -------------------------------------------------
            var tour = await _userTourRepository.GetUserTourDetailsAsync(tourId);

            if (tour == null)
                return null;

            // -------------------------------------------------
            // 3️⃣ Derived values (belongs in SERVICE)
            // -------------------------------------------------
            //tour.TotalPrice = tour.BasePrice;
            // Later:
            // + selected add-ons
            // + discounts
            // + coupons
            // + taxes

            return tour;
        }
    }
}
