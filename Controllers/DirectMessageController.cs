using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.DirectMessageContract;
using TeamChat.Models;
using TeamChat.Services.DirectMessageService;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/directmessages")]
    public class DirectMessageController : ControllerBase
    {
        private readonly IDirectMessageService _directMessageService;
        private readonly IMapper _mapper;
        private readonly string includeProp = "member,conversation,member.profile";

        public DirectMessageController(IDirectMessageService directMessageService, IMapper mapper)
        {
            _directMessageService = directMessageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<DirectMessageResponse>> Create(
            CreateDirectMessageRequest directRequest
        )
        {
            DirectMessage direct = _mapper.Map<DirectMessage>(directRequest);
            DirectMessage createdDirect = await _directMessageService.Create(direct);
            return CreatedAtAction(
                nameof(Create),
                _mapper.Map<DirectMessageResponse>(directRequest)
            );
        }

        [HttpGet]
        public async Task<ActionResult<List<DirectMessageResponse>>> GetAll()
        {
            List<DirectMessage> direct = await _directMessageService.GetAll(
                includeProperties: includeProp
            );
            List<DirectMessageResponse> directResponse = direct
                .Select(channel => _mapper.Map<DirectMessageResponse>(channel))
                .ToList();
            return Ok(directResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DirectMessageResponse>> Get(string id)
        {
            Boolean isExist = await _directMessageService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            DirectMessage? foundDirect = await _directMessageService.Get(
                s => s.id == id,
                includeProp
            );
            return Ok(_mapper.Map<DirectMessageResponse>(foundDirect));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DirectMessageResponse>> Update(
            string id,
            UpdateDirectMessageRequest directRequest
        )
        {
            Boolean isExist = await _directMessageService.isExist(id);
            if (!isExist)
                return NotFound();

            directRequest.id = id;

            DirectMessage direct = _mapper.Map<DirectMessage>(directRequest);

            DirectMessage updatedDirect = await _directMessageService.Update(direct);

            return Ok(_mapper.Map<DirectMessageResponse>(updatedDirect));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<DirectMessageResponse>> PartialUpdate(
            string id,
            UpdateDirectMessageRequest directRequest
        )
        {
            Boolean isExist = await _directMessageService.isExist(id);
            if (!isExist)
                return NotFound();

            DirectMessage direct = _mapper.Map<DirectMessage>(directRequest);

            DirectMessage updatedDirect = await _directMessageService.PartialUpdate(id, direct);

            return Ok(_mapper.Map<DirectMessageResponse>(updatedDirect));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            DirectMessage? existingDirect = await _directMessageService.Get(s => s.id == id);

            if (existingDirect == null)
                return NotFound();

            await _directMessageService.Remove(existingDirect);

            return NoContent();
        }

        [HttpGet("conversation")]
        public async Task<ActionResult<List<DirectMessageResponse>>> GetDirectMessageByChannelId(
            [FromQuery] string conversationId
        )
        {
            List<DirectMessage> directMessages =
                await _directMessageService.GetDirectMessagesByConversationId(conversationId);
            List<DirectMessageResponse> messResponse = directMessages
                .Select(channel => _mapper.Map<DirectMessageResponse>(channel))
                .ToList();
            return Ok(messResponse);
        }
    }
}
