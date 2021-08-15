using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models;
using MonkeyButler.Abstractions.Data.Storage;
using MonkeyButler.Business.Engines;

namespace MonkeyButler.Business.Managers
{
    internal class UserManager : IUserManager
    {
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<UserManager> _logger;
        private readonly IValidator<User> _validator;

        public UserManager(
            IUserAccessor userAccessor,
            ILogger<UserManager> logger,
            IValidator<User> validator)
        {
            _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task AddCharacterIds(IDictionary<ulong, IEnumerable<long>> usersWithCharacters)
        {
            await Task.WhenAll(usersWithCharacters.Select(async kvp =>
            {
                var userId = kvp.Key;
                var characterIds = kvp.Value;

                if (userId == 0 || characterIds is null || !characterIds.Any())
                {
                    return Task.CompletedTask;
                }

                _logger.LogDebug("Saving user '{UserId}'.", userId);

                var storedUser = await _userAccessor.GetUser(userId) ?? new() { Id = userId };
                var mergedUser = storedUser.Merge(characterIds);

                return _userAccessor.SaveUser(mergedUser);
            }));
        }

        public async Task<User> AddOrUpdateUser(User user)
        {
            _validator.ValidateAndThrow(user);

            _logger.LogDebug("Saving user '{UserId}'.", user.Id);

            var storedUser = await _userAccessor.GetUser(user.Id) ?? new() { Id = user.Id };
            var mergedUser = storedUser.Merge(user.CharacterIds);

            var updatedUser = await _userAccessor.SaveUser(mergedUser);

            return new()
            {
                Id = updatedUser.Id,
                CharacterIds = updatedUser.CharacterIds
            };
        }

        public async Task<User?> GetUser(ulong id)
        {
            _logger.LogDebug("Getting user '{UserId}'.", id);

            var user = await _userAccessor.GetUser(id);

            return user is null ? null : new()
            {
                Id = user.Id,
                CharacterIds = user.CharacterIds
            };
        }
    }
}
