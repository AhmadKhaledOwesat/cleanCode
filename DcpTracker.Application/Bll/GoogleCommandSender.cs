namespace DcpTracker.Application.Bll
{
    using DcpTracker.Infrastructure.Extensions;
    using Google.Apis.Auth.OAuth2;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;

    public class GoogleCommandSender(string firebaseCredentialsPath, string projectId)
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly GoogleCredential _googleCredential = GoogleCredential.FromFile(firebaseCredentialsPath)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

        public async Task SendInternalAsync(string deviceToken, string title, string body, bool isNotify, Dictionary<string, string> dictionary = null)
        {
            var accessToken = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            var json = GetJsonBody(deviceToken, title, body, dictionary, isNotify);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var url = $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send";
            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"FCM send failed: {response.StatusCode} - {responseContent}");
            }
        }
        private static string GetJsonBody(string deviceToken, string title, string body, Dictionary<string, string> dictionary, bool isNotify)
        {
            dictionary = dictionary ?? new Dictionary<string, string>();

            if (isNotify)
            {
                dictionary.Add("title", title);
                dictionary.Add("body", body);
            }

            var message = new
            {
                message = new
                {
                    token = deviceToken,
                    data = dictionary
                }

            };
            return JsonSerializer.Serialize(message);

        }
        public async Task SendCommandAsync(string deviceToken, string command, string[] packages, string apkUrl, string password, string packageName, string wallpaperUrl, bool isInternal, string filePath, string fileName,DateTime? fromDate,DateTime? toDate)
        {
            // Get access token
            var dictionary = new Dictionary<string, string> { { "command", command } };

            if (command == "blacklist_settings" || command == "whitelist_settings")
                dictionary.Add("packages", $"[{string.Join("','", packages.Select(a => a))}]");

            if (command == "uploadSingleFile")
            {
                dictionary.Add("filePath", filePath);
                dictionary.Add("fileName", fileName);
            }

            if (command == "getInternetUsage")
            {
                dictionary.Add("fromDate", fromDate?.ToString());
                dictionary.Add("toDate", toDate?.ToString());
                dictionary.Add("packageName", packageName);
            }

            if (command == "silent_install" && !apkUrl.IsNullOrEmpty())
                dictionary.Add("apkLink", isInternal ? apkUrl : $"https://mobcentra.com\\assets\\applications\\{apkUrl}");

            if (command == "resetPassword")
                dictionary.Add("password", password);

            if (command == "uninstall_app" && !packageName.IsNullOrEmpty())
                dictionary.Add("packageName", packageName);

            if (command == "changeWallPaper" && !wallpaperUrl.IsNullOrEmpty())
                dictionary.Add("wallpaperUrl", $"https://mobcentra.com\\assets\\images\\{wallpaperUrl}" );

            dictionary.Add("fcmToken", deviceToken);

            await SendInternalAsync(deviceToken, string.Empty, string.Empty, false, dictionary);
        }
        public async Task SendNotifyAsync(string deviceToken, string title, string body) => await SendInternalAsync(deviceToken, title, body, true);
    }


}
