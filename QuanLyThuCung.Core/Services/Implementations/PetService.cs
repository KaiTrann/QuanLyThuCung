using QuanLyThuCung.Core.Models;
using QuanLyThuCung.Core.Services.Interfaces;

namespace QuanLyThuCung.Core.Services.Implementations
{
    /// <summary>
    /// Implementation of pet management service
    /// </summary>
    public class PetService : IPetService
    {
        private readonly List<Pet> _pets = new();
        private int _nextId = 1;

        public List<Pet> GetAllPets()
        {
            return _pets.ToList();
        }

        public Pet? GetPetById(int id)
        {
            return _pets.FirstOrDefault(p => p.Id == id);
        }

        public List<Pet> GetAvailablePets()
        {
            return _pets.Where(p => p.IsAvailable).ToList();
        }

        public void AddPet(Pet pet)
        {
            if (pet == null)
                throw new ArgumentNullException(nameof(pet));
            if (string.IsNullOrWhiteSpace(pet.Name))
                throw new ArgumentException("Pet name is required", nameof(pet));
            if (string.IsNullOrWhiteSpace(pet.Species))
                throw new ArgumentException("Pet species is required", nameof(pet));
            if (pet.Price < 0)
                throw new ArgumentException("Pet price cannot be negative", nameof(pet));
            if (pet.Age < 0)
                throw new ArgumentException("Pet age cannot be negative", nameof(pet));
                
            pet.Id = _nextId++;
            pet.DateAdded = DateTime.Now;
            _pets.Add(pet);
        }

        public void UpdatePet(Pet pet)
        {
            if (pet == null)
                throw new ArgumentNullException(nameof(pet));
                
            var existingPet = GetPetById(pet.Id);
            if (existingPet == null)
                throw new InvalidOperationException($"Pet with ID {pet.Id} not found");
                
            var index = _pets.IndexOf(existingPet);
            _pets[index] = pet;
        }

        public void DeletePet(int id)
        {
            var pet = GetPetById(id);
            if (pet != null)
            {
                _pets.Remove(pet);
            }
        }

        public List<Pet> SearchPets(string keyword)
        {
            return _pets.Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Species.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Breed.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
    }
}
