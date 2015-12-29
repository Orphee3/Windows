using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Midi;
using Newtonsoft.Json.Linq;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
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

        public async Task<bool> SendNote(INoteToSend note, bool isNoteOn, Channel channel, int currentOctave)
        {
            var noteToSend = new NoteToSend()
            {
                Note = note.Note,
                Channel = channel,
                ColumnIndex = note.ColumnIndex,
                LineIndex = note.LineIndex,
                Octave = currentOctave,
            };
            try
            {
                var result = await Task.FromResult(this._socket.Emit("note", JObject.FromObject(new {note = noteToSend, isOn = isNoteOn})));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendMoreColumns()
        {
            try
            {
                var result = await Task.FromResult(this._socket.Emit("add columns"));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendCurrentPieceInfo(Dictionary<List<INoteToSend>, UserBase> actualPieceInfo)
        {
            try
            {
                var result = await Task.FromResult(this._socket.Emit("piece info", JObject.FromObject(new {pieceInfo = actualPieceInfo})));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendTempoModification(int newTempo)
        {
            try
            {
                var result = await Task.FromResult(this._socket.Emit("tempo", JObject.FromObject(new { tempo = newTempo })));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendPieceQuit()
        {
            try
            {
                var result = await Task.FromResult(this._socket.Emit("piece quit"));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
