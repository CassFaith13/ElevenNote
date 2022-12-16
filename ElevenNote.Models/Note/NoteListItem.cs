namespace ElevenNote.Models.Note
{
    public class NoteListItem
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
    }
}