namespace TCGPlayerApiWrapper.Models;

public class AuthToken {
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public long ExpiresIn { get; set; }

    public string UserName { get; set; }
}