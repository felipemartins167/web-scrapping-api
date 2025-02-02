namespace FmsWebScrapingApi.Infra.Constants
{
    public static class ErrorMessageConstants
    {
        public static string GenericError = "Ocorreu um erro inesperado na API!";

        // Auth Errors
        public static string InsertTokenUser = "Ocorreu um erro ao salvar token de acesso do usuário!";

        // Token Erros
        public static string SecurityInvalidToken = "O Token de autenticação enviado não é válido!";

        // Admin Erros
        public static string CreateAdmin = "Erro ao realizar criação de usuário administrador!";
        public static string GetAdminById = "Erro ao recuperar informações de usuário administrador!";
        public static string ValidationAdministrator = "Erro ao realizar validação dos dados do administrador!";

        // Email Errors
        public static string SendEmail = "Erro ao realizar envio de e-mail para o usuário!";
        public static string SendEmailPassword = "Erro ao realizar envio de e-mail de recuperação de senha para o usuário!";
        public static string InvalidEmailTemplate = "Não foi possível encontrar o template de e-mail requisitado!";
        public static string InvalidEmail = "E-mail inválido!";

        // Mysql Errors
        public static string DatabaseConnectionError = "Houve um erro ao se conectar com o banco de dados!";
        public static string DatabaseDisposeConnectionError = "Houve um erro ao fechar conexão com o banco de dados!";
        public static string MysqlConsult = "Ocorreu um erro ao realizar consulta no banco de dados!";
        public static string MysqlInsert = "Ocorreu um erro ao realizar inserção no banco de dados!";
        public static string MysqlUpdate = "Ocorreu um erro ao realizar atualização no banco de dados!";
        public static string MysqlDelete = "Ocorreu um erro ao realizar deleção no banco de dados!";
        public static string MysqlSql = "Ocorreu um erro ao executar sentença no banco de dados!";

        // Create User
        public static string InvalidBirthDate = "Data de nascimento é inválida!";
        public static string UserFoundError = "Usuário já registrado em nossa base de dados!";
        public static string UserNotFoundError = "Usuário ou senha incorretos!";
        public static string GetUserByIdError = "Ocorreu um erro ao recuperar os dados do usuário!";
        public static string CreateUser = "Ocorreu um erro ao criar usuário!";

        // Settings
        public static string SettingsNotFound = "Informações de configurações padrão não encontradas";
    }
}
