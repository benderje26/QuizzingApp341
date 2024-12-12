namespace QuizzingApp341.Models;

public enum AccountCreationResult { Success, BadEmail, BadUsername, BadPassword, DuplicateEmail, DuplicateUsername, NetworkError, Other }

public enum LoginResult { Success, BadCredentials, NetworkError, Other }

public enum UpdateEmailResult { Success, BadEmail, DuplicateEmail, NotSignedIn, NetworkError, Other }

public enum UpdateUsernameResult { Success, BadUsername, DuplicateUsername, NotSignedIn, NetworkError, Other }

public enum UpdatePasswordResult { Success, BadPassword, NotSignedIn, NetworkError, Other }

public enum LogoutResult { Success, NotSignedIn, NetworkError, Other }

public enum DeleteAccountResult { Success, NotSignedIn, NetworkError, Other }
