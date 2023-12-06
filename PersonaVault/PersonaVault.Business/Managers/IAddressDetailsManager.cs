using PersonaVault.Contracts.Requests;
using PersonaVault.Contracts.Responses;

namespace PersonaVault.Business.Managers
{
    public interface IAddressDetailsManager
    {
        Task<ActionResponse> CreateAddressDetails(CreateAddressDetailsRequest request, Guid userId);
        Task<ActionResponse> UpdateCountry(string newCountry, Guid userId);
        Task<ActionResponse> UpdateCity(string newCity, Guid userId);
        Task<ActionResponse> UpdateStreet(string newStreet, Guid userId);
        Task<ActionResponse> UpdateHouseNumber(string newHouseNumber, Guid userId);
        Task<ActionResponse> UpdateApartamentNumber(string newApartamentNumber, Guid userId);
    }
}