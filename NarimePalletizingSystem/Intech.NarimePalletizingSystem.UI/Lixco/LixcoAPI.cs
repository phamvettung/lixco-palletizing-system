using Intech.NarimePalletizingSystem.UI.Responses;
using Intech.NarimePalletizingSystem.UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using Intech.NarimePalletizingSystem.UI.Logging;

namespace Intech.NarimePalletizingSystem.UI.Lixco
{
    public class LixcoAPI
    {
        public static async Task<ResponseMessage> Login(Account account)
        {
            try
            {
                ResponseMessage responseMessage = new ResponseMessage();
                string rawBody = JsonConvert.SerializeObject(account);
                StringContent queryString = new StringContent(rawBody, System.Text.Encoding.UTF8, "application/json");
                Logger.WriteLog(rawBody, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.SENDING);
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(new Uri("http://192.168.20.252:8180/."), queryString);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var rawMsgReceived = await response.Content.ReadAsStringAsync();
                        responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(rawMsgReceived);
                        Logger.WriteLog(rawMsgReceived, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.RECEIVING);
                    }
                }
                return responseMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog("Login Err: " + e.Message, Microsoft.Extensions.Logging.LogLevel.Error, LogAction.RECEIVING);
                throw e;
            }
        }

        public static async Task<ResponseMessage> GetTeamLix(Authorization authorization)
        {
            try
            {
                ResponseMessage responseMessage = null;
                Logger.WriteLog(string.Format("GetTeamLix tokenType:{0} accessToken: {1}", authorization.tokenType, authorization.accessToken), Microsoft.Extensions.Logging.LogLevel.Information, LogAction.SENDING);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization.tokenType + authorization.accessToken);
                    HttpResponseMessage response = await client.GetAsync(new Uri("http://192.168.20.252:8180/."));
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var rawMsgReceived = await response.Content.ReadAsStringAsync();
                        responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(rawMsgReceived);
                        Logger.WriteLog(rawMsgReceived, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.RECEIVING);
                    }
                }
                return responseMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog("GetTeamLix Err: " + e.Message, Microsoft.Extensions.Logging.LogLevel.Error, LogAction.RECEIVING);
                throw e;
            }
        }

        public static async Task<ResponseMessage> GetShiftLix(Authorization authorization)
        {
            try
            {
                ResponseMessage responseMessage = null;
                Logger.WriteLog(string.Format("GetShiftLix tokenType:{0} accessToken: {1}", authorization.tokenType, authorization.accessToken), Microsoft.Extensions.Logging.LogLevel.Information, LogAction.SENDING);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization.tokenType + authorization.accessToken);
                    HttpResponseMessage response = await client.GetAsync(new Uri("http://192.168.20.252:8180/."));
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var rawMsgReceived = await response.Content.ReadAsStringAsync();
                        responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(rawMsgReceived);
                        Logger.WriteLog(rawMsgReceived, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.RECEIVING);
                    }
                }
                return responseMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog("GetShiftLix Err: " + e.Message, Microsoft.Extensions.Logging.LogLevel.Error, LogAction.RECEIVING);
                throw e;
            }
        }

        public static async Task<ResponseMessage> GetLineLix(Authorization authorization)
        {
            try
            {
                ResponseMessage responseMessage = null;
                Logger.WriteLog(string.Format("GetLineLix tokenType:{0} accessToken: {1}", authorization.tokenType, authorization.accessToken), Microsoft.Extensions.Logging.LogLevel.Information, LogAction.SENDING);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization.tokenType + authorization.accessToken);
                    HttpResponseMessage response = await client.GetAsync(new Uri("http://192.168.20.252:8180/."));
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var rawMsgReceived = await response.Content.ReadAsStringAsync();
                        responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(rawMsgReceived);
                        Logger.WriteLog(rawMsgReceived, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.RECEIVING);
                    }
                }
                return responseMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog("GetShiftLix Err: " + e.Message, Microsoft.Extensions.Logging.LogLevel.Error, LogAction.RECEIVING);
                throw e;
            }
        }



        public static async Task<ResponseMessage> GetProductLix(Authorization authorization, string productCode)
        {
            try
            {
                ResponseMessage responseMessage = null;
                Logger.WriteLog(string.Format("GetProductLix tokenType:{0} accessToken: {1}", authorization.tokenType, authorization.accessToken), Microsoft.Extensions.Logging.LogLevel.Information, LogAction.SENDING);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization.tokenType + authorization.accessToken);
                    HttpResponseMessage response = await client.GetAsync(new Uri(string.Format("http://192.168.20.252:8180/.", productCode)));
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var rawMsgReceived = await response.Content.ReadAsStringAsync();
                        responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(rawMsgReceived);
                        Logger.WriteLog(rawMsgReceived, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.RECEIVING);
                    }
                }
                return responseMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog("GetProductLix Err: " + e.Message, Microsoft.Extensions.Logging.LogLevel.Error, LogAction.RECEIVING);
                throw e;
            }
        }

        public static async Task<ResponseMessage> SavePalletLix(Authorization authorization, PalletLixDTO palletLix)
        {
            try
            {
                ResponseMessage responseMessage = null;
                string rawBody = JsonConvert.SerializeObject(palletLix);
                StringContent queryString = new StringContent(rawBody, System.Text.Encoding.UTF8, "application/json");
                Logger.WriteLog(rawBody, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.SAVING);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorization.tokenType + authorization.accessToken);
                    HttpResponseMessage response = await client.PostAsync(new Uri("http://192.168.20.252:8180/."), queryString);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var rawMsgReceived = await response.Content.ReadAsStringAsync();
                        responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(rawMsgReceived);
                        Logger.WriteLog("SavePalletLix success " + rawMsgReceived, Microsoft.Extensions.Logging.LogLevel.Information, LogAction.RECEIVING);
                    }
                    else
                    {
                        Logger.WriteLog("SavePalletLix warning " + response.StatusCode.ToString() + " " + response.Content, Microsoft.Extensions.Logging.LogLevel.Warning, LogAction.RECEIVING);
                    }
                }
                return responseMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog("SavePalletLix Err: " + e.Message, Microsoft.Extensions.Logging.LogLevel.Error, LogAction.RECEIVING);
                throw e;
            }
        }
    }
}
