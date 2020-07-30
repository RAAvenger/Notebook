using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace Notebook {
    public partial class LogIn_SignIn : Window {
        #region variables
        private bool isLogIn;
        private List<Error> listErrors;
        private Database database;
        private MainPage homePage;
        private TitleBar titleBar;
        private WindowProperties windowProperties;
        #endregion variables

        #region Error data structur
        /// <summary>
        /// struct for Errors.
        /// </summary>
        enum ErrorMessage { required, dontMatchPattern, invalidUser, wrongPasswordConfirmation, usernameAlredyExists };
        struct Error {
            public string FieldName;
            public ErrorMessage Message;
            public Error(string FieldName, ErrorMessage Message) {
                this.FieldName = FieldName;
                this.Message = Message;
            }
        }
        #endregion Error data structur

        public LogIn_SignIn(MainPage homePage, Database database, WindowProperties windowProperties) {
            InitializeComponent();
            this.windowProperties = windowProperties;
            titleBar = new TitleBar(this, homePage);
            this.homePage = homePage;
            this.database = database;
            isLogIn = true;
            listErrors = new List<Error> {
                new Error("LogIn_Username", ErrorMessage.required),
                new Error("LogIn_Password", ErrorMessage.required)
            };
            button_LogIn_Submit.IsEnabled = false;
        }

        #region TitleBar
        /// <summary>
        /// back to log in form. 
        /// </summary>
        private void Button_Back_Click(object sender, RoutedEventArgs e) {
            isLogIn = false;
            form_LogIn.Visibility = Visibility.Visible;
            form_SignIn.Visibility = Visibility.Collapsed;
            button_Back.Visibility = Visibility.Collapsed;
            Title = "Log In";
            button_LogIn_Submit.IsEnabled = false;
            listErrors.Clear();
            listErrors.Add(new Error("LogIn_Username", ErrorMessage.required));
            listErrors.Add(new Error("LogIn_Password", ErrorMessage.required));

        }
        /// <summary>
        /// Close Window.
        /// </summary>
        private void Button_Close_Click(object sender, RoutedEventArgs e) {
            titleBar.Close();
        }
        /// <summary>
        /// call AdjustWindowSize() to change window size.
        /// </summary>
        private void Button_Maximize_Click(object sender, RoutedEventArgs e) {
            titleBar.Maximize(sender);
        }
        /// <summary>
        /// Minimize Window.
        /// </summary>
        private void Button_Minimize_Click(object sender, RoutedEventArgs e) {
            titleBar.Minimize();
        }
        #endregion TitleBar

        #region open sign in form and close log in form 
        /// <summary>
        /// show Sign In Form and Hide Log In Form
        /// </summary>
        private void Button_SignIn_Click(object sender, RoutedEventArgs e) {
            isLogIn = false;
            form_LogIn.Visibility = Visibility.Collapsed;
            form_SignIn.Visibility = Visibility.Visible;
            button_Back.Visibility = Visibility.Visible;
            Title = "Sign In";
            button_SignIn_Submit.IsEnabled = false;
            listErrors.Clear();
            listErrors.Add(new Error("SignIn_Username", ErrorMessage.required));
            listErrors.Add(new Error("SignIn_Password", ErrorMessage.required));
            listErrors.Add(new Error("SignIn_Confirmation", ErrorMessage.required));
        }
        #endregion open sign in form and close log in form

        #region Submit Functions
        /// <summary>
        /// submit Log In Data. 
        /// </summary>
        private void Button_LogIn_Submit_Click(object sender, RoutedEventArgs e) {
            textBox_LogIn_Username.Text = textBox_LogIn_Username.Text.Trim();
            int validationResult = database.UserValidation(textBox_LogIn_Username.Text, passwordBox_LogIn_Password.Password);
            if (validationResult > 0) {
                homePage.SetUserIdAndUsername(validationResult, textBox_LogIn_Username.Text);
                homePage.Refresh();
                homePage.Show();
                Close();
            }
            else {
                listErrors.Add(new Error("LogIn_Password", ErrorMessage.invalidUser));
                ValidateForm();
                MessageBox.Show("no");
            }
        }
        /// <summary>
        /// submit Sign In Data
        /// </summary>
        private void Button_SignIn_Submit_Click(object sender, RoutedEventArgs e) {
            textBox_SignIn_Username.Text = textBox_SignIn_Username.Text.Trim();
            int addewUserResult = database.AddNewUser(textBox_SignIn_Username.Text, passwordBox_SignIn_Password.Password);
            if (addewUserResult > 0) {
                homePage.SetUserIdAndUsername(addewUserResult, textBox_SignIn_Username.Text);
                homePage.Refresh();
                homePage.Show();
                Close();
            }
            else
                MessageBox.Show("Error");

        }
        /// <summary>
        /// call logIn or signIn submit functions when user press "Enter".
        /// </summary>
        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (isLogIn && button_LogIn_Submit.IsEnabled)
                    Button_LogIn_Submit_Click(null, null);
                else if (!isLogIn && button_SignIn_Submit.IsEnabled)
                    Button_SignIn_Submit_Click(null, null);
            }
        }
        #endregion Submit Function

        #region User Inputs TextChanged Functions
        /// <summary>
        /// check username: required.
        /// </summary>
        private void TextBox_LogIn_Username_TextChanged(object sender, TextChangedEventArgs e) {
            if (string.IsNullOrEmpty(textBox_LogIn_Username.Text) || string.IsNullOrWhiteSpace(textBox_LogIn_Username.Text)) {
                listErrors.Add(new Error("LogIn_Username", ErrorMessage.required));
            }
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "LogIn_Username");
                for (int i = elementErrors.Count - 1; i >= 0; i--) {
                    listErrors.RemoveAt(elementErrors[i]);
                }
            }
            PasswordBox_LogIn_Password_PasswordChanged(null, null);
            ValidateForm();
        }
        /// <summary>
        /// check password: required.
        /// </summary>
        private void PasswordBox_LogIn_Password_PasswordChanged(object sender, RoutedEventArgs e) {
            List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "LogIn_Password");
            for (int i = elementErrors.Count - 1; i >= 0; i--) {
                listErrors.RemoveAt(elementErrors[i]);
            }
            if (string.IsNullOrEmpty(passwordBox_LogIn_Password.Password) || string.IsNullOrWhiteSpace(passwordBox_LogIn_Password.Password)) {
                listErrors.Add(new Error("LogIn_Password", ErrorMessage.required));
            }

            ValidateForm();
        }

        /// <summary>
        /// check username: required, already exists in database.
        /// </summary>
        private void TextBox_SignIn_Username_TextChanged(object sender, TextChangedEventArgs e) {
            List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "SignIn_Username");
            for (int i = elementErrors.Count - 1; i >= 0; i--) {
                listErrors.RemoveAt(elementErrors[i]);
            }
            if (string.IsNullOrEmpty(textBox_SignIn_Username.Text) || string.IsNullOrWhiteSpace(textBox_SignIn_Username.Text)) {
                listErrors.Add(new Error("SignIn_Username", ErrorMessage.required));
            }
            else if (database.FindUser(textBox_SignIn_Username.Text) > 0) {
                listErrors.Add(new Error("SignIn_Username", ErrorMessage.usernameAlredyExists));
                MessageBox.Show("alredy exists.");
            }
            ValidateForm();
        }
        /// <summary>
        /// check password: required.
        /// </summary>
        private void PasswordBox_SignIn_Password_PasswordChanged(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(passwordBox_SignIn_Password.Password) || string.IsNullOrWhiteSpace(passwordBox_SignIn_Password.Password)) {
                listErrors.Add(new Error("SignIn_Password", ErrorMessage.required));
            }
            else {
                List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "SignIn_Password");
                for (int i = elementErrors.Count - 1; i >= 0; i--)
                    listErrors.RemoveAt(elementErrors[i]);
            }
            PasswordBox_SignIn_Confirmation_PasswordChanged(null, null);
            ValidateForm();
        }
        /// <summary>
        /// check password confirmation: required, confirmation match password.
        /// </summary>
        private void PasswordBox_SignIn_Confirmation_PasswordChanged(object sender, RoutedEventArgs e) {
            List<int> elementErrors = FindFieldErrorsIndexes(listErrors, "SignIn_Confirmation");
            for (int i = elementErrors.Count - 1; i >= 0; i--) {
                listErrors.RemoveAt(elementErrors[i]);
            }
            if (string.IsNullOrEmpty(passwordBox_SignIn_Confirmation.Password) || string.IsNullOrWhiteSpace(passwordBox_SignIn_Confirmation.Password)) {
                listErrors.Add(new Error("SignIn_Confirmation", ErrorMessage.required));
            }
            else if (passwordBox_SignIn_Confirmation.Password != passwordBox_SignIn_Password.Password) {
                listErrors.Add(new Error("SignIn_Confirmation", ErrorMessage.wrongPasswordConfirmation));
            }
            ValidateForm();
        }
        #endregion

        #region Validation Functions
        /// <summary>
        /// Validate form and Enable/UnEnable submit button.
        /// Show invalid input fields.
        /// </summary>
        private void ValidateForm() {
            ClearErrors();
            if (listErrors.Count != 0) {
                if (isLogIn) {
                    button_LogIn_Submit.IsEnabled = false;
                }
                else {
                    button_SignIn_Submit.IsEnabled = false;
                }

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
                        case ErrorMessage.invalidUser: {
                            errorText = "نام کاربری یا رمز ورود غلط است";
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
        #endregion
            }
}