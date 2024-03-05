using System.Net.Http;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.ChannelContact;
using TeamChat.Models;
using TeamChat.Services.ChannelService;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/channels")]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _channelService;
        private readonly IMapper _mapper;

        public ChannelController(IChannelService channelService, IMapper mapper)
        {
            _channelService = channelService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ChannelResponse>> Create(CreateChannelRequest channelRequest)
        {
            Channel channel = _mapper.Map<Channel>(channelRequest);
            Channel createdChannel = await _channelService.Create(channel);
            return CreatedAtAction(nameof(Create), _mapper.Map<ChannelResponse>(createdChannel));
        }

        [HttpGet]
        public async Task<ActionResult<List<ChannelResponse>>> GetAll()
        {
            List<Channel> channels = await _channelService.GetAll(
                includeProperties: "server,profile"
            );
            List<ChannelResponse> channelResponse = channels
                .Select(channel => _mapper.Map<ChannelResponse>(channel))
                .ToList();
            return Ok(channelResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelResponse>> Get(string id)
        {
            Boolean isExist = await _channelService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            Channel? foundChannel = await _channelService.Get(s => s.id == id, "server,profile");
            return Ok(_mapper.Map<ChannelResponse>(foundChannel));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChannelResponse>> Update(
            string id,
            UpdateChannelRequest channelRequest
        )
        {
            Boolean isExist = await _channelService.isExist(id);
            if (!isExist)
                return NotFound();

            channelRequest.id = id;

            Channel channel = _mapper.Map<Channel>(channelRequest);

            Channel updatedChannel = await _channelService.Update(channel);

            return Ok(_mapper.Map<ChannelResponse>(updatedChannel));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ChannelResponse>> PartialUpdate(
            string id,
            UpdateChannelRequest channelRequest
        )
        {
            Boolean isExist = await _channelService.isExist(id);
            if (!isExist)
                return NotFound();

            Channel channel = _mapper.Map<Channel>(channelRequest);

            Channel updatedChannel = await _channelService.PartialUpdate(id, channel);

            return Ok(_mapper.Map<ChannelResponse>(updatedChannel));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Channel? existingChannel = await _channelService.Get(s => s.id == id);

            if (existingChannel == null)
                return NotFound();

            await _channelService.Remove(existingChannel);

            return NoContent();
        }

        [HttpGet("token")]
        public async Task<ActionResult<String>> GetTokenLiveKit(
            [FromQuery] string user,
            [FromQuery] string room
        )
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = HttpContext.Items["token"] as string;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                accessToken
            );

            string apiUrl =
                "https://chatterbox-nestjs.onrender.com/livekit?identity="
                + user
                + "&chatId="
                + room;

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            string responseData = await response.Content.ReadAsStringAsync();

            return Ok(responseData);
        }
    }
}
