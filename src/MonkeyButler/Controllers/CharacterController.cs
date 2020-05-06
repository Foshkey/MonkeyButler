using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonkeyButler.Business.Managers;
using MonkeyButler.Extensions;
using MonkeyButler.Models.Character;

namespace MonkeyButler.Controllers
{
    /// <summary>
    /// Controller for accessing character information.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/{controller}")]
    [ApiVersion("1")]
    [ProducesResponseType(500)]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterSearchManager _characterSearchManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="characterSearchManager"></param>
        public CharacterController(ICharacterSearchManager characterSearchManager)
        {
            _characterSearchManager = characterSearchManager ?? throw new ArgumentNullException(nameof(characterSearchManager));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(CharacterSearchResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CharacterSearchResponse>> Search([FromQuery] CharacterSearchRequest request)
        {
            var criteria = new Business.Models.CharacterSearch.CharacterSearchCriteria()
            {
                Query = request.Query
            };

            var result = await _characterSearchManager.Process(criteria);

            var response = await result.ToDisplayModel();

            return Ok(response);
        }
    }
}
