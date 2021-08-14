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

                return _userAccessor.SaveCharacterToUser(new()
                {
                    UserId = userId,
                    CharacterId = characterId
                });
            }));
        }

        public async Task<User> AddOrUpdateUser(User user)
        {
            _validator.ValidateAndThrow(user);

            var userId = user.Id;

            await Task.WhenAll(user.CharacterIds.Select(characterId =>
            {
                _logger.SavingCharacter(userId, characterId);

                return _userAccessor.SaveCharacterToUser(new()
                {
                    UserId = userId,
                    CharacterId = characterId
                });
            }));

            var updatedUser = await _userAccessor.GetUser(new()
            {
                UserId = userId
            });

            return new()
            {
                Id = userId,
                CharacterIds = updatedUser?.CharacterIds ?? new()
            };
        }

        public async Task<User?> GetUser(ulong id)
        {
            var user = await _userAccessor.GetUser(new()
            {
                UserId = id
            });

            return user is null ? null : new()
            {
                Id = user.Id,
                CharacterIds = user.CharacterIds
            };
        }
    }
}
