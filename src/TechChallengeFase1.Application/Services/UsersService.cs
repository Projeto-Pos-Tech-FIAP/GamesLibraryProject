using AutoMapper;
using TechChallengeFase1.Application.DTOs.UsersDto;
using TechChallengeFase1.Application.Interfaces;
using TechChallengeFase1.Domain.DTOs.AuthDto;
using TechChallengeFase1.Domain.Exceptions;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Application.Services
{
    public class UsersService : IUsersServices
    {

        private readonly IKeycloakService _keycloakService;
        private readonly IMapper _mapper;

        public UsersService(IKeycloakService keycloakService, IMapper mapper)
        {
            _keycloakService = keycloakService ?? throw new ArgumentNullException(nameof(keycloakService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<List<KeycloakUserResponseDto>> GetAllUsersAsync() 
        {
            return await _keycloakService.GetAllUsersAsync();  
        }
        public async Task<KeycloakUserResponseDto> GetByUsernameOrEmailAsync(string? username, string? email) 
        {
            var users = await _keycloakService.GetUsersAsync(email: email, username: username);
            if (users == null || !users.Any())
                throw new ExternalServiceException($"User with email {email} or username {username} does not exist.");
            return users.FirstOrDefault();
        }

        public async Task CreateUserAsync(CreateUserInputDto command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var userExists = await _keycloakService.GetUsersAsync(command.Username, command.Email);
            if (userExists.Any())
                throw new ExternalServiceException($"User with email {command.Email} already exists.");

            var usermapper = _mapper.Map<CreateUserInputDto, CreateUserModel>(command);

            var userId = await _keycloakService.CreateUserAsync(usermapper);

            var guidGroup = await _keycloakService.GetAllGroupsByToken();

            var groupExists = 
                guidGroup.FirstOrDefault(g => g.Name.Equals(command.Level.ToString(), 
                StringComparison.OrdinalIgnoreCase));
            
            if(groupExists == null)
                throw new ExternalServiceException($"Group with name {command.Level} does not exist.");

            await _keycloakService.AssignUserToGroupAsync( userId, groupExists.Id);
        }
        public async Task SetUserEnabledAsync(string userEmail, bool enable)
        {
            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentException("User email cannot be null or empty.", nameof(userEmail));

           var userExists = await _keycloakService.GetUsersAsync(email :userEmail, username: null);
            if (userExists == null || !userExists.Any())
                throw new ExternalServiceException($"User with email {userEmail} does not exist.");

            await _keycloakService.SetUserEnabledAsync(userExists.First().Id, enable);
        }
        public async Task UpdateUserAsync(EditUserDto command, string userEmail)
        {

           var userExists = await _keycloakService.GetUsersAsync(email : userEmail, username: null);
            if (userExists == null || !userExists.Any())
                throw new ExternalServiceException($"User with email {userEmail} does not exist.");

            var user = userExists.First();
            if (command.FirstName != null) user.FirstName = command.FirstName;
            if (command.LastName != null) user.LastName = command.LastName;
            if (command.Gender != null) user.Attributes["Gender"] = new List<string> { command.Gender.ToString() };
            if (command.DateOfBirth != null) user.Attributes["DateOfBirth"] = new List<string> { command.DateOfBirth.Value.ToString("yyyy-MM-dd") };

            var userUpdateModel = _mapper.Map<CreateUserModel>(user);
            await _keycloakService.UpdateUserAsync(user.Id, userUpdateModel);
        }
    }
}
