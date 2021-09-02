using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using voteCollector.DTO;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "admin, user")]
    public class QRcodeСheckAPIController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly IHttpClientFactory httpClientFactory;

        private readonly ILogger<QRcodeСheckAPIController> _logger;

        public QRcodeСheckAPIController(ILogger<QRcodeСheckAPIController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            httpClientFactory = clientFactory;
            httpClient = new HttpClient();
        }

        // "/api/QRcodeСheckAPI/checkqrcode?qrText="
        [HttpGet]
        [Route("checkqrcode")]
        public async Task<IActionResult> CheckQRcode(String qrText)
        {
            if (qrText != null && qrText != "")
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                Friend friendUpdate = null;
                try
                {
                    friendUpdate = serviceFriends.FindUserByQRtext(qrText);
                    friendUpdate.Voter = true;
                    friendUpdate.VotingDate = DateTime.Now.Date;
                    
                }
                catch(Exception ex)
                {
                    return Ok("Пользователь не найден.");
                }

                try
                {
                    await serviceFriends.SaveFriends(friendUpdate);
                    return Ok("<h4>"+ friendUpdate.Name+" "+friendUpdate.PatronymicName + ", поздравляем, Вы зарегистрированы как участник розыгрыша!" + "</h4>");
                }
                catch(Exception ex)
                {
                    return Ok("<h4>Не удалось сохранить данные. Обратитесь к администратору!</h4>" + " "+ex.ToString());
                }
            }
            else
            {
                return Ok("<h4>Ошибка QR-кода. QR-код не содержет индификатора пользователя.</h4>");
            }
            
        }

        // "/api/QRcodeСheckAPI/checphonenumber?phoneNumber="
        [HttpGet]
        [Route("checkphonenumber")]
        public async Task<IActionResult> CheckPhoneNumber(String phoneNumber)
        {
            if (phoneNumber != null && phoneNumber != "")
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                string clearPhoneNumber = ServicePhoneNumber.LeaveOnlyNumbers(phoneNumber).Substring(1,10);
                Friend friendUpdate = null;
                try
                {
                    friendUpdate = serviceFriends.FindUserByPhoneNumber(clearPhoneNumber);
                    friendUpdate.Voter = true;
                    friendUpdate.VotingDate = DateTime.Now.Date;

                }
                catch (Exception ex)
                {
                    return Ok("Пользователь не найден.");
                }

                try
                {
                    await serviceFriends.SaveFriends(friendUpdate);
                    return Ok("<h4>" + friendUpdate.Name + " " + friendUpdate.PatronymicName + ", поздравляем, Вы зарегистрированы как участник розыгрыша!" + "</h4>");
                }
                catch (Exception ex)
                {
                    return Ok("<h4>Не удалось сохранить данные. Обратитесь к администратору!</h4>" + " " + ex.ToString());
                }
            }
            else
            {
                return Ok("<h4>Ошибка. Запрос не содержет номера телефона.</h4>");
            }
        }

        //"/api/QRcodeСheckAPI/checkmasjsonqrcode"
        [HttpPost]
        [Route("checkmasjsonqrcode")]
        public void CheckMasjsonqrcode(string[] qrCodeDTOs)
        {
            if (qrCodeDTOs.Length > 0)
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                foreach (string qrcode in qrCodeDTOs)
                {
                    if (qrcode != null && qrcode != "")
                    {
                        Friend friendUpdate;
                        try
                        {
                            friendUpdate = serviceFriends.FindUserByQRtext(qrcode);
                            friendUpdate.Voter = true;
                            friendUpdate.VotingDate = DateTime.Now.Date;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        try
                        {
                            serviceFriends.SaveFriends(friendUpdate);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        //"/api/QRcodeСheckAPI/checkmasjsonphonenumber"
        [HttpPost]
        [Route("checkmasjsonphonenumber")]
        public void CheckMasjsonPhonenumber(string[] phoneNumberDTOs)
        {

            if (phoneNumberDTOs.Length > 0)
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                foreach (string phoneNumber in phoneNumberDTOs) {

                    string clearPhoneNumber = ServicePhoneNumber.LeaveOnlyNumbers(phoneNumber).Substring(1, 10);
                    Friend friendUpdate;
                    try
                    {
                        friendUpdate = serviceFriends.FindUserByPhoneNumber(clearPhoneNumber);
                        friendUpdate.Voter = true;
                        friendUpdate.VotingDate = DateTime.Now.Date;

                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    try
                    {
                        serviceFriends.SaveFriends(friendUpdate);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }      
            }
            else
            {

            }
        }

        [HttpPost]
        [Route("requestqrcodesreceiving")]
        public async Task<IActionResult> RequestQRcodesReceiving([FromBody] DateTimeDTO dateTimeStr)
        {
            string url = "https://etsa.online/app/api/barcodes/get.php";
            ReportQrCodeExchange reportQrCodeExchange = new ReportQrCodeExchange();
            ResponseMessageDataQRCode responseData = null;
            string dateTimeNow;
            DateTime dateTimeFront;

            if (dateTimeStr != null && dateTimeStr.Date !=null && !dateTimeStr.Date.Equals("") && dateTimeStr.Time != null && !dateTimeStr.Time.Equals(""))
            {
                String[] date = dateTimeStr.Date.Split('-');
                string[] time = dateTimeStr.Time.Split(':');
                dateTimeFront = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), Convert.ToInt32(time[0]), Convert.ToInt32(time[1]),0);
                dateTimeNow = dateTimeFront.ToString("yyyy-MM-dd hh:mm:ss");
            }
            else
            {
                DateTime dateTimeUTC = DateTime.UtcNow;
                dateTimeNow = dateTimeUTC.AddHours(3).ToString("yyyy-MM-dd hh:mm:ss");
            }           


            Dictionary<string, string> requestMessageDict = new Dictionary<string, string>
            {
                ["token"] = "N2U1OThkZDZkZDliZGFiM/lAfhoRNk0D+iJh2Z1h1fpYEUWXkzsbvGg1",
                ["date"] = dateTimeNow
            };

            //try
            //{
            //    string jsonRequest = JsonSerializer.Serialize(requestMessage);
            //    string jsonResponseData = await PostRequestHttpAsync(url, jsonRequest);
            //    responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCode>(jsonResponseData);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            try
            {
                //string jsonResponseData = await PostRequestFormAsync(url, requestMessageDict);
                //responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCode>(jsonResponseData);

                responseData = await PostRequestFormAsync(url, requestMessageDict, "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (responseData != null)
            {
                if (responseData.status.Equals("200") || responseData.status.Equals("OK"))
                {
                    try
                    {
                        ResponseMessageDataQRCodeSuccessfully responseMessageDataQRCodeSuccessfully = (ResponseMessageDataQRCodeSuccessfully)responseData;
                        reportQrCodeExchange = CheckQrCodes(responseMessageDataQRCodeSuccessfully);
                    }catch
                    {
                        reportQrCodeExchange.error = responseData.error;
                        reportQrCodeExchange.status = responseData.status;
                        reportQrCodeExchange.dateTimeRequest = responseData.date;
                    }
                }
                else
                {
                    reportQrCodeExchange.error = responseData.error;
                    reportQrCodeExchange.status = responseData.status;
                    reportQrCodeExchange.dateTimeRequest = responseData.date;
                    return Ok(reportQrCodeExchange);
                }
            }

            return Ok(reportQrCodeExchange);
        }

        public async Task<string> PostRequestHttpAsync(string url, string json)
        {
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Add("Application", "remoteData");
            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            using HttpResponseMessage response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<string> PostRequestFormAsync(string url, IEnumerable<KeyValuePair<string, string>> data)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(data)
            };
            request.Headers.Add("Application", "remoteData");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<ResponseMessageDataQRCode> PostRequestFormAsync(string url, IEnumerable<KeyValuePair<string, string>> data, string str)
        {
            ResponseMessageDataQRCode responseData;

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(data)
            };
            request.Headers.Add("Application", "remoteData");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            string jsonResponseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                if (!jsonResponseData.Contains("[]"))
                {
                    responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCodeSuccessfully>(jsonResponseData);
                }
                else
                {
                    responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCodeError>(jsonResponseData);
                }
            }
            else {
                responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCodeError>(jsonResponseData);
            }
            return responseData;
        }



        public ReportQrCodeExchange CheckQrCodes(ResponseMessageDataQRCodeSuccessfully dataQRcodes)
        {
            ReportQrCodeExchange reportQrCodeExchange = new ReportQrCodeExchange
            {
                status = dataQRcodes.status,
                error = dataQRcodes.error,
                numberReceivedCodes = dataQRcodes.items.Count,
                dateTimeRequest = dataQRcodes.date.ToString(),
                notFoundQRcodes = new List<Item>()
            };

            int numberMarkedCodes = 0;
            int numberNotFound = 0;
            ServiceFriends serviceFriends = new ServiceFriends();

            foreach (KeyValuePair<string,string> keyValue in dataQRcodes.items)
            {
                if (keyValue.Key != null && keyValue.Key != "")
                {
                    if (CheckQrCode(keyValue.Key, serviceFriends))
                    {
                        numberMarkedCodes++;
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

        public bool CheckQrCode(string qrCodes, ServiceFriends serviceFriends)
        {
                Friend friendUpdate;
                try
                {
                    friendUpdate = serviceFriends.FindUserByQRtext(qrCodes);
                    friendUpdate.Voter = true;
                    friendUpdate.VotingDate = DateTime.Now.Date;
                }
                catch (Exception ex)
                {
                    return false;
                }

                try
                {
                    serviceFriends.SaveFriends(friendUpdate);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }            
        }

    }
}
