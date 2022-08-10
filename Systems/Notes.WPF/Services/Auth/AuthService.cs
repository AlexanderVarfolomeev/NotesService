using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Notes.WPF.Models.Auth;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Services.Auth
{
    internal class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserDialogService _userDialogService;
        public AuthService(HttpClient httpClient, IUserDialogService userDialogService)
        {
            _httpClient = httpClient;
            _userDialogService = userDialogService;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var url = $"{Settings.IdentityRoot}/connect/token";

            var request_body = new[]
            {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", Settings.ClientId),
            new KeyValuePair<string, string>("client_secret", Settings.ClientSecret),
            new KeyValuePair<string, string>("username", loginModel.Email!),
            new KeyValuePair<string, string>("password", loginModel.Password!)
            };

            var requestContent = new FormUrlEncodedContent(request_body);

            var response = await _httpClient.PostAsync(url, requestContent);

            var content = await response.Content.ReadAsStringAsync();

            var loginResult = JsonSerializer.Deserialize<LoginResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();
            loginResult.Successful = response.IsSuccessStatusCode;

            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);
            return loginResult;
        }

        public async Task Logout()
        {

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task Register(LoginModel registerModel)
        {
            string url = $"{Settings.ApiRoot}/Account/Register";


            var body = JsonSerializer.Serialize(registerModel);
            var request = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, request);


            if (!response.IsSuccessStatusCode)
            {
                _userDialogService.ShowError("Ошибка при регистрации аккаунта.", "Ошибка!");
            }
            else
            {
                _userDialogService.ShowInformation("Аккаунт успешно зарегистрирован.", "Успешно.");
            }
        }
    }
}
