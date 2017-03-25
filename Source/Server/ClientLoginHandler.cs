using System.Net.Sockets;
using log4net;
using Shared;
using Shared.Domain;
using Shared.Message;
using Shared.Message.LoginMessage;
using Shared.Repository;
using Shared.Serialiser;
using Shared.Serialiser.MessageSerialiser;

namespace Server
{
    /// <summary>
    /// Handles a client attempting to log in to the server.
    /// </summary>
    internal sealed class ClientLoginHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientLoginHandler));

        /// <summary>
        /// From a <see cref="TcpClient" /> object, attempt to initialise a new <see cref="User" /> <see cref="Entity" />.
        /// </summary>
        /// <param name="tcpClient">The connection between the attempting-to-connect client.</param>
        /// <param name="serviceRegistry">Holds services to initialise client</param>
        /// <returns></returns>
        public static LoginResponse InitialiseNewClient(TcpClient tcpClient, IServiceRegistry serviceRegistry)
        {
            var repositoryManager = serviceRegistry.GetService<RepositoryManager>();
            var userRepository = (UserRepository) repositoryManager.GetRepository<User>();

            var entityIdAllocatorFactory = serviceRegistry.GetService<EntityIdAllocatorFactory>();

            LoginRequest loginRequest = GetLoginRequest(tcpClient);
            User user = userRepository.FindUserByUsername(loginRequest.Username);

            LoginResponse loginResponse;

            var passwordHandler = serviceRegistry.GetService<PasswordHandler>();

            if (IsNewUser(user))
            {
                loginResponse = HandleNewUser(loginRequest, userRepository, entityIdAllocatorFactory, passwordHandler);
                SendLoginResponse(loginResponse, tcpClient);
            }
            else if (IsExistingUser(user))
            {
                loginResponse = HandleExistingUser(userRepository, user, loginRequest, passwordHandler);
                SendLoginResponse(loginResponse, tcpClient);
            }
            else
            {
                Log.InfoFormat("User with user Id {0} already connected, denying user login.", user.Id);
                loginResponse = new LoginResponse(null, LoginResult.AlreadyConnected);
                SendLoginResponse(loginResponse, tcpClient);
            }

            return loginResponse;
        }

        private static LoginResponse HandleNewUser(LoginRequest loginRequest, UserRepository userRepository, EntityIdAllocatorFactory entityIdAllocatorFactory, PasswordHandler passwordHandler)
        {
            User user = CreateUserEntity(loginRequest, userRepository, entityIdAllocatorFactory);

            var userLoginResult = LoginResult.UnknownError;

            if (passwordHandler.StorePassword(user.Id, loginRequest.Password))
            {
                userLoginResult = LoginResult.Success;
            }

            return new LoginResponse(user, userLoginResult);
        }

        private static LoginResponse HandleExistingUser(UserRepository userRepository, User user, LoginRequest loginRequest, PasswordHandler passwordHandler)
        {
            bool doesPasswordMatch = passwordHandler.IsPasswordValid(user.Id, loginRequest.Password);

            var userLoginResult = LoginResult.Success;

            if (!doesPasswordMatch)
            {
                userLoginResult = LoginResult.IncorrectPassword;
            }
            else
            {
                userRepository.UpdateUserConnectionStatus(new ConnectionStatus(user.Id, ConnectionStatus.Status.Connected));
            }

            var loginResponse = new LoginResponse(user, userLoginResult);

            return loginResponse;
        }

        private static LoginRequest GetLoginRequest(TcpClient tcpClient)
        {
            MessageIdentifier messageIdentifier = MessageIdentifierSerialiser.DeserialiseMessageIdentifier(tcpClient.GetStream());

            IMessageSerialiser messageSerialiser = SerialiserFactory.GetSerialiser(messageIdentifier);

            var loginRequest = (LoginRequest) messageSerialiser.Deserialise(tcpClient.GetStream());

            return loginRequest;
        }

        private static User CreateUserEntity(LoginRequest clientLogin, IEntityRepository<User> userRepository, EntityIdAllocatorFactory entityIdAllocator)
        {
            int newUserId = entityIdAllocator.AllocateEntityId<User>();
            var newUser = new User(newUserId, clientLogin.Username, new ConnectionStatus(newUserId, ConnectionStatus.Status.Connected));

            userRepository.AddEntity(newUser);

            return newUser;
        }

        private static void SendLoginResponse(IMessage loginResponse, TcpClient tcpClient)
        {
            IMessageSerialiser messageSerialiser = SerialiserFactory.GetSerialiser(loginResponse.MessageIdentifier);
            messageSerialiser.Serialise(tcpClient.GetStream(), loginResponse);
        }

        private static bool IsNewUser(User user)
        {
            return user == null;
        }

        private static bool IsExistingUser(User user)
        {
            return user.ConnectionStatus.UserConnectionStatus != ConnectionStatus.Status.Connected;
        }
    }
}