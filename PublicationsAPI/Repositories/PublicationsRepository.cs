using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicationsAPI.Data;
using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Repositories {
    public class PublicationsRepository : IPublicationsRepository
    {
        private readonly AppDBContext _context;

        public PublicationsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Publications> AddPublicationAsync(Publications publication)
        {
            if(publication == null)
                throw new InvalidDataException("publication cannot be null when trying to add it do the database");

            //Populates the author object field with the correct Author object
            publication.Author = await _context.Users.FirstOrDefaultAsync(u => u.Uuid == publication.AuthorUuid);

            try {
                _context.Publications.Add(publication);
                await _context.SaveChangesAsync();
            } catch (Exception e) {
                throw new Exception(e.Message + e.InnerException.ToString());
            }

            return publication;
        }

        public async Task<bool> DeletePublicationAsync(string Uuid)
        {
            Publications? publication = await _context.Publications.FirstOrDefaultAsync(p => p.Uuid == Uuid);

            if (publication == null)
                return false;

            try {
                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();
            } catch (Exception e) {
                throw new Exception(e.Message + e.InnerException.ToString());
            }

            return true;
        }

        public async Task<IEnumerable<Publications>> GetAllPublicationsFromUserAsync(string publisherUuid)
        {
            return await _context.Publications.Where(p => p.Author.Uuid == publisherUuid).ToListAsync();
        }

        public async Task<Publications> GetPublicationAsync(string Uuid)
        {
            return await _context.Publications.FirstOrDefaultAsync(p => p.Uuid == Uuid);
           /* Publications? publication = await _context.Publications.FirstOrDefaultAsync(p => p.Uuid == Uuid);

            if(publication == null)
                return null;

            return publication;*/
        }

        public async Task<IEnumerable<Publications>> GetPublicationsPaginatedAsync(string publisherUuid, int page, int pageSize)
        {
            return await (_context.Publications.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize)).ToListAsync();
        }

        public async Task<Publications> UpdatePublicationAsync(Publications publication)
        {
            try {
                _context.Publications.Update(publication);
                await _context.SaveChangesAsync();
            } catch (Exception e) {
                throw new Exception(e.Message + e.InnerException.ToString());
            }

            return publication;
        }
    }
}