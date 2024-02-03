using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TeamChat.Contracts.DirectMessageContract;
using TeamChat.Contracts.MemberContract;
using TeamChat.Models;
using TeamChat.Services.DirectMessageService;
using TeamChat.Services.MemberService;

namespace TeamChat.Hubs
{
    public class ChatDirectMessageHub : Hub
    {
        private readonly IDirectMessageService _directMessageService;
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public ChatDirectMessageHub(
            IDirectMessageService directMessageService,
            IMemberService memberService,
            IMapper mapper
        )
        {
            _directMessageService = directMessageService;
            _memberService = memberService;
            _mapper = mapper;
        }

        public async Task ConnectConversation(UserDirectMessageConnect userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.conversationId);
            await Clients
                .Group(userConnection.conversationId)
                .SendAsync("UserConnectedDirect", userConnection.profileId);
        }

        public async Task SendMessage(CreateDirectMessageRequest directRequest)
        {
            DirectMessage directMessage = _mapper.Map<DirectMessage>(directRequest);
            DirectMessage createdDirectMessage = await _directMessageService.Create(directMessage);

            Member? member = await _memberService.Get(
                m => m.id == createdDirectMessage.memberId,
                "profile"
            );

            DirectMessageResponse directResponse = _mapper.Map<DirectMessageResponse>(
                createdDirectMessage
            );

            MemberResponse memberResponse = _mapper.Map<MemberResponse>(member);

            await Clients
                .Group(directRequest.conversationId)
                .SendAsync("ReceiveMessage", memberResponse, directResponse);
        }

        public async Task UpdateMessage(UpdateDirectMessageRequest directRequest)
        {
            DirectMessage directMessage = _mapper.Map<DirectMessage>(directRequest);
            DirectMessage updatedMessage = await _directMessageService.PartialUpdate(
                directMessage.id,
                directMessage
            );

            Member? member = await _memberService.Get(
                m => m.id == updatedMessage.memberId,
                "profile"
            );

            DirectMessageResponse directResponse = _mapper.Map<DirectMessageResponse>(
                updatedMessage
            );

            MemberResponse memberResponse = _mapper.Map<MemberResponse>(member);

            await Clients
                .Group(directRequest.conversationId!)
                .SendAsync("ReceiveUpdateMessage", memberResponse, directResponse);
        }

        public async Task DeleteMessage(UpdateDirectMessageRequest directRequest)
        {
            DirectMessage? findDirectMessage = await _directMessageService.Get(
                m => m.id == directRequest.id
            );
            if (findDirectMessage == null || findDirectMessage.deleted == true)
                return;

            findDirectMessage.content = "This message has been deleted";
            findDirectMessage.fileUrl = null;
            findDirectMessage.deleted = true;

            DirectMessage directMessage = _mapper.Map<DirectMessage>(directRequest);
            DirectMessage updatedDirectMessage = await _directMessageService.PartialUpdate(
                findDirectMessage.id,
                findDirectMessage
            );

            Member? member = await _memberService.Get(
                m => m.id == updatedDirectMessage.memberId,
                "profile"
            );

            DirectMessageResponse directMessageResponse = _mapper.Map<DirectMessageResponse>(
                updatedDirectMessage
            );

            MemberResponse memberResponse = _mapper.Map<MemberResponse>(member);

            await Clients
                .Group(directRequest.conversationId!)
                .SendAsync("ReceiveUpdateMessage", memberResponse, directMessageResponse);
        }
    }
}
