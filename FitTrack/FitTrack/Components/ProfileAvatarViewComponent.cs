using System.Security.Claims;
using FitTrack.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Components;

public sealed class ProfileAvatarViewComponent(AppDbContext db) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var photoPath = string.IsNullOrWhiteSpace(userId)
            ? null
            : await db.UserProfiles.AsNoTracking()
                .Where(profile => profile.IdentityUserId == userId)
                .Select(profile => profile.ProfilePhotoPath)
                .FirstOrDefaultAsync(HttpContext.RequestAborted);

        return View("Default", photoPath);
    }
}
