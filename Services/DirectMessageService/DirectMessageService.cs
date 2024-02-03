using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.DirectMessageService
{
    public class DirectMessageService : IDirectMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string includeProp = "member,conversation,member.profile";

        public DirectMessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectMessage> Create(DirectMessage directEntity)
        {
            var createdDirect = _unitOfWork.directMessageRepository.Create(directEntity);
            await _unitOfWork.Save();
            return createdDirect;
        }

        public async Task<List<DirectMessage>> GetAll(string? includeProperties = null)
        {
            var directs = await _unitOfWork.directMessageRepository.GetAll(includeProperties);
            return directs.ToList();
        }

        public async Task<DirectMessage?> Get(
            Expression<Func<DirectMessage, bool>> filter,
            string? includeProperties = null
        )
        {
            DirectMessage? direct = await _unitOfWork.directMessageRepository.Get(
                filter,
                includeProperties
            );
            return direct;
        }

        public async Task<DirectMessage> Update(DirectMessage directEntity)
        {
            var updatedDirect = _unitOfWork.directMessageRepository.Update(directEntity);
            await _unitOfWork.Save();
            return updatedDirect;
        }

        public async Task<Boolean> isExist(string id)
        {
            DirectMessage? direct = await _unitOfWork.directMessageRepository.Get(s => s.id == id);
            return direct == null ? false : true;
        }

        public async Task<DirectMessage> PartialUpdate(string id, DirectMessage directEntity)
        {
            DirectMessage? existingDirect = await _unitOfWork.directMessageRepository.Get(
                p => p.id == id
            );

            if (existingDirect != null)
            {
                existingDirect.id = directEntity?.id ?? existingDirect.id;
                existingDirect.content = directEntity?.content ?? existingDirect.content;

                existingDirect.fileUrl = directEntity?.fileUrl ?? existingDirect.fileUrl;

                existingDirect.deleted = directEntity?.deleted ?? existingDirect.deleted;

                existingDirect.memberId = directEntity?.memberId ?? existingDirect.memberId;

                existingDirect.conversationId =
                    directEntity?.conversationId ?? existingDirect.conversationId;

                _unitOfWork.directMessageRepository.Update(existingDirect);
                await _unitOfWork.Save();

                return existingDirect;
            }

            return null;
        }

        public async Task<Boolean> Remove(DirectMessage directEntity)
        {
            Boolean result = _unitOfWork.directMessageRepository.Remove(directEntity);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }

        public async Task<List<DirectMessage>> GetDirectMessagesByConversationId(string id)
        {
            var directMessages = await _unitOfWork.directMessageRepository.GetAll(includeProp);
            var directMessagesByChannel = new List<DirectMessage>();

            foreach (DirectMessage message in directMessages)
            {
                if (message.conversation != null && message.conversationId == id)
                {
                    directMessagesByChannel.Add(message);
                }
            }

            return directMessagesByChannel.ToList();
        }
    }
}
