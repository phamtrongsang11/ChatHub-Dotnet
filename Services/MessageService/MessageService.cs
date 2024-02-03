using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string includeProp = "member,channel,member.profile,channel.profile";
        private readonly int PAGE = 10;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> Create(Message messEntity)
        {
            var createdMess = _unitOfWork.messageRepository.Create(messEntity);
            await _unitOfWork.Save();
            return createdMess;
        }

        public async Task<List<Message>> GetAll(string? includeProperties = null)
        {
            var messages = await _unitOfWork.messageRepository.GetAll(includeProperties);
            return messages.ToList();
        }

        public async Task<Message?> Get(
            Expression<Func<Message, bool>> filter,
            string? includeProperties = null
        )
        {
            Message? message = await _unitOfWork.messageRepository.Get(filter, includeProperties);
            return message;
        }

        public async Task<Message> Update(Message messEntity)
        {
            var updatedMess = _unitOfWork.messageRepository.Update(messEntity);
            await _unitOfWork.Save();
            return updatedMess;
        }

        public async Task<Boolean> isExist(string id)
        {
            Message? message = await _unitOfWork.messageRepository.Get(s => s.id == id);
            return message == null ? false : true;
        }

        public async Task<Message> PartialUpdate(string id, Message messEntity)
        {
            Message? existingMess = await _unitOfWork.messageRepository.Get(p => p.id == id);

            if (existingMess != null)
            {
                existingMess.id = messEntity?.id ?? existingMess.id;
                existingMess.content = messEntity?.content ?? existingMess.content;

                existingMess.fileUrl = messEntity?.fileUrl ?? existingMess.fileUrl;

                existingMess.deleted = messEntity?.deleted ?? existingMess.deleted;

                existingMess.memberId = messEntity?.memberId ?? existingMess.memberId;

                existingMess.channelId = messEntity?.channelId ?? existingMess.channelId;

                _unitOfWork.messageRepository.Update(existingMess);
                await _unitOfWork.Save();

                return existingMess;
            }

            return null;
        }

        public async Task<Boolean> Remove(Message messEntity)
        {
            Boolean result = _unitOfWork.messageRepository.Remove(messEntity);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }

        public async Task<List<Message>> GetMessagesByChannelId(int cursor, string id)
        {
            var messages = await _unitOfWork.messageRepository.GetAll(
                includeProp,
                cursor,
                page: PAGE
            );
            List<Message> messagesByChannel = new List<Message>();

            foreach (Message message in messages)
            {
                if (message.channel != null && message.channelId == id)
                {
                    messagesByChannel.Add(message);
                }
            }

            return messagesByChannel.ToList();
        }
    }
}
