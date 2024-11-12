namespace QuizzingApp341.Models;

public enum AccountCreationResult { Success, BadEmail, BadUsername, BadPassword, DuplicateEmail, DuplicateUsername, NetworkError }

public enum LoginResult { Success, BadCredentials, NetworkError }
