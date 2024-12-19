namespace SkyBro.Authentication;

public interface IAuthenticator
{
    void AttachToClient(HttpClient client);
}