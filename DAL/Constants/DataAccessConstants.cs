using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Constants
{
    public class DataAccessConstants
    {
        public readonly static string ApiAudioToTextUrl = "http://35.159.119.219:5000/api/audio/upload";
        public readonly static string ApiGemimiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=";
        public readonly static string ApiGemimiKey = "AIzaSyDYcdRiM7LAUcg2Ql_00boBZff2Dfmldgo";
        //public readonly static string ConnectionString = "Host=ep-orange-art-a2mumvqw-pooler.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_zlQnApPi62Ry";
        public readonly static string ConnectionString = "Host=ep-autumn-union-a2dgzlyl-pooler.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_25QGlXLygkKh";
    }
}
