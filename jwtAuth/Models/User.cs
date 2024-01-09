namespace jwtAuth.Models;

public record User(
    int Id,
    string Username,
    string Name,
    string Email,
    string Password,
    string[] Roles)
{
}