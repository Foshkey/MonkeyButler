using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MonkeyButler.Abstractions.Business;
using MonkeyButler.Abstractions.Business.Models;
using MonkeyButler.Abstractions.Data.Storage;

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

        public async Task AddCharacterIds(IDictionary<ulong, long> usersWithCharacters)
        {
            await Task.WhenAll(usersWithCharacters.Select(kvp =>
            {
                var userId = kvp.Key;
                var characterId = kvp.Value;

                if (userId == 0 || characterId <= 0)
                {
                    return Task.CompletedTask;
                }

                _logger.SavingCharacter(userId, characterId);

                return _userAccessor.SaveUser(new()
                {
                    Id = userId,
                    CharacterIds = new() { characterId }
                });
            }));
        }

        public async Task<User> AddOrUpdateUser(User user)
        {
            _validator.ValidateAndThrow(user);

            _logger.LogDebug("Saving user '{UserId}'.", user.Id);

            var updatedUser = await _userAccessor.SaveUser(new()
            {
                Id = user.Id,
                CharacterIds = user.CharacterIds
                    .Distinct()
                    .ToHashSet()
            });

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
