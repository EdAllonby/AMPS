using System.Linq;
using log4net;
using Shared.Domain;
using Shared.Repository;

namespace ConsoleClient
{
    public static class Validator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Validator));

        public static void ValidateUserRepository(IReadOnlyEntityRepository<User> userRepository, int expectedUsers)
        {
            if (userRepository.GetAllEntities().Count() == expectedUsers)
            {
                Log.DebugFormat("{0} contains {1} users as expected", userRepository, expectedUsers);
            }
            else
            {
                int actualUserCount = userRepository.GetAllEntities().Count();
                Log.ErrorFormat("{0} does not contain {1} expected users. Actually contains {2} Users", userRepository, expectedUsers, actualUserCount);
            }
        }
    }
}