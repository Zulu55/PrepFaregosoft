using Faregosoft.Api2.Models;

namespace Faregosoft.Api2.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
