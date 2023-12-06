using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using PersonaVault.Database.Models;

namespace PersonaVault.Business.Services
{
    public interface IEncryptionService
    {
        EncryptedPersonalDetailsDTO EncryptPersonalDetails(NewPersonalDetailsDTO request);
        EncryptedAddressDetailsDTO EncryptAddressDetails(CreateAddressDetailsRequest data);
        byte[] EncryptStringToBytes(string plainText);
        FullUserData DecrpytFullUserData(User user);
        FullUserData DecrpytUserAndPersonalDetails(User user);
        FullUserData DecrpytUserData(User user);
    }
}