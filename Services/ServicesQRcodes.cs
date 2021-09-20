using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.DTO;
using voteCollector.Models;

namespace voteCollector.Services
{
    public class ServicesQRcodes
    {
        private ServiceFriends serviceFriends;

        public ServicesQRcodes()
        {
            serviceFriends = new ServiceFriends();
        }
        public ReportQrCodeExchange CheckQrCodes(ResponseMessageDataQRCodeSuccessfully dataQRcodes)
        {
            ReportQrCodeExchange reportQrCodeExchange = new ReportQrCodeExchange
            {
                status = dataQRcodes.status,
                error = dataQRcodes.error,
                numberReceivedCodes = dataQRcodes.items.Count,
                dateTimeRequest = dataQRcodes.date.ToString(),
                notFoundQRcodes = new List<Item>(),
                foundQRcodes = new List<Item>()
            };

            int numberMarkedCodes = 0;
            int numberNotFound = 0;
            ServiceFriends serviceFriends = new ServiceFriends();

            foreach (KeyValuePair<string, string> keyValue in dataQRcodes.items)
            {
                if (keyValue.Key != null && keyValue.Key != "")
                {
                    DateTime dateTimeCheck;
                    try
                    {
                        dateTimeCheck = Convert.ToDateTime(keyValue.Value);
                    }
                    catch
                    {
                        dateTimeCheck = DateTime.UtcNow.AddHours(5);
                    }

                    if (CheckQrCode(keyValue.Key, dateTimeCheck, serviceFriends).Result)
                    {
                        numberMarkedCodes++;
                        reportQrCodeExchange.foundQRcodes.Add(new Item { qrText = keyValue.Key, date = keyValue.Value });
                    }
                    else
                    {
                        numberMarkedCodes++;
                        numberNotFound++;
                        reportQrCodeExchange.notFoundQRcodes.Add(new Item { qrText = keyValue.Key, date = keyValue.Value });
                    }
                }
            }
            reportQrCodeExchange.numberMarkedCodes = numberMarkedCodes;
            reportQrCodeExchange.numberNotFound = numberNotFound;

            return reportQrCodeExchange;
        }

        public async Task<bool> CheckQrCode(string qrCodes, DateTime dateTime, ServiceFriends serviceFriends)
        {
            Friend friendUpdate;
            try
            {
                friendUpdate = serviceFriends.FindUserByQRtext(qrCodes);
                friendUpdate.Voter = true;
                friendUpdate.VotingDate = dateTime;
            }
            catch
            {
                return false;
            }

            try
            {
                await serviceFriends.SaveFriends(friendUpdate);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
