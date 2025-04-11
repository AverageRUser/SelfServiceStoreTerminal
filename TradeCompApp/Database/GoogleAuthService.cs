
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Database
{
    public class GoogleAuthService
    {
        public async Task<string> GetOAuthTokenAsync()
        {
            try
            {
                var clientSecrets = new ClientSecrets
                {
                    ClientId = await SecureStorage.GetAsync("GoogleClientId"),
                    ClientSecret = await SecureStorage.GetAsync("GoogleClientSecret")

                };
                var storedRefreshToken = await SecureStorage.GetAsync("GmailRefreshToken");
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    new[] { "https://www.googleapis.com/auth/gmail.send" },
                    "uspehru231@gmail.com",
                    CancellationToken.None,
                    new FileDataStore("MailKit.Auth.Store"));
                if (credential.Token.IsExpired(SystemClock.Default))
                {
                    if (!string.IsNullOrEmpty(credential.Token.RefreshToken))
                    {
                        // Если есть refresh token - используем его
                        await credential.RefreshTokenAsync(CancellationToken.None);
                    }
                    else if (!string.IsNullOrEmpty(storedRefreshToken))
                    {
                        // Если refresh token был сохранён ранее - пробуем использовать его
                        credential.Token.RefreshToken = storedRefreshToken;
                        await credential.RefreshTokenAsync(CancellationToken.None);
                    }
                    else
                    {
                        throw new InvalidOperationException("Token expired and no refresh token available");
                    }

                }
                if (!string.IsNullOrEmpty(credential.Token.RefreshToken))
                {
                    await SecureStorage.SetAsync("GmailRefreshToken", credential.Token.RefreshToken);
                }
                return credential.Token.AccessToken;
               
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Ошибка","Не удалось получить данные из Google Api Service","OK");
                throw;
            }
         
        }
      
    }

   
}
