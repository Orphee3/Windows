using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Models;
using Orphee.RestApiManagement.Senders.Interfaces;

namespace Orphee.RestApiManagement.Senders
{
    public class FileUploader : IFileUploader
    {
        private string _newCreationId;
        private readonly INotificationSender _notifyer;
        public FileUploader(INotificationSender notifyer)
        {
            this._notifyer = notifyer;
        }
        public async Task<bool> UploadFile(StorageFile fileToUpload)
        {
            var createNewCreationEntryResult = await CreateNewCreationEntry(fileToUpload.Name);
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync("api/upload/audio/x-midi"))
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var urlPair = JsonConvert.DeserializeObject<CreationUrls>(responseData);
                    if (!createNewCreationEntryResult || !response.IsSuccessStatusCode || !await SendNewCreationRequestToAws(urlPair.PutUrl, fileToUpload) || !await UpdateNewCreationEntry(urlPair.GetUrl))
                        return false;
                    var result = await this._notifyer.SendNotification("creations", this._newCreationId);
                    if (!result)
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> CreateNewCreationEntry(string fileName)
        {
            var values = new Dictionary<string, string>()
            {
                { "name",  fileName },
                { "creator", RestApiManagerBase.Instance.UserData.User.Id },
            };

            using (var httpClient = new HttpClient {BaseAddress = RestApiManagerBase.Instance.RestApiUrl})
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = await httpClient.PostAsync("api/creation/", content))
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                        dynamic newCreation = JsonConvert.DeserializeObject<dynamic>(responseData);
                        RestApiManagerBase.Instance.UserData.User.Creations.Add(newCreation);
                        this._newCreationId = newCreation["_id"];
                    }
                }
            }
            return true;
        }

        private async Task<bool> UpdateNewCreationEntry(string getFileUri)
        {
            var values = new Dictionary<string, string>()
            {
                { "url", getFileUri }
            };
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = await httpClient.PutAsync("api/creation/" + this._newCreationId, content))
                    {
                        await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                    }
                }
            }
            return true;
        }

        private async Task<bool> SendNewCreationRequestToAws(string responseData, StorageFile fileToUpload)
        {
            using (var httpClient = new HttpClient())
            {
                var fileToArray = (await FileIO.ReadBufferAsync(fileToUpload)).ToArray();
                using (var content = new ByteArrayContent(fileToArray))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("audio/x-midi");
                    using (var response = await httpClient.PutAsync(responseData, content))
                    {
                        await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                    }
                }
            }
            return true;
        }

        public async Task<bool> UploadImage(StorageFile fileToUpload)
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync("api/upload/image/jpeg"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var urlPair = JsonConvert.DeserializeObject<CreationUrls>(responseData);
                    var sendRequestToAwsResponse = await SendImageRequestToAws(urlPair.PutUrl, fileToUpload);
                    var updateImageEntry = await UpdateImageEntry(urlPair.GetUrl);
                    if (!response.IsSuccessStatusCode || !sendRequestToAwsResponse || !updateImageEntry)
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> UpdateImageEntry(string getFileUri)
        {
            var values = new Dictionary<string, string>()
            {
                { "picture", getFileUri }
            };
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + RestApiManagerBase.Instance.UserData.Token);
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = await httpClient.PutAsync("api/user/" + RestApiManagerBase.Instance.UserData.User.Id, content))
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                        RestApiManagerBase.Instance.UserData.User.Picture = getFileUri;
                        RestApiManagerBase.Instance.UserData.User.PictureHasBeenUplaodedWithSuccess = true;
                    }
                }
            }
            return true;
        }

        private async Task<bool> SendImageRequestToAws(string responseData, StorageFile fileToUpload)
        {
            using (var httpClient = new HttpClient())
            {
                var imageToArray = (await FileIO.ReadBufferAsync(fileToUpload)).ToArray();
                using (var content = new ByteArrayContent(imageToArray))
                {
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                      
                    using (var response = await httpClient.PutAsync(responseData, content))
                    {
                        await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
