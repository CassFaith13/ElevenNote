using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ElevenNote.Services.Note
{
    public class NoteService : INoteService
    {
        private readonly int _userID;
        public NoteService(IHttpContextAccessor httpContextAccessor)
        {
            var userClaims = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            var value = userClaims.FindFirst("ID")?.Value;

            var validID = int.TryParse(value, out _userID);

            if (!validID)
            {
                throw new Exception("Attempted to build NoteService without User ID claim.");
            }
            
        }
    }
}