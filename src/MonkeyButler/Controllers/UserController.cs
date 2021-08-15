using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models;

namespace MonkeyButler.Controllers
{
    /// <summary>
    /// Controller for exporting data from the data storage.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserController(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
    }
}
