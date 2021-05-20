using ChargingPointApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChargingPointApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        // GET: api/<TransactionsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TransactionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TransactionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TransactionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TransactionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
        
        private Uri _url { get; set; }

        
        [HttpPost("start")]
        public async Task<string> StartTransCP(CStransactionStart csStart)
        {
            ClientWebSocket socket = new ClientWebSocket();
            var responseString = "";

            if (_url == null)
            {
                Random rnd = new Random();


                Random _rnd = new Random();
                
                var _random = _rnd.Next(1, 3);
                if (_random == 1)
                {
                    var _rnd1 = rnd.Next(10, 10000);
                    Uri _uri = new Uri($"ws://127.0.0.1:5000" + "/WP-" + _rnd1);
                    _url = _uri;
                }
                if (_random == 2)
                {
                    var _rnd2 = rnd.Next(10001, 20000);
                    Uri _uri = new Uri($"ws://127.0.0.1:5000" + "/WP-" + _rnd2);
                    _url = _uri;
                }
                if (_random == 3)
                {
                    var _rnd3 = rnd.Next(20001, 30000);
                    Uri _uri = new Uri($"ws://127.0.0.1:5000" + "/WP-" + _rnd3);
                    _url = _uri;
                }
 
            }



            await socket.ConnectAsync(_url, cancellationToken: CancellationToken.None);

            List<string> list = new List<string>();

            list.Add(csStart.Message);
            list.Add(csStart.SocketId);
            var json = JsonConvert.SerializeObject(list);

            var data = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(data, WebSocketMessageType.Text, true, cancellationToken: CancellationToken.None);

            while (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[4096 * 2];

                var responseBytes = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                responseString = Encoding.UTF8.GetString(buffer, 0, responseBytes.Count);
                if (responseString != null)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
                    return responseString;
                }
            }

            responseString = "Bad connection: CSMS didn't receive your message";
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
            return responseString;
        }

        [HttpPost("stop")]
        public async Task<string> StopTransCP(CStransactionStop csStop)
        {
            ClientWebSocket socket = new ClientWebSocket();

            var responseString = "";


            if (_url == null)
            {
                Random rnd = new Random();


                Random _rnd = new Random();

                var _random = _rnd.Next(1, 3);
                if (_random == 1)
                {
                    var _rnd1 = rnd.Next(10, 10000);
                    Uri _uri = new Uri($"ws://127.0.0.1:5000" + "/WP-" + _rnd1);
                    _url = _uri;
                }
                if (_random == 2)
                {
                    var _rnd2 = rnd.Next(10001, 20000);
                    Uri _uri = new Uri($"ws://127.0.0.1:5000" + "/WP-" + _rnd2);
                    _url = _uri;
                }
                if (_random == 3)
                {
                    var _rnd3 = rnd.Next(20001, 30000);
                    Uri _uri = new Uri($"ws://127.0.0.1:5000" + "/WP-" + _rnd3);
                    _url = _uri;
                }

            }



            await socket.ConnectAsync(_url, cancellationToken: CancellationToken.None);

            List<string> list = new List<string>();
            list.Add(csStop.Message);
            list.Add(csStop.SocketId);
            list.Add(csStop.TransactionId);

            var json = JsonConvert.SerializeObject(list);

            var data = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(data, WebSocketMessageType.Text, true, cancellationToken: CancellationToken.None);

            while (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[4096 * 2];

                var responseBytes = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                responseString = Encoding.UTF8.GetString(buffer, 0, responseBytes.Count);
                if (responseString != null)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
                    return responseString;
                }
                
            }

            responseString = "Bad connection: CSMS didn't receive your message";
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
            return responseString;
        }
    }
}
