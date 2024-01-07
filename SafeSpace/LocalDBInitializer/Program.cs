using System;
using MySql.Data.MySqlClient;

namespace LocalDBInitializer
{
    class Program
    {
        static void Main()
        {
            Console.Write("Enter your MySQL root password: ");
            string rootPassword = Console.ReadLine();

            string connectionString = $"Server=localhost;Port=3306;Uid=root;Pwd={rootPassword};";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string checkUserSql = "SELECT COUNT(*) FROM mysql.user WHERE user = 'safespace' AND host = '%';";
                    MySqlCommand checkUserCommand = new MySqlCommand(checkUserSql, connection);
                    int userExists = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                    if (userExists > 0)
                    {
                        string revokePrivilegesSql = "REVOKE ALL PRIVILEGES, GRANT OPTION FROM 'safespace'@'%';";
                        MySqlCommand revokePrivilegesCommand = new MySqlCommand(revokePrivilegesSql, connection);
                        revokePrivilegesCommand.ExecuteNonQuery();

                        string dropUserSql = "DROP USER 'safespace'@'%';";
                        MySqlCommand dropUserCommand = new MySqlCommand(dropUserSql, connection);
                        dropUserCommand.ExecuteNonQuery();
                    }

                    string createUserSql = "CREATE USER 'safespace'@'%' IDENTIFIED BY 'local-db-password';";
                    MySqlCommand createUserCommand = new MySqlCommand(createUserSql, connection);
                    createUserCommand.ExecuteNonQuery();

                    string createDbSql = "CREATE DATABASE IF NOT EXISTS safespace_local_db;";
                    MySqlCommand createDbCommand = new MySqlCommand(createDbSql, connection);
                    createDbCommand.ExecuteNonQuery();

                    string grantPrivilegesSql = "GRANT ALL PRIVILEGES ON safespace_local_db.* TO 'safespace'@'%';";
                    MySqlCommand grantPrivilegesCommand = new MySqlCommand(grantPrivilegesSql, connection);
                    grantPrivilegesCommand.ExecuteNonQuery();

                    string flushPrivilegesSql = "FLUSH PRIVILEGES;";
                    MySqlCommand flushPrivilegesCommand = new MySqlCommand(flushPrivilegesSql, connection);
                    flushPrivilegesCommand.ExecuteNonQuery();

                    Console.WriteLine("User 'safespace' has been granted privileges on database 'safespace_local_db'.");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Setup complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
