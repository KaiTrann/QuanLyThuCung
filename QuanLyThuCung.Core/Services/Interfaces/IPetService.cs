using QuanLyThuCung.Core.Models;

namespace QuanLyThuCung.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for managing pets in the pet shop
    /// </summary>
    public interface IPetService
    {
        List<Pet> GetAllPets();
        Pet? GetPetById(int id);
        List<Pet> GetAvailablePets();
        void AddPet(Pet pet);
        void UpdatePet(Pet pet);
        void DeletePet(int id);
        List<Pet> SearchPets(string keyword);
    }
}
