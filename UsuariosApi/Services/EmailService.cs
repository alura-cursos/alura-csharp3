using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class EmailService
    {

        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnviarEmail(string[] to, string subject, int usuarioId, string content)
        {
            var message = new Mensagem(to, subject, usuarioId, content);
            var emailMessage = CriaCorpoDoEmail(message);
            Enviar(emailMessage);
        }

        private MimeMessage CriaCorpoDoEmail(Mensagem message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration.GetValue<string>("EmailSettings:From")));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private void Enviar(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    Console.WriteLine(_configuration.GetValue<string>("EmailSettings:SmtpServer"));
                    Console.WriteLine(_configuration.GetValue<string>("EmailSettings:From"));
                    Console.WriteLine(_configuration.GetValue<string>("EmailSettings:Password"));
                    Console.WriteLine(_configuration.GetValue<int>("EmailSettings:Port"));
                    client.Connect(_configuration.GetValue<string>("EmailSettings:SmtpServer"), _configuration.GetValue<int>("EmailSettings:Port"), true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_configuration.GetValue<string>("EmailSettings:From"), _configuration.GetValue<string>("EmailSettings:Password"));
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}