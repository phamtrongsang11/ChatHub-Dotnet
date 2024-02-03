using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.MessageContract;
using TeamChat.Models;
using TeamChat.Services.MessageService;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly string includeProp = "member,channel,member.profile,channel.profile";

        public MessageController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse>> Create(CreateMessageRequest messRequest)
        {
            Message message = _mapper.Map<Message>(messRequest);
            Message createdMessage = await _messageService.Create(message);
            return CreatedAtAction(nameof(Create), _mapper.Map<MessageResponse>(createdMessage));
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageResponse>>> GetAll()
        {
            List<Message> messages = await _messageService.GetAll(includeProperties: includeProp);
            List<MessageResponse> messResponse = messages
                .Select(channel => _mapper.Map<MessageResponse>(channel))
                .ToList();
            return Ok(messResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MessageResponse>> Get(string id)
        {
            Boolean isExist = await _messageService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            Message? foundMessage = await _messageService.Get(s => s.id == id, includeProp);
            return Ok(_mapper.Map<MessageResponse>(foundMessage));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MessageResponse>> Update(
            string id,
            UpdateMessageRequest messRequest
        )
        {
            Boolean isExist = await _messageService.isExist(id);
            if (!isExist)
                return NotFound();

            messRequest.id = id;

            Message message = _mapper.Map<Message>(messRequest);

            Message updatedMessage = await _messageService.Update(message);

            return Ok(_mapper.Map<MessageResponse>(updatedMessage));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<MessageResponse>> PartialUpdate(
            string id,
            UpdateMessageRequest messRequest
        )
        {
            Boolean isExist = await _messageService.isExist(id);
            if (!isExist)
                return NotFound();

            Message message = _mapper.Map<Message>(messRequest);

            Message updatedMessage = await _messageService.PartialUpdate(id, message);

            return Ok(_mapper.Map<MessageResponse>(updatedMessage));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Message? existingMessage = await _messageService.Get(s => s.id == id);

            if (existingMessage == null)
                return NotFound();

            await _messageService.Remove(existingMessage);

            return NoContent();
        }

        [HttpGet("channel")]
        public async Task<ActionResult<List<MessageResponse>>> GetMessageByChannelId(
            [FromQuery] int cursor,
            [FromQuery] string channelId
        )
        {
            List<Message> messages = await _messageService.GetMessagesByChannelId(
                cursor,
                channelId
            );
            List<MessageResponse> messResponse = messages
                .Select(channel => _mapper.Map<MessageResponse>(channel))
                .ToList();
            return Ok(messResponse);
        }
    }
}
