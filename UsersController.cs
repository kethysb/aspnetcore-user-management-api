using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, User> _users = new(new[]
    {
        new KeyValuePair<int, User>(1, new User { Id = 1, FullName = "Alice Johnson", Email = "alice@techhive.com", Department = "HR", Role = "Admin" }),
        new KeyValuePair<int, User>(2, new User { Id = 2, FullName = "Carlos Silva", Email = "carlos@techhive.com", Department = "IT", Role = "Developer" })
    });
    private static int _nextId = 3;

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll([FromQuery] string? department = null)
    {
        var list = _users.Values.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(department))
        {
            list = list.Where(u => u.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
        }
        return Ok(list.OrderBy(u => u.Id));
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetById(int id)
    {
        if (!_users.TryGetValue(id, out var user))
        {
            return NotFound(new { error = $"User with ID {id} was not found." });
        }
        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> Create([FromBody] User newUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_users.Values.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new { error = "A user with this email already exists." });
        }

        var id = Interlocked.Increment(ref _nextId);
        newUser.Id = id;
        newUser.CreatedAt = DateTime.UtcNow;
        _users[id] = newUser;

        return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] User updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!_users.TryGetValue(id, out var existing))
        {
            return NotFound(new { error = $"User with ID {id} was not found." });
        }

        existing.FullName = updatedUser.FullName;
        existing.Email = updatedUser.Email;
        existing.Department = updatedUser.Department;
        existing.Role = updatedUser.Role;
        existing.IsActive = updatedUser.IsActive;

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_users.TryRemove(id, out _))
        {
            return NotFound(new { error = $"User with ID {id} was not found." });
        }
        return NoContent();
    }
}
