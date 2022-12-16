using Microsoft.AspNetCore.Authorization;
using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Mvc;
using ElevenNote.Models.Note;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _noteService.CreateNoteAsync(request))
            {
                return Ok("Note created successfully.");
            }
            return BadRequest("Note NOT created.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpGet, Route("{noteID:int}")]
        public async Task<IActionResult> GetNoteByID(int noteID)
        {
            var detail = await _noteService.GetNoteByIDAsync(noteID);

            // Similar to our service method, we're using a ternary to determine our return type
            // If they returned value (detail) is not null, return it with a 200 OK
            // Otherwise return a NotFound() 404 response
            return detail is not null
            ? Ok(detail)
            : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNoteByID([FromBody] NoteUpdate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _noteService.UpdateNoteAsync(request)
            ? Ok("Note successfully updated.")
            : BadRequest("Note NOT updated.")
        }
    }
}