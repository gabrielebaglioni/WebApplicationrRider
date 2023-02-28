using System.Collections;
using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Authorization;
using WebApplicationrRider.Controllers.Support;
using WebApplicationrRider.Domain.Comunication.OperationResults;
using WebApplicationrRider.Domain.Models;
using WebApplicationrRider.Domain.Models.DTOs.Incoming;
using WebApplicationrRider.Domain.Services;

namespace WebApplicationrRider.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable>> GetAll()
    {
        var users = await _userService.GetListAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.Get(id);
        return Ok(user);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserSaveDto userSaveDto)

    {
        var user = await _userService.CreateAsync(userSaveDto);
        return Ok(UserSavingOperationResult.Okay(user, "User added successfully"));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserSaveDto userSaveDto)
    {
        var user = await _userService.UpdateAsync(id, userSaveDto);
        return Ok(UserSavingOperationResult.Okay(user, "User updated successfully"));
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.DeleteAsync(id);
        return Ok(UserSavingOperationResult.Okay(user, "User deleted successfully"));
    }
}