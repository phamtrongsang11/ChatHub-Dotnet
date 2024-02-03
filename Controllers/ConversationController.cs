using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.ConversationContract;
using TeamChat.Models;
using TeamChat.Services.ConversationService;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/conversations")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IMapper _mapper;

        public ConversationController(IConversationService conversationService, IMapper mapper)
        {
            _conversationService = conversationService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ConversationResponse>> Create(
            CreateConversationRequest converRequest
        )
        {
            Conversation conver = _mapper.Map<Conversation>(converRequest);
            Conversation createdConver = await _conversationService.Create(conver);
            return CreatedAtAction(
                nameof(Create),
                _mapper.Map<ConversationResponse>(createdConver)
            );
        }

        [HttpGet]
        public async Task<ActionResult<List<ConversationResponse>>> GetAll()
        {
            List<Conversation> conversations = await _conversationService.GetAll(
                includeProperties: "memberOne,memberTwo"
            );
            List<ConversationResponse> converResponse = conversations
                .Select(channel => _mapper.Map<ConversationResponse>(channel))
                .ToList();
            return Ok(converResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationResponse>> Get(string id)
        {
            Boolean isExist = await _conversationService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            Conversation? foundConver = await _conversationService.Get(
                s => s.id == id,
                "memberOne,memberTwo"
            );
            return Ok(_mapper.Map<ConversationResponse>(foundConver));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ConversationResponse>> Update(
            string id,
            UpdateConversationRequest converRequest
        )
        {
            Boolean isExist = await _conversationService.isExist(id);
            if (!isExist)
                return NotFound();

            converRequest.id = id;

            Conversation conver = _mapper.Map<Conversation>(converRequest);

            Conversation updatedConver = await _conversationService.Update(conver);

            return Ok(_mapper.Map<ConversationResponse>(updatedConver));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ConversationResponse>> PartialUpdate(
            string id,
            UpdateConversationRequest convRequest
        )
        {
            Boolean isExist = await _conversationService.isExist(id);
            if (!isExist)
                return NotFound();

            Conversation conversation = _mapper.Map<Conversation>(convRequest);

            Conversation updatedConver = await _conversationService.PartialUpdate(id, conversation);

            return Ok(_mapper.Map<ConversationResponse>(updatedConver));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Conversation? existingConver = await _conversationService.Get(s => s.id == id);

            if (existingConver == null)
                return NotFound();

            await _conversationService.Remove(existingConver);

            return NoContent();
        }

        [HttpPost("member")]
        public async Task<ActionResult<ConversationResponse>> GetOrCreate(
            CreateConversationRequest converRequest
        )
        {
            Conversation conversation = _mapper.Map<Conversation>(converRequest);

            Conversation? foundConversation = await _conversationService.Get(
                c =>
                    (
                        c.memberOneId == conversation.memberOneId
                        && c.memberTwoId == conversation.memberTwoId
                    )
                    || (
                        c.memberOneId == conversation.memberTwoId
                        && c.memberTwoId == conversation.memberOneId
                    )
            );

            if (foundConversation == null)
            {
                foundConversation = await _conversationService.Create(conversation);
            }

            foundConversation = await _conversationService.Get(
                c => c.id == foundConversation.id,
                "memberOne,memberTwo,memberOne.profile,memberTwo.profile"
            );

            return Ok(_mapper.Map<ConversationResponse>(foundConversation));
        }
    }
}
