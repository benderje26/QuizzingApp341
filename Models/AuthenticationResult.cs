namespace QuizzingApp341.Models;

public enum AccountCreationResult { Success, BadEmail, BadUsername, BadPassword, DuplicateEmail, DuplicateUsername, NetworkError, Other }

public enum LoginResult { Success, BadCredentials, NetworkError, Other }

public enum UpdateEmailResult { Success, BadEmail, DuplicateEmail, NetworkError, Other }

public enum UpdateUsernameResult { Success, BadUsername, DuplicateUsername, NetworkError, Other }

public enum UpdatePasswordResult { Success, BadPassword, NetworkError, Other }

public enum LogoutResult { Success, NetworkError, Other }

public enum DeleteAccountResult { Success, NetworkError, Other }
