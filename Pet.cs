using System;

namespace Nhóm_7
{
    public class Pet
    {
        public int PetId { get; set; }
        public int OwnerId { get; set; }

        public string Name { get; set; } = ""; 
        public string Species { get; set; } = "";
        public string Breed { get; set; } = "";
        public string Sex { get; set; } = "";
        public DateTime? BirthDate { get; set; }

        public string OwnerName { get; set; } = "";
    }
}