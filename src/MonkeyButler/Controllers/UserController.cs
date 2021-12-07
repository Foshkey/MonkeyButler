using System.Net;
using Microsoft.AspNetCore.Mvc;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models.LinkCharacter;
using MonkeyButler.Abstractions.Business.Models.User;
using MonkeyButler.Abstractions.Business.Models.VerifyCharacter;

namespace MonkeyButler.Controllers;

/// <summary>
/// Controller for exporting data from the data storage.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILinkCharacterManager _linkCharacterManager;
    private readonly IUserManager _userManager;
    private readonly IVerifyCharacterManager _verifyCharacterManager;

    /// <summary>
    /// Constructor
    /// </summary>
    public UserController(ILinkCharacterManager linkCharacterManager, IUserManager userManager, IVerifyCharacterManager verifyCharacterManager)
    {
        _linkCharacterManager = linkCharacterManager ?? throw new ArgumentNullException(nameof(linkCharacterManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _verifyCharacterManager = verifyCharacterManager ?? throw new ArgumentNullException(nameof(verifyCharacterManager));
    }

    /// <summary>
    /// Gets the user of the <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    [HttpGet("{id}")]
    [ProducesDefaultResponseType(typeof(User))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public Task<User?> Get(ulong id) => _userManager.GetUser(id);

    /// <summary>
    /// Adds or updates the given user.
    /// </summary>
    [HttpPut()]
    [ProducesDefaultResponseType(typeof(User))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public Task<User> Put([FromBody] User user) => _userManager.AddOrUpdateUser(user);

    /// <summary>
    /// Adds a collection of users with character Ids.
    /// </summary>
    [HttpPut("Collection")]
    [ProducesDefaultResponseType()]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public Task PutCollection([FromBody] IDictionary<ulong, IEnumerable<long>> collection) => _userManager.AddCharacterIds(collection);

    /// <summary>
    /// Links a character to the given user Id (Without verification).
    /// </summary>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpPost("Link")]
    [ProducesDefaultResponseType(typeof(LinkCharacterResult))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<LinkCharacterResult> LinkCharacter([FromBody] LinkCharacterCriteria criteria)
    {
        var result = await _linkCharacterManager.Process(criteria);

        if (!result.Success)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        return result;
    }

    /// <summary>
    /// Verifies a character exists within the guild's FC and links it to the user if so.
    /// </summary>
    /// <param name="criteria"></param>
    /// <returns></returns>
    [HttpPost("Verify")]
    [ProducesDefaultResponseType(typeof(VerifyCharacterResult))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<VerifyCharacterResult> VerifyCharacter([FromBody] VerifyCharacterCriteria criteria)
    {
        var result = await _verifyCharacterManager.Process(criteria);

        if (result.Status != Status.Verified)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        return result;
    }
}
