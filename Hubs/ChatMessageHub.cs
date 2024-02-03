using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TeamChat.Contracts.MemberContract;
using TeamChat.Contracts.MessageContract;
using TeamChat.Models;
using TeamChat.Services.MemberService;
using TeamChat.Services.MessageService;

namespace TeamChat.Hubs
{
    public class ChatMessageHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public ChatMessageHub(
            IMessageService messageService,
            IMemberService memberService,
            IMapper mapper
        )
        {
            _messageService = messageService;
            _memberService = memberService;
            _mapper = mapper;
        }

        public async Task ConnectChannel(UserMessageConnect userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.channelId);
            await Clients
                .Group(userConnection.channelId)
                .SendAsync("UserConnected", userConnection.profileId);
        }

        public async Task SendMessage(CreateMessageRequest messageRequest)
        {
            Message message = _mapper.Map<Message>(messageRequest);
            Message createdMessage = await _messageService.Create(message);

            Member? member = await _memberService.Get(
                m => m.id == createdMessage.memberId,
                "profile"
            );

            MessageResponse messageResponse = _mapper.Map<MessageResponse>(createdMessage);

            MemberResponse memberResponse = _mapper.Map<MemberResponse>(member);

            await Clients
                .Group(messageRequest.channelId)
                .SendAsync("ReceiveMessage", memberResponse, messageResponse);
        }

        public async Task UpdateMessage(UpdateMessageRequest messageRequest)
        {
            Message message = _mapper.Map<Message>(messageRequest);
            Message updatedMessage = await _messageService.PartialUpdate(message.id, message);

            Member? member = await _memberService.Get(
                m => m.id == updatedMessage.memberId,
                "profile"
            );

            MessageResponse messageResponse = _mapper.Map<MessageResponse>(updatedMessage);

            MemberResponse memberResponse = _mapper.Map<MemberResponse>(member);

            await Clients
                .Group(messageRequest.channelId!)
                .SendAsync("ReceiveUpdateMessage", memberResponse, messageResponse);
        }

        public async Task DeleteMessage(UpdateMessageRequest messageRequest)
        {
            Message? findMessage = await _messageService.Get(m => m.id == messageRequest.id);
            if (findMessage == null || findMessage.deleted == true)
                return;

            messageRequest.content = "This message has been deleted";
            messageRequest.fileUrl = null;
            messageRequest.deleted = true;

            Message message = _mapper.Map<Message>(messageRequest);
            Message updatedMessage = await _messageService.PartialUpdate(message.id, message);

            Member? member = await _memberService.Get(
                m => m.id == updatedMessage.memberId,
                "profile"
            );

            MessageResponse messageResponse = _mapper.Map<MessageResponse>(updatedMessage);

            MemberResponse memberResponse = _mapper.Map<MemberResponse>(member);

            await Clients
                .Group(messageRequest.channelId!)
                .SendAsync("ReceiveUpdateMessage", memberResponse, messageResponse);
        }
    }
}
