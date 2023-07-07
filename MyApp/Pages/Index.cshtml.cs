using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IConfiguration configuration;
        public String userName;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration _configuration)
        {
            _logger = logger;
            configuration = _configuration;
        }

        public void OnGet()
        {
            userName = GetUserName(configuration.GetConnectionString("OracleConnection"));
        }

        private String GetUserName(String connectionStr)
        {
            String result;

            OracleConnection conn = new OracleConnection(connectionStr);
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select FIRST_NAME||' '||LAST_NAME from SYSTEM_USER where ID = 1";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();

            result = dr.GetString(0);
            conn.Dispose();

            return result;
        }
    }
}
