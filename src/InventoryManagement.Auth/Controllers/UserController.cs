using AutoMapper;
using InventoryManagement.Auth.Contracts.Request;
using InventoryManagement.Auth.Contracts.Response;
using InventoryManagement.Auth.Models;
using InventoryManagement.Auth.Services.Interfaces;
using InventoryManagement.Common.ApiRoutes;
using InventoryManagement.Common.ExceptionHandling;
using InventoryManagement.Common.Filters;
using InventoryManagement.Common.Logger;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.auth.Controllers;

[ApiController]

public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ICustomLogger _logger;

    public UserController(IMapper mapper, IUserService userService, ICustomLogger logger)
    {
        
        _mapper = mapper ;
        _userService = userService ;
        _logger = logger;
    }
    ///<summary> Register a new user. </summary>
        
    [HttpPost(Routes.RegisterUser)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseContract))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    
    public async Task<ActionResult<UserResponseContract>> registerConsumer([FromBody] UserRequestContract user){
         
         return Ok( _mapper.Map<UserResponseContract>(await _userService.Register(_mapper.Map<User>(user), user.Password, false)));
    }
    ///<summary> Login with existing userId and password to get the token. </summary>
    [HttpPost(Routes.LoginUser)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TokenContract>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Error))]
    public async Task<ActionResult<TokenContract>> Login([FromBody] LoginRequestContract user){
        return Ok(_mapper.Map<TokenContract>(await _userService.Login(user.Username, user.Password))) ;
    }
    ///<summary> To validate a token. (Used by external API to validate the token) </summary>
    [HttpGet(Routes.Validate)]
    public ActionResult validate(){
        string x = Request.Headers.Authorization!;
        string token = x.Substring(7);
        bool validate = false;
        validate =  _userService.ValidateToken(token);
        if(validate)
            return Ok();
        throw new UnauthorizedAccessException("Unauthorized");
    }
    ///<summary> Lists all the users </summary>
    
    [HttpGet(Routes.GetAllUsers)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserResponseContract>))]
    public async Task<ActionResult<List<UserResponseContract>>> GetAllUsers([FromQuery] UserFilterContract userFilter){
        var y = await  _userService.GetAllUsers(_mapper.Map<UserFilter>(userFilter));
        List<UserResponseContract>ans = new List<UserResponseContract>();
        foreach(var x in y ){
            ans.Add(_mapper.Map<UserResponseContract>(x));
        }
        return Ok(ans);
    }
    ///<summary> Get a user by its userId.</summary>
    
    ///<param name="userId"> Write the User Id of the User you want to get.</param>
    [HttpGet(Routes.GetByUserId)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseContract))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<ActionResult<List<UserResponseContract>>> GetByUserId([FromRoute] string userId){
        var y = await  _userService.GetByUserId(userId);
        var ans = _mapper.Map<UserResponseContract>(y);
        return Ok(ans);
    }

    ///<summary> Delete a user by its userId.</summary>
    ///<param name="userId"> Write the User Id of the User you want to delete.</param>
    [HttpDelete (Routes.GetByUserId)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseContract))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<ActionResult> Delete([FromRoute]string userId){
        await _userService.Delete(userId);
        return Ok();
    }

    ///<summary> Update a user's information. </summary>
    ///<param name="userId"> Write the User Id of the User you want to update.</param>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseContract))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [HttpPut(Routes.GetByUserId)]
    public async Task<ActionResult<UserResponseContract>> Update([FromBody]UpdateUserRequestContract user,[FromRoute]string userId ){
        return Ok(_mapper.Map<UserResponseContract>(await _userService.Update(_mapper.Map<User>(user), userId)));
    }
}
