using System;
using System.Windows;
using System.Windows.Controls;

namespace Notbook {
    public partial class LogIn_SignIn : Window {
        public LogIn_SignIn() {
            InitializeComponent();
            button_LogIn_Submit.IsEnabled = false;
            button_SignIn_Submit.IsEnabled = false;
        }
        /// <summary>
        /// show Sign In Form and Hide Log In Form
        /// </summary>
        private void Button_SignIn_Click(object sender, RoutedEventArgs e) {
            form_LogIn.Visibility = Visibility.Collapsed;
            form_SignIn.Visibility = Visibility.Visible;
            this.Title = "Sign In";
        }
        /// <summary>
        /// submit Log In Data. 
        /// </summary>
        private void Button_LogIn_Submit_Click(object sender, RoutedEventArgs e) {
            string username;
            string password;
        }
        /// <summary>
        /// submit Sign In Data
        /// </summary>
        private void Button_SignIn_Submit_Click(object sender, RoutedEventArgs e) {

        }
        /// <summary>
        /// Log In input Validation
        /// </summary>
        private void TextBox_LogIn_Username_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.IsNullOrEmpty(textBox_LogIn_Username.Text) || String.IsNullOrWhiteSpace(textBox_LogIn_Username.Text))
                button_LogIn_Submit.IsEnabled = false;
            else
                button_LogIn_Submit.IsEnabled = true;
        }

        private void PasswordBox_LogIn_Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (String.IsNullOrEmpty(passwordBox_LogIn_Password.Password) || String.IsNullOrWhiteSpace(passwordBox_LogIn_Password.Password))
                button_LogIn_Submit.IsEnabled = false;
            else
                button_LogIn_Submit.IsEnabled = true;
        }
        /// <summary>
        /// Sign In Input Validation
        /// </summary>
        private void TextBox_SignIn_Username_TextChanged(object sender, TextChangedEventArgs e) {
            if (!(String.IsNullOrEmpty(textBox_SignIn_Username.Text) || String.IsNullOrWhiteSpace(textBox_SignIn_Username.Text))
                && !(String.IsNullOrEmpty(passwordBox_SignIn_Password.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Password.Password))
                && !(String.IsNullOrEmpty(passwordBox_SignIn_Confirmation.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Confirmation.Password)))
                button_SignIn_Submit.IsEnabled = true;
            else
                button_SignIn_Submit.IsEnabled = false;
        }

        private void PasswordBox_SignIn_Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if ((String.IsNullOrEmpty(textBox_SignIn_Username.Text) || String.IsNullOrWhiteSpace(textBox_SignIn_Username.Text))
                && (String.IsNullOrEmpty(passwordBox_SignIn_Password.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Password.Password))
                && (String.IsNullOrEmpty(passwordBox_SignIn_Confirmation.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Confirmation.Password)))
                button_SignIn_Submit.IsEnabled = false;
            else
                button_SignIn_Submit.IsEnabled = true;
        }

        private void PasswordBox_SignIn_Confirmation_PasswordChanged(object sender, RoutedEventArgs e) {
            if ((String.IsNullOrEmpty(textBox_SignIn_Username.Text) || String.IsNullOrWhiteSpace(textBox_SignIn_Username.Text))
                    && (String.IsNullOrEmpty(passwordBox_SignIn_Password.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Password.Password))
                    && (String.IsNullOrEmpty(passwordBox_SignIn_Confirmation.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Confirmation.Password)))
                button_SignIn_Submit.IsEnabled = false;
            else
                button_SignIn_Submit.IsEnabled = true;
        }
    }

}