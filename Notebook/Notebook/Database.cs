using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Notebook {
    public class Database {
        private SqlConnection connection;
        /// <summary>
        /// struct for Errors.
        /// </summary>
        enum ErrorMessage { required, dontMatchPattern, wrongPassword, wrongPasswordConfirmation, usernameAlredyExists };
        
        /// <summary>
        /// create and open Connection to database.
        /// </summary>
        private bool Connect() {
            try {
                this.connection = new SqlConnection("data source=.;initial catalog=Notebook;integrated security=True;");
                this.connection.Open();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }
        /// <summary>
        /// Close connection to database.
        /// </summary>
        private void Disconnect() {
            this.connection.Close();
        }

        public bool AddNewUser(string username, string password) {
            try {
                /// Hashing password.
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                string hashedPassword = Convert.ToBase64String(hashBytes);
                /// Add User to Database.
                Connect();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "NewUser";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param;
                param = command.Parameters.Add("@username", SqlDbType.NVarChar, 25);
                param.Value = username;
                param = command.Parameters.Add("@password", SqlDbType.Text);
                param.Value = hashedPassword;
                command.ExecuteNonQuery();
                Disconnect();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        public bool UserValidation(string username, string password) {
            try {
                Connect();
                //SqlCommand command = new SqlCommand("select true whare exists", this.connection);
                SqlCommand command = new SqlCommand("SELECT * FROM Users where username = @username", this.connection);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();
                string hashedPasssword = null;
                while (reader.Read()) {
                    hashedPasssword = reader[2].ToString();
                }
                Disconnect();
                /// compare password and hashed password.
                if (!String.IsNullOrEmpty(hashedPasssword)) {
                    byte[] hashBytes = Convert.FromBase64String(hashedPasssword);
                    byte[] salt = new byte[16];
                    Array.Copy(hashBytes, 0, salt, 0, 16);
                    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    for (int i = 0; i < 20; i++)
                        if (hashBytes[i + 16] != hash[i])
                            return false;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e) {
                return false;
            }
        }

        public bool FindUser(string username) {
            try {
                Connect();
                SqlCommand command = new SqlCommand("SELECT * FROM Users where username = @username", this.connection);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) {
                    Disconnect();
                    return true;
                }
                else {
                    Disconnect();
                    return false;
                }
            }
            catch (Exception e) {
                return false;
            }
        }
        public DataSet GetDS() {
            Connect();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("SELECT * FROM Users", this.connection);
            da.SelectCommand = command;
            DataSet ds = new DataSet();
            da.Fill(ds);
            Disconnect();
            return ds;
        }
    }
}
