using CoreWebAPI.DAL;
using CoreWebAPI.Hubs;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class MessageController: ControllerBase
    {
        private IHubContext<MessageHub> _hub;
        private string liteDbPath = @$"{Directory.GetCurrentDirectory()}\Data\MessageDatabase.db";
        public MessageController(IHubContext<MessageHub> hub)
        {
            _hub = hub;
        }

        //public async Task<IActionResult> GetMessage()
        //{
        //    var data = _doStuff.GetData();
        //    await _hub.Clients.All.SendAsync("show_data", data);

        //    return "";
        //}

        /// <summary>
        /// Get All ClientMessages from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {

            using (var db = new LiteDatabase(liteDbPath))
            {
                try
                {
                    var t = await Task.Factory.StartNew(() =>
                    {
                        var messages = db.GetCollection<ClientMessage>();
                        return messages;
                        
                    });

                    if (t == null)
                        return NotFound();

                    return Ok(t.FindAll().ToArray());

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }        
        }

        /// <summary>
        /// Insrt new ClientMessage into Database
        /// </summary>
        /// <param name="messageText"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(template: "InsertMessage")]
        public async Task<IActionResult> SendMessages(string messageText)
        {

            using (var db = new LiteDatabase(liteDbPath))
            {
                try
                {
                    var t = await Task.Factory.StartNew(() =>
                    {
                        var messages = db.GetCollection<ClientMessage>();
                        ClientMessage message = new ClientMessage()
                        {
                            CreatedDate = DateTime.Now,
                            MessageText = messageText
                        };

                        messages.Insert(message);

                        return message;

                    });                    

                    return Ok(t);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
        }
    }
}
