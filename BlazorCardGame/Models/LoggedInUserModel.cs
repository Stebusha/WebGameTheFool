using System.Security.Claims;

namespace BlazorCardGame.Models;

public record class LoggedInUserModel(int Id, string Name){
    public Claim[] ToClaims() =>
        [
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim(ClaimTypes.Name, Name)
        ];
}