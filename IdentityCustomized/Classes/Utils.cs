using IdentityCustomized.ParsGreenSms;
using System.Configuration;
using System.Threading.Tasks;

namespace IdentityCustomized.Classes
{
    public class Utils
    {
        public static int SendSms(string ToNumber, string MessageText)
        {
            string apiKey = ConfigurationManager.AppSettings["ParsGreenApiKey"];
            string sendNumber = ConfigurationManager.AppSettings["ParsGreenSendNumber"];
            SendSMSSoapClient service = new SendSMSSoapClient();
            return service.SendGroupSmsSimple(
                apiKey
                , sendNumber
                , new ArrayOfString()
                {
                    ToNumber
                },
                MessageText
                ,false
                , "");
        }

        public static Task SendSmsAsync(string ToNumber, string MessageText)
        {
            string apiKey = ConfigurationManager.AppSettings["ParsGreenApiKey"];
            string sendNumber = ConfigurationManager.AppSettings["ParsGreenSendNumber"];
            SendSMSSoapClient service = new SendSMSSoapClient();
            return service.SendGroupSmsSimpleAsync(
                apiKey
                , sendNumber
                , new ArrayOfString()
                {
                    ToNumber
                },
                MessageText
                , false
                , "");
        }
    }
}