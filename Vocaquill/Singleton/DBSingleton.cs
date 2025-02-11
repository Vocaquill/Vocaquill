using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocaquill.Singleton
{
    public sealed class DBSingleton
    {
        private static readonly Lazy<DBSingleton> _instance =
            new Lazy<DBSingleton>(() => new DBSingleton());

        public static DBSingleton Instance => _instance.Value;

        public DBService DBService { get; }

        private DBSingleton()
        {
            DBService = new DBService();
        }
    }
}
