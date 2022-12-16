using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ElevenNote.Models.Note;
using ElevenNote.Data;
using Microsoft.EntityFrameworkCore;
using ElevenNote.Data.Entities;

namespace ElevenNote.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly int _userID;
        private readonly ApplicationDbContext _dbContext;
        public NoteService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            var userClaims = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            var value = userClaims.FindFirst("ID")?.Value;

            var validID = int.TryParse(value, out _userID);

            if (!validID)
            {
                throw new Exception("Attempted to build NoteService without User ID claim.");
            }

            _dbContext = dbContext;
        }

        public async Task<bool> CreateNoteAsync(NoteCreate request)
        {
            var noteEntity = new NoteEntity
            {
                Title = request.Title,
                Content = request.Content,
                CreatedUtc = DateTimeOffset.Now,
                OwnerID = _userID
            };

            _dbContext.Notes.Add(noteEntity);

            var numberOfChanges = await _dbContext.SaveChangesAsync();

            return numberOfChanges == 1;
        }

        public async Task<IEnumerable<NoteListItem>> GetAllNotesAsync()
        {
            var notes = await _dbContext.Notes
            .Where(entity => entity.OwnerID == _userID)
            .Select(entity => new NoteListItem
            {
                ID = entity.ID,
                Title = entity.Title,
                CreatedUtc = entity.CreatedUtc
            })
        .ToListAsync();
        
            return notes;
        }
    }
}