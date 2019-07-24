using System.Threading.Tasks;

namespace MCP_WEB
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message);
    }
}