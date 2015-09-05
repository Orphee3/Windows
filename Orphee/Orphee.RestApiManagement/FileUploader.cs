using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces; 

namespace Orphee.RestApiManagement
{
    public class FileUploader : IFileUploader
    {
        private string _newCreationId;
        public async Task<bool> UploadFile(StorageFile fileToUpload)
        {
            var createNewCreationEntryResult = await CreateNewCreationEntry(fileToUpload.Name);
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync("api/upload/audio/x-midi"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var urlPair = JsonConvert.DeserializeObject<CreationUrls>(responseData);
                    if (!createNewCreationEntryResult || !response.IsSuccessStatusCode || !await SendRequestToAws(urlPair.PutUrl, fileToUpload) || !await UpdateNewCreationEntry(urlPair.GetUrl))
                        return false;
                }
            }
            return true;
        }

        public async Task<bool> CreateNewCreationEntry(string fileName)
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

        private async Task<bool> SendRequestToAws(string responseData, StorageFile fileToUpload)
        {
            using (var httpClient = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var byteArray = await ConvertFileToByteArray(fileToUpload);
                    content.Headers.ContentType = new MediaTypeHeaderValue("audio/x-midi");
                    content.Add(new StreamContent(new MemoryStream(byteArray)));
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

        private async Task<byte[]> ConvertFileToByteArray(StorageFile file)
        {
            byte[] fileBytes = null;
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            return fileBytes;
        }
    }
}
