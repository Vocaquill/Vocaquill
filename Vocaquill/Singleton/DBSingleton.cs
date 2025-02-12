using BLL.Models;
using BLL.Services;

namespace Vocaquill.Singleton
{
    public sealed class DBSingleton
    {
        private static readonly Lazy<DBSingleton> _instance =
            new Lazy<DBSingleton>(() => new DBSingleton());

        public static DBSingleton Instance => _instance.Value;

        public DBService DBService { get; }
        public UserDTO? CurrentUser { get; set; }

        private DBSingleton()
        {
            DBService = new DBService();
        }
    }
}
