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

        public async Task<NoteDetail?> GetNoteByIDAsync(int noteID)
        {
            // Find the first note that has the given ID and an OwnerID that matches the requesting userID
            var noteEntity = await _dbContext.Notes
            .FirstOrDefaultAsync(e =>
            e.ID == noteID && e.OwnerID == _userID);

            // If noteEntity is null then return null, otherwise intialize and return a new NoteDetail
            return noteEntity is null ? null : new NoteDetail
            {
                ID = noteEntity.ID,
                Title = noteEntity.Title,
                Content = noteEntity.Content,
                CreatedUtc = noteEntity.CreatedUtc,
                ModifiedUtc = noteEntity.ModifiedUtc
            };
        }

        public async Task<bool> UpdateNoteAsync(NoteUpdate request)
        {
            // Find the note and validate it's owner by the user
            var noteEntity = await _dbContext.Notes.FindAsync(request.ID);

            // By using the null conditional operator we can check if its null at the same time we check the ownerID
            if (noteEntity?.OwnerID != _userID)
            {
                return false;
            }

            // Now we update the entity's properties
            noteEntity.Title = request.Title;
            noteEntity.Content = request.Content;
            noteEntity.ModifiedUtc = DateTimeOffset.Now;

            // Save the changes to the database and capture how many rows were udpated
            var numberOfChanges = await _dbContext.SaveChangesAsync();

            // numberOfChanges is stated to be equal to one because only one row is updated

            return numberOfChanges == 1;
        }

        public async Task<bool> DeleteNoteAsync(int noteID)
        {
            // Find the note by the given ID
            var noteEntity = await _dbContext.Notes.FindAsync(noteID);

            // Validate the note exists and is owned by the user
            if (noteEntity?.OwnerID != _userID)
            {
                return false;
            }
            // Remove the note from the DbContext and assert that the one change was saved
            _dbContext.Notes.Remove(noteEntity);
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}