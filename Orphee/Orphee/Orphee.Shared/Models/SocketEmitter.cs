using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Orphee.RestApiManagement.Models;
using Quobject.SocketIoClientDotNet.Client;

namespace Orphee.Models
{
    public class SocketEmitter
    {
        private readonly Socket _socket;

        public SocketEmitter(Socket socket)
        {
            this._socket = socket;
        }

        public async Task<bool> SendMessage(string messageToSend, string target, bool isPrivate)
        {
            try
            {
                var result = await Task.FromResult(this._socket.Emit(isPrivate ? "private message" : "group message", JObject.FromObject(new {to = target, message = messageToSend})));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CreateGroupChat(List<string> userList)
        {
            try
            {
                var result = await Task.FromResult(this._socket.Emit("create chat group", JObject.FromObject(new { people = userList })));
                while (RestApiManagerBase.Instance.UserData.User.ConversationList.Last().Id == null)
                    await Task.Delay(1);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
