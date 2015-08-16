using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Orphee.RestApiManagement.Interfaces;

namespace Orphee.RestApiManagement
{
    public class FileUploader : IFileUploader
    {
        public async Task<bool> UploadFile(StorageFile fileToUpload)
        {
            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var response = await httpClient.GetAsync("api/upload/audio/x-midi"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    RestApiManagerBase.Instance.UserData.User.CreationGetPutKeyList.Add(fileToUpload.Name, JsonConvert.DeserializeObject<CreationUrls>(responseData));
                    var stringArray = RestApiManagerBase.Instance.UserData.User.CreationGetPutKeyList[fileToUpload.Name].GetUrl.Split('/');
                    if (!response.IsSuccessStatusCode || !await SendRequestToAws(RestApiManagerBase.Instance.UserData.User.CreationGetPutKeyList[fileToUpload.Name].PutUrl, fileToUpload) || 
                        !await SendRequestToRestApi(RestApiManagerBase.Instance.UserData.User.CreationGetPutKeyList[fileToUpload.Name].GetUrl, stringArray.Last()))
                        return false;
                }
            }
            return true;
        }

        private async Task<bool> SendRequestToRestApi(string getFileUri, string id)
        {
            var values = new Dictionary<string, string>()
            {
                { "url",getFileUri }
            };

            using (var httpClient = new HttpClient { BaseAddress = RestApiManagerBase.Instance.RestApiUrl })
            {
                using (var content = new FormUrlEncodedContent(values))
                {
                    using (var response = await httpClient.PutAsync("api/creation/" + id, content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
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
                        string responseData2 = await response.Content.ReadAsStringAsync();
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
