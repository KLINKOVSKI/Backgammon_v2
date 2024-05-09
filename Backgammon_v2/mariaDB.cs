using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;




namespace Backgammon_v2
{
    public class mariaDB
    {
        private static MySqlConnection Connection { get; set; }

        private string ConnectionString = "server = localhost; user=root;port=3307;password=zayzay;database=Tawle;";

        public void Connect()
        {
            Connection = new MySqlConnection("server = localhost; user=root;port=3307;password=zayzay;database=Tawle;");
            Connection.Open();

        }
        public void createDB()
        {
            string connStr = "server=localhost;user=root;port=3307;password=zayzay;";
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS Tawle;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "USE Tawle;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Leaderboard (id INT AUTO_INCREMENT PRIMARY KEY, username VARCHAR(255), wins int, losses int);";
                cmd.ExecuteNonQuery();

                Console.WriteLine("Database and table created successfully.");
            }

        }

        private bool Exists(string name)
        {
            string query = "select count(*) from Leaderboard where Username=@Username";
            MySqlCommand command = new MySqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            int count = int.Parse(command.ExecuteScalar().ToString());
            return count > 0;
        }

        public void CreateUser(string username, string password)
        {
            string query = "INSERT INTO Leaderboard (Username, Password, Wins, Losses) VALUES (@Username, @Password, 0, 0)";
            MySqlCommand command = new MySqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.ExecuteNonQuery();
        }


        public void UpdateUserWins(string name)
        {
            if (Exists(name)) return;
            string query = "update Leaderboard set Wins = Wins + 1 where Username = @Username";
            MySqlCommand command = new MySqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            command.ExecuteNonQuery();
        }
        public void UpdateUserLoses(string name)
        {
            if (Exists(name)) return;
            string query = "update Leaderboard set Loses = Loses + 1 where Username = @Username";
            MySqlCommand command = new MySqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", name);
            command.ExecuteNonQuery();
        }

        public static List<UserData> GetLeaderboard()
        {
            string query = "select Username,Wins,Loses from Leaderboard";
            MySqlCommand command = new MySqlCommand(query, Connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable table = ds.Tables[0];
            List<UserData> data = new List<UserData>();
            foreach (DataRow row in table.Rows)
            {
                data.Add(new UserData
                {
                    Name = row.ItemArray[0].ToString(),
                    Wins = int.Parse(row.ItemArray[1].ToString()),
                    Loses = int.Parse(row.ItemArray[2].ToString())
                });
            }

            return data;
        }
        public bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Leaderboard WHERE Username = @Username";
            MySqlCommand command = new MySqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", username);
            int count = int.Parse(command.ExecuteScalar().ToString());

            if (count == 0)
            {
                // Username doesn't exist, register the user
                CreateUser(username, password);
                return true; // Return false indicating the login failed
            }

            // Username exists, check if the password is correct
            query = "SELECT COUNT(*) FROM Leaderboard WHERE Username = @Username AND Password = @Password";
            command = new MySqlCommand(query, Connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            count = int.Parse(command.ExecuteScalar().ToString());

            if (count > 0)
            {
                // Password is correct, return true indicating successful login
                return true;
            }
            else
            {
                // Password is incorrect, return false indicating failed login
                return false;
            }
        }



        public void Close()
        {
            Connection.Close();
        }
    }


    //public void testConnection()
    //{
    //    string myConnectionString;
    //    myConnectionString = "server=localhost;uid=root;pwd=zayzay;port=3307";
    //    try
    //    {
    //        MySqlConnection conn = new MySqlConnection(myConnectionString);
    //        conn.Open();
    //        var stm = "SELECT VERSION()";
    //        var cmd = new MySqlCommand(stm, conn);

    //        var version = cmd.ExecuteScalar().ToString();
    //        Console.WriteLine($"MariaDB version: {version}");
    //    }
    //    catch (Exception ex) { }

    //}

    //public void createDB()
    //{
    //    string connStr = "server=localhost;user=root;port=3307;password=zayzay;";
    //    using (var conn = new MySqlConnection(connStr))
    //    using (var cmd = conn.CreateCommand())
    //    {
    //        conn.Open();

    //        cmd.CommandText = "CREATE DATABASE IF NOT EXISTS Tawle;";
    //        cmd.ExecuteNonQuery();

    //        cmd.CommandText = "USE Tawle;";
    //        cmd.ExecuteNonQuery();

    //        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Leaderboard (id INT AUTO_INCREMENT PRIMARY KEY, username VARCHAR(255), wins int, losses int);";
    //        cmd.ExecuteNonQuery();

    //        Console.WriteLine("Database and table created successfully.");
    //    }
    //}


    //public void tryCreateAlterTable()
    //{
    //    string connStr = "server=localhost;user=root;port=3307;password=zayzay;database=nameofdbgame;";
    //    using (var conn = new MySqlConnection(connStr))
    //    using (var cmd = conn.CreateCommand())
    //    {
    //        conn.Open();
    //        cmd.CommandText = "CREATE TABLE customers (id INT AUTO_INCREMENT    PRIMARY KEY, name VARCHAR(255), address VARCHAR(255))";
    //        var response = cmd.ExecuteNonQuery();
    //        Console.WriteLine($"Response: {response}");

    //        cmd.CommandText = "ALTER TABLE customers ADD COLUMN surname VARCHAR(255)";
    //        response = cmd.ExecuteNonQuery();
    //        Console.WriteLine($"Response: {response}");
    //    }

    //}


    //public void createUser()
    //{
    //    string connStr = "server=localhost;user=root;port=3307;password=zayzay;database=nameofdbgame;";
    //    using (var conn = new MySqlConnection(connStr))
    //    using (var cmd = conn.CreateCommand())
    //    {
    //        conn.Open();
    //        cmd.CommandText = "insert into Leaderboard (Username,Wins,Losses) values(@Username, @wins, @losses)";
    //        cmd.Parameters.AddWithValue("@username", "Ivan");
    //        cmd.Parameters.AddWithValue("@wins", 0);
    //        cmd.Parameters.AddWithValue("@losses", 0);
    //        cmd.ExecuteNonQuery();
    //        conn.Close();
    //    }
    //}


    //public void listTable()
    //{
    //    string connStr = "server=localhost;user=root;port=3307;password=zayzay;database=nameofdbgame;";
    //    var connection = new MySqlConnection(connStr);

    //    try
    //    {
    //        connection.Open();
    //        MySqlCommand command = new MySqlCommand("SELECT * FROM customers", connection);
    //        using (MySqlDataReader reader = command.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                // access your record colums by using reader
    //                Console.WriteLine(reader["name"]);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // handle exception here
    //    }
    //    finally
    //    {
    //        connection.Close();
    //    }
    //}

    //public void deletePlayer()
    //{
    //    string connStr = "server=localhost;user=root;port=3307;password=zayzay;database=nameofdbgame;";
    //    using (var conn = new MySqlConnection(connStr))
    //    using (var cmd = conn.CreateCommand())
    //    {
    //        conn.Open();
    //        string name = "ivan";
    //        cmd.CommandText = "Delete from players where name = '" + name + "'";
    //        var response = cmd.ExecuteNonQuery();
    //        Console.WriteLine($"Response: {response}");
    //    }


    //}


}












