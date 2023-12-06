using PersonaVault.Contracts.DTOs;
using PersonaVault.Database.Models;

namespace PersonaVault.Database.Repositories
{
    public interface IAddressDetailsRepository
    {
        Task CreateAddressDetailsAndAddToUser(EncryptedAddressDetailsDTO data, User user);
        Task UpdateCountry(byte[] country, User user);
        Task UpdateCity(byte[] city, User user);
        Task UpdateStreet(byte[] street, User user);
        Task UpdateHouseNumber(byte[] houseNumber, User user);
        Task UpdateApartamentNumber(byte[] apartamentNumber, User user);
    }
}