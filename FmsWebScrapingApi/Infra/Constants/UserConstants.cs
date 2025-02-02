namespace FmsWebScrapingApi.Infra.Constants
{
    public class UserConstants
    {
        public const bool PASSWORD_SHOULD_HAS_LOWER = true;
        public const bool PASSWORD_SHOULD_HAS_UPPER = true;
        public const bool PASSWORD_SHOULD_HAS_SYMBOLS = true;
        public const bool PASSWORD_SHOULD_HAS_NUMBERS = true;
        public const int PASSWORD_MINSIZE = 8;
        public const int PASSWORD_MAXSIZE = 20;

        public static int ID_ROLE_ADMIN = 1;
        public static int ID_ROLE_TESTER = 2;
        public static int ID_ROLE_APP_USER = 3;
    }
}
