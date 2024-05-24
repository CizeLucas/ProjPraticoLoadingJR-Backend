using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IPublicationsRepository
    {

        public Task<IActionResult> GetAllAsync();

        public Task<IActionResult> GetAsync(string Uuid);

        public Task<IEnumerable<Publications>> GetPaginatedAsync(int page, int pageSize);

        public Task<IActionResult> AddPublicationAsync(string Uuid, string publisherUuid, PublicationDTO publication);

        public Task<IActionResult> UpdatePublicationAsync(string Uuid, PublicationDTO publication);

        public Task<IActionResult> DeletePublicationAsync(string Uuid);


    }
}