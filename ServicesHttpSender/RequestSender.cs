using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using voteCollector.DTO;
using voteCollector.IServices;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.ModelsHttpSender
{
    public class RequestSender : IRequestSender
    {

        private readonly HttpClient httpClient;
        private ServicesQRcodes _servicesQRcodes;
        public RequestSender()
        {
            httpClient = new HttpClient();
            _servicesQRcodes = new ServicesQRcodes();
        }

        public Task<ReportQrCodeExchange> RequestQRcodesReceiving(string url, Dictionary<string, string> requestDataDict)
        {
            ReportQrCodeExchange reportQrCodeExchange = new ReportQrCodeExchange();
            ResponseMessageDataQRCode responseData = null;
            try
            {
                responseData =  SendRequestAsync(url, requestDataDict, "").Result;
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
                        reportQrCodeExchange = _servicesQRcodes.CheckQrCodes(responseMessageDataQRCodeSuccessfully);
                    }
                    catch
                    {
                        ResponseMessageDataQRCodeError responseMessageDataQRCodeError = (ResponseMessageDataQRCodeError)responseData;

                        foreach (string str in responseMessageDataQRCodeError.items)
                        {
                            responseMessageDataQRCodeError.items.Append(str);
                        }
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
                    return Task.FromResult(reportQrCodeExchange);
                }
            }

            return Task.FromResult(reportQrCodeExchange);
        }
        public async Task<ResponseMessageDataQRCode> SendRequestAsync(string url, IEnumerable<KeyValuePair<string, string>> data, string str)
        {
            ResponseMessageDataQRCode responseData = null;

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(data)
            };
            request.Headers.Add("Application", "remoteData");
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            string jsonResponseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {

                if (FindItemsToken(jsonResponseData) == JsonTokenType.StartObject) //indxF==-1 && indxL==-1 && indxF>=indxL
                {
                    responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCodeSuccessfully>(jsonResponseData);
                }
                else if (FindItemsToken(jsonResponseData) == JsonTokenType.StartArray)
                {
                    responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCodeError>(jsonResponseData);
                }
            }
            else
            {
                responseData = JsonSerializer.Deserialize<ResponseMessageDataQRCodeError>(jsonResponseData);
            }
            return await Task.FromResult(responseData);
        }

        //Проверка типа объекта в JSON после имени свойства "items"
        private static JsonTokenType FindItemsToken(string json)
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "items")
                    break;
            }
            if (!reader.Read()) return JsonTokenType.None;

            return reader.TokenType;
        }        

    }
}
