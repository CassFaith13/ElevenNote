using ElevenNote.Models.Note;

namespace ElevenNote.Services.Note
{
    public interface INoteService
    {
        Task<bool> CreateNoteAsync(NoteCreate request);
        Task<IEnumerable<NoteListItem>> GetAllNotesAsync();
        Task<NoteDetail> GetNoteByIDAsync(int noteID);
        Task<bool> UpdateNoteAsync(NoteUpdate request);
    }
}