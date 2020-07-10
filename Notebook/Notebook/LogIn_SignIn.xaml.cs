using Notebook;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace Notbook {
    public partial class LogIn_SignIn : Window {
        /// <summary>
        /// variables.
        /// </summary>
        private bool isLogIn;
        private List<Error> listErrors;
        private Database database;
        /// <summary>
        /// struct for Errors.
        /// </summary>
        enum ErrorMessage { required, dontMatchPattern, wrongPassword, wrongPasswordConfirmation, usernameAlredyExists };
        struct Error {
            public string FieldName;
            public ErrorMessage Message;
            public Error(string FieldName, ErrorMessage Message) {
                this.FieldName = FieldName;
                this.Message = Message;
            }
        }

        public LogIn_SignIn() {
            InitializeComponent();
            this.isLogIn = true;
            button_LogIn_Submit.IsEnabled = false;
            this.listErrors = new List<Error>();
            this.listErrors.Add(new Error("LogIn_Username", ErrorMessage.required));
            this.listErrors.Add(new Error("LogIn_Password", ErrorMessage.required));
            this.database = new Database();
        }
        /// <summary>
        /// show Sign In Form and Hide Log In Form
        /// </summary>
        private void Button_SignIn_Click(object sender, RoutedEventArgs e) {
            this.isLogIn = false;
            form_LogIn.Visibility = Visibility.Collapsed;
            form_SignIn.Visibility = Visibility.Visible;
            this.Title = "Sign In";
            button_SignIn_Submit.IsEnabled = false;
            this.listErrors.Clear();
            this.listErrors.Add(new Error("SignIn_Username", ErrorMessage.required));
            this.listErrors.Add(new Error("SignIn_Password", ErrorMessage.required));
            this.listErrors.Add(new Error("SignIn_Confirmation", ErrorMessage.required));
        }

        /// <summary>
        /// submit Log In Data. 
        /// </summary>
        private void Button_LogIn_Submit_Click(object sender, RoutedEventArgs e) {
           if( database.UserValidation(textBox_LogIn_Username.Text, passwordBox_LogIn_Password.Password))
                MessageBox.Show("yes");
           else
                MessageBox.Show("no");
        }

        /// <summary>
        /// submit Sign In Data
        /// </summary>
        private void Button_SignIn_Submit_Click(object sender, RoutedEventArgs e) {
            database.AddNewUser(textBox_SignIn_Username.Text, passwordBox_SignIn_Password.Password);
        }

        /// <summary>
        /// Log In input Validation
        /// </summary>
        private void TextBox_LogIn_Username_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.IsNullOrEmpty(textBox_LogIn_Username.Text) || String.IsNullOrWhiteSpace(textBox_LogIn_Username.Text))
                listErrors.Add(new Error("LogIn_Username", ErrorMessage.required));
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "LogIn_Username");
                for (int i = elementErrors.Count - 1; i >= 0; i--)
                    listErrors.RemoveAt(elementErrors[i]);
            }
            ValidateForm();
        }
        private void PasswordBox_LogIn_Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (String.IsNullOrEmpty(passwordBox_LogIn_Password.Password) || String.IsNullOrWhiteSpace(passwordBox_LogIn_Password.Password))
                listErrors.Add(new Error("LogIn_Password", ErrorMessage.required));
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "LogIn_Password");
                for (int i = elementErrors.Count - 1; i >= 0; i--)
                    listErrors.RemoveAt(elementErrors[i]);
            }
            ValidateForm();
        }

        /// <summary>
        /// Sign In Input Validation
        /// </summary>
        private void TextBox_SignIn_Username_TextChanged(object sender, TextChangedEventArgs e) {
            if (String.IsNullOrEmpty(textBox_SignIn_Username.Text) || String.IsNullOrWhiteSpace(textBox_SignIn_Username.Text))
                listErrors.Add(new Error("SignIn_Username", ErrorMessage.required));
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "SignIn_Username");
                for (int i = elementErrors.Count - 1; i >= 0; i--)
                    listErrors.RemoveAt(elementErrors[i]);
            }
            ValidateForm();
        }
        private void PasswordBox_SignIn_Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (String.IsNullOrEmpty(passwordBox_SignIn_Password.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Password.Password))
                listErrors.Add(new Error("SignIn_Password", ErrorMessage.required));
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "SignIn_Password");
                for (int i = elementErrors.Count - 1; i >= 0; i--)
                    listErrors.RemoveAt(elementErrors[i]);
            }
            ValidateForm();
        }
        private void PasswordBox_SignIn_Confirmation_PasswordChanged(object sender, RoutedEventArgs e) {
            if (String.IsNullOrEmpty(passwordBox_SignIn_Confirmation.Password) || String.IsNullOrWhiteSpace(passwordBox_SignIn_Confirmation.Password))
                listErrors.Add(new Error("SignIn_Confirmation", ErrorMessage.required));
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "SignIn_Confirmation");
                for (int i = elementErrors.Count - 1; i >= 0; i--)
                    listErrors.RemoveAt(elementErrors[i]);
            }
            ValidateForm();
        }

        /// <summary>
        /// Validate form and Enable/UnEnable submit button.
        /// Show invalid input fields.
        /// </summary>
        private void ValidateForm() {
            ClearErrors();
            if (listErrors.Count != 0) {
                if (isLogIn)
                    button_LogIn_Submit.IsEnabled = false;
                else
                    button_SignIn_Submit.IsEnabled = false;
                foreach (Error error in listErrors) {
                    string name = string.Concat("textBlock_", error.FieldName, "_Error_", error.Message.ToString());
                    TextBlock textBlock_Error = new TextBlock() {
                        Name = name,
                        Foreground = Brushes.Red,
                        FontSize = 12,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        Width = Double.NaN,
                        Height = Double.NaN
                    };
                    string errorText = null;
                    int row = 5;
                    switch (error.Message) {
                        case ErrorMessage.required: {
                            errorText = "فیلد بالا را پر کنید";
                            row = 0;
                            break;
                        }
                        case ErrorMessage.dontMatchPattern: {
                            errorText = "مقدار ورودی با الگو تطابق ندارد";
                            row = 1;
                            break;
                        }
                        case ErrorMessage.wrongPassword: {
                            errorText = "رمز ورود غلط است";
                            row = 2;
                            break;
                        }
                        case ErrorMessage.wrongPasswordConfirmation: {
                            errorText = "رمز عبور و تکرار رمز عبور همخوانی ندارند";
                            row = 3;
                            break;
                        }
                        case ErrorMessage.usernameAlredyExists: {
                            errorText = "نام‌کاربری موجود می‌باشد";
                            row = 4;
                            break;
                        }
                    }
                    textBlock_Error.Text = String.IsNullOrEmpty(errorText) ? "خطا" : errorText;
                    switch (error.FieldName) {
                        case "LogIn_Username": {
                            grid_LogIn_Username_Error.Children.Add(textBlock_Error);
                            Grid.SetRow(textBlock_Error, row);
                            break;
                        }
                        case "LogIn_Password": {
                            grid_LogIn_Password_Error.Children.Add(textBlock_Error);
                            Grid.SetRow(textBlock_Error, row);
                            break;
                        }
                        case "SignIn_Username": {
                            grid_SignIn_Username_Error.Children.Add(textBlock_Error);
                            Grid.SetRow(textBlock_Error, row);
                            break;
                        }
                        case "SignIn_Password": {
                            grid_SignIn_Password_Error.Children.Add(textBlock_Error);
                            Grid.SetRow(textBlock_Error, row);
                            break;
                        }
                        case "SignIn_Confirmation": {
                            grid_SignIn_Confirmation_Error.Children.Add(textBlock_Error);
                            Grid.SetRow(textBlock_Error, row);
                            break;
                        }
                    }
                }
            }
            else {
                if (isLogIn) {
                    grid_LogIn_Username_Error.Children.Clear();
                    grid_LogIn_Password_Error.Children.Clear();
                    button_LogIn_Submit.IsEnabled = true;
                }
                else {
                    grid_SignIn_Username_Error.Children.Clear();
                    grid_SignIn_Password_Error.Children.Clear();
                    grid_SignIn_Confirmation_Error.Children.Clear();
                    button_SignIn_Submit.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// find indexes of errors that blong to a form element.
        /// </summary>
        /// <returns>indexes of errors</returns>
        private List<int> FindFieldErrorsIndexes(List<Error> errors, string fieldName) {
            List<int> indexes = new List<int>();
            for (int i = 0; i < errors.Count; i++)
                if (errors[i].FieldName == fieldName)
                    indexes.Add(i);
            return indexes;
        }

        /// <summary>
        /// clear Errors from errorGrids.
        /// </summary>
        private void ClearErrors() {
            if (isLogIn) {
                grid_LogIn_Username_Error.Children.Clear();
                grid_LogIn_Password_Error.Children.Clear();
            }
            else {
                grid_SignIn_Username_Error.Children.Clear();
                grid_SignIn_Password_Error.Children.Clear();
                grid_SignIn_Confirmation_Error.Children.Clear();
            }
        }
    }
}