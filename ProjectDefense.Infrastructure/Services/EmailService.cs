using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Infrastructure.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace ProjectDefense.Infrastructure.Services
{
    public class EmailService : IEmailService, IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly MailGunSettings _mailGunSettings;

        public EmailService(HttpClient httpClient, IOptions<MailGunSettings> mailGunSettings)
        {
            _httpClient = httpClient;
            _mailGunSettings = mailGunSettings.Value;

            _httpClient.BaseAddress = new Uri(_mailGunSettings.ApiBaseUrl);
            var byteArray = Encoding.ASCII.GetBytes($"api:{_mailGunSettings.ApiKey}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("from", $"Mailgun Sandbox <postmaster@{_mailGunSettings.Domain}>"),
                new KeyValuePair<string, string>("to", email),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("html", htmlMessage)
            });

            var response = await _httpClient.PostAsync($"/v3/{_mailGunSettings.Domain}/messages", content);
            response.EnsureSuccessStatusCode();
        }
    }
}