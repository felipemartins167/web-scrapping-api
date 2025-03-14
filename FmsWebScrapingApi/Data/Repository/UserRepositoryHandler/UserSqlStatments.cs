

namespace FmsWebScrapingApi.Data.Repository.UserRepositoryHandler
{
    public static class UserSqlStatments
    {
        public static string CreateUser()
        {
            return $"INSERT INTO users (name, email, password, create_at, update_at, status_id, email_confirmed, hash_email_confirm) VALUES(@name, @email, @password, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, 0, @hashEmail) ";
        }

        public static string CreateUserRole()
        {
            return $"INSERT INTO roles_users (user_id, role_id) VALUES(@userId, @roleId) ";
        }

        public static string GetUserByLoginRole()
        {
            return $"select usr.id, usr.name, usr.email, usr.create_at, usr.update_at, usr.status_id, stu.name as 'name_status' from users usr inner join status_user stu on stu.id = usr.status_id where usr.email = @email and usr.password = @password and (select user_id from roles_users where roles_users.user_id = usr.id and roles_users.role_id = @role) is not null";
        }

        public static string GetUserById()
        {
            return $"select usr.id, usr.name, usr.email, usr.create_at, usr.update_at, usr.status_id, stu.name as 'name_status' from users usr inner join status_user stu on stu.id = usr.status_id where usr.id = @id ";
        }

        public static string GetUserByEmail()
        {
            return $"select usr.id, usr.name, usr.email, usr.create_at, usr.update_at, usr.status_id, stu.name as 'name_status' from users usr inner join status_user stu on stu.id = usr.status_id where usr.email = @email ";
        }

        public static string CreateRefreshTokenUser()
        {
            return $"insert into tokens (token, refresh_token, expiration, user_id) values (@token, @refreshToken, @expiration, @userId)";
        }

        public static string UpdatePasswordUserById()
        {
            return $"update users set password = @password where id = @userId ";
        }

        public static string ValidateUserEmailByToken()
        {
            return $"update users set email_confirmed = @email_confirmed where hash_email_confirm = @hash_email_confirm ";
        }

        public static string UpdateRefreshTokenUser()
        {
            return $"update tokens set token = @token, refresh_token = @refreshToken, expiration = @expiration, user_id = @userId where refresh_token = @oldRefreshToken";
        }

        public static string CreateHashTokenPassword()
        {
            return $"INSERT INTO hash_tokens_password (token, expiration, user_id) VALUES(@token, @expiration, @userId) ";
        }

        public static string GetHashTokenPasswordByToken()
        {
            return $"select ht.token, ht.expiration, ht.user_id from hash_tokens_password ht inner join users us on us.id = ht.user_id where ht.token = @token and us.email = @email";
        }

        public static string DeleteHashTokenByToken()
        {
            return $"delete from hash_tokens_password where token = @token";
        }

        public static string GetUserByRefreshToken()
        {
            return $"select usr.id, usr.name, usr.email, usr.create_at, usr.update_at, usr.status_id, stu.name as 'name_status' from users usr inner join status_user stu on stu.id = usr.status_id inner join tokens tok on tok.user_id = usr.id where tok.refresh_token = @refreshToken ";
        }

        public static string GetUsers()
        {
            return $"select usr.id, usr.name, usr.email, usr.create_at, usr.update_at, usr.status_id, stu.name as 'name_status' from users usr inner join status_user stu on stu.id = usr.status_id ";
        }

        public static string GetTotalUsers()
        {
            return $"select count(*) from users usr inner join status_user stu on stu.id = usr.status_id ";
        }

        public static string SearchUser()
        {
            return $" where usr.name like @search or usr.email like @search ";
        }

        public static string PaginationUser()
        {
            return $" LIMIT @limit OFFSET @offset";
        }

        public static string GetUsersRoles()
        {
            return $"select rolu.user_id, rolu.role_id, rol.name as 'name_role' from roles_users rolu inner join roles rol on rolu.role_id = rol.id inner join users usr on usr.id = rolu.user_id";
        }

        public static string VerifyEmailExist()
        {
            return $"select count(*) from users usr where usr.email = @email";
        }
    }
}
