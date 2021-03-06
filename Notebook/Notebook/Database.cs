﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Notebook {
    public class Database {
        #region variables
        private SqlConnection connection;
        #endregion variables
        #region Connection
        /// <summary>
        /// create and open Connection to database.
        /// </summary>
        /// <returns>
        /// true -> success.
        /// false -> error.
        /// </returns>
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
        #endregion Connection
        #region User Functions
        /// <summary>
        /// add new user to database.
        /// </summary>
        /// <returns>
        /// -1 -> unknown error.
        /// -2 -> username added but username not found in database.
        /// </returns>
        public int AddNewUser(string username, string password) {
            try {
                /// Hashing password.
                string hashedPassword = HashingPassword(password);
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
                return FindUser(username);
            }
            catch (Exception e) {
                return -1;
            }
        }

        /// <summary>
        /// Validate User
        /// </summary>
        /// <returns>
        /// -1 -> unknown error.
        /// -2 -> wrong username.
        /// -3 -> wrong password.
        /// positive number -> user id.
        /// </returns>
        public int UserValidation(string username, string password) {
            try {
                Connect();
                //SqlCommand command = new SqlCommand("select true whare exists", this.connection);
                SqlCommand command = new SqlCommand("SELECT * FROM Users where username = @username", this.connection);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();
                string hashedPasssword = null;
                int userID = -4;
                if (reader.Read()) {
                    userID = Int32.Parse(reader[0].ToString());
                    hashedPasssword = reader[2].ToString();
                    Disconnect();
                    if (PasswordValidation(hashedPasssword, password))
                        return userID;
                    else
                        return -3;
                }
                else {
                    Disconnect();
                    return -2;
                }
            }
            catch (Exception e) {
                return -1;
            }
        }

        /// <summary>
        /// search for username.
        /// </summary>
        /// <returns>
        /// -1 -> unknown error.
        /// -2 -> username not found.
        /// positive number -> user id.
        /// </returns>
        public int FindUser(string username) {
            try {
                Connect();
                SqlCommand command = new SqlCommand("SELECT * FROM Users where username = @username", this.connection);
                command.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) {
                    int userID = Int32.Parse(reader[0].ToString()); ;
                    Disconnect();
                    return userID;
                }
                else {
                    Disconnect();
                    return -2;
                }
            }
            catch (Exception e) {
                return -1;
            }
        }
        #endregion User Functions
        #region Note Functions
        /// <summary>
        /// Add new Note to database.
        /// </summary>
        /// <returns>
        /// -1 -> error.
        /// 1 -> success.
        /// </returns>
        public int AddNewNote(int writerID, string noteTitle, string noteText) {
            try {
                noteTitle = CheckUserInput(noteTitle);
                noteText = CheckUserInput(noteText);
                Connect();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText = "AddNewNote";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param;
                param = command.Parameters.Add("@writerID", SqlDbType.Int);
                param.Value = writerID;
                param = command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                param.Value = noteTitle;
                param = command.Parameters.Add("@text", SqlDbType.Text);
                param.Value = noteText;
                command.ExecuteNonQuery();
                Disconnect();
                return 1;
            }
            catch (Exception e) {
                return -1;
            }
        }

        /// <summary>
        /// gets all user notes from detabase.
        /// </summary>
        /// <returns>
        /// a detaSet of user notes.
        /// </returns>
        public DataSet GetUserNotes(int writerID) {
            Connect();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("SELECT * FROM Notes Where writerid = @writerID", this.connection);
            SqlParameter param;
            param = command.Parameters.Add("@writerID", SqlDbType.Int);
            param.Value = writerID;
            dataAdapter.SelectCommand = command;
            DataSet result = new DataSet();
            dataAdapter.Fill(result);
            Disconnect();
            return result;
        }
        /// <summary>
        /// update note title and text.
        /// </summary>
        /// <returns>
        /// -1 -> error
        /// 1 -> success
        /// </returns>
        public int EditNote(int noteID, string noteTitle, string noteText) {
            try {
                noteTitle = CheckUserInput(noteTitle);
                noteText = CheckUserInput(noteText);
                Connect();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText =
                    "Update notes " +
                    "SET title = @noteTitle, text = @noteText " +
                    "WHERE id = @noteID";
                SqlParameter param;
                param = command.Parameters.Add("@noteID", SqlDbType.Int);
                param.Value = noteID;
                param = command.Parameters.Add("@noteTitle", SqlDbType.NVarChar, 50);
                param.Value = noteTitle;
                param = command.Parameters.Add("@noteText", SqlDbType.Text);
                param.Value = noteText;
                command.ExecuteNonQuery();
                Disconnect();
                return 1;
            }
            catch (Exception e) {
                return -1;
            }
        }

        public int DeleteNote(int noteID) {
            try {
                Connect();
                SqlCommand command = new SqlCommand();
                command.Connection = this.connection;
                command.CommandText =
                    "DELETE FROM notes " +
                    "WHERE id = @noteID";
                SqlParameter param;
                param = command.Parameters.Add("@noteID", SqlDbType.Int);
                param.Value = noteID;
                command.ExecuteNonQuery();
                Disconnect();
                return 1;
            }
            catch (Exception e) {
                return -1;
            }
        }
        #endregion Note Functions

        private string CheckUserInput(string input) {
            return String.IsNullOrEmpty(input) ? "null" : input;
        }
        #region security
        /// <summary>
        /// hash the password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>
        /// hashed password.
        /// </returns>
        private string HashingPassword(string password) {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// compare input password and hashed password.
        /// </summary>
        /// <returns>
        /// true -> password is valid.
        /// false -> password is invalid.
        /// </returns>
        private bool PasswordValidation(string hashedPasssword, string password) {
            /// compare password and hashed password.
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
        #endregion security
    }
}
