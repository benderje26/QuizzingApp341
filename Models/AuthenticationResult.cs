namespace QuizzingApp341.Models;

public enum AccountCreationResult { Success, BadEmail, BadUsername, BadPassword, DuplicateEmail, DuplicateUsername, NetworkError, Other }

public enum LoginResult { Success, BadCredentials, NetworkError, Other }

public enum LogoutResult { Success, NetworkError, Other }

public enum ResetPasswordResult { EmailSent, BadEmail, NetworkError }
