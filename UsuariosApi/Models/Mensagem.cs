using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuariosApi.Models
{
    public class Mensagem
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Mensagem(IEnumerable<string> to, string subject, int usuarioId, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = $"http://localhost:6000/ativa?UsuarioId={usuarioId}&CodigoDeAtivacao={content}";
        }
    }
}
