namespace FmsWebScrapingApi.Infra.Constants
{
    public static class ErrorCodeConstants
    {
        public static string GenericError = "GEN001";

        // Auth Errors
        public static string InsertTokenUser = "AUT001";

        // Token Erros
        public static string SecurityInvalidToken = "TKN001";

        // Admin Erros
        public static string CreateAdmin = "ADM001";
        public static string GetAdminById = "ADM002";
        public static string ValidationAdministrator = "ADM003";

        // Email Erros
        public static string SendEmail = "EMA001";
        public static string SendEmailPassword = "EMA002";
        public static string InvalidEmailTemplate = "EMA003";
        public static string InvalidEmail = "EMA004";

        // Mysql Error Codes
        public static string MysqlConnectionCode = "MSC001";
        public static string MysqlDisposeConnectionCode = "MSC002";
        public static string MysqlConsult = "MSC002";
        public static string MysqlInsert = "MSC003";
        public static string MysqlUpdate = "MSC004";
        public static string MysqlDelete = "MSC005";
        public static string MysqlSql = "MSC006";

        // Auth User
        public static string InvalidBirthDate = "AUU001";
        public static string UserFoundCode = "AUU002";
        public static string UserNotFoundCode = "AUU003";
        public static string GetUserByIdCode = "AUU004";
        public static string CreateUser = "AUU005";

        // Settings
        public static string SettingsNotFound = "SET001";
    }
}
