using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.ConversationService
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConversationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Conversation> Create(Conversation converEntity)
        {
            var createdConver = _unitOfWork.conversationRepository.Create(converEntity);
            await _unitOfWork.Save();
            return createdConver;
        }

        public async Task<List<Conversation>> GetAll(string? includeProperties = null)
        {
            var convers = await _unitOfWork.conversationRepository.GetAll(includeProperties);
            return convers.ToList();
        }

        public async Task<Conversation?> Get(
            Expression<Func<Conversation, bool>> filter,
            string? includeProperties = null
        )
        {
            Conversation? conver = await _unitOfWork.conversationRepository.Get(
                filter,
                includeProperties
            );
            return conver;
        }

        public async Task<Conversation> Update(Conversation converEntity)
        {
            var updatedConver = _unitOfWork.conversationRepository.Update(converEntity);
            await _unitOfWork.Save();
            return updatedConver;
        }

        public async Task<Boolean> isExist(string id)
        {
            Conversation? conver = await _unitOfWork.conversationRepository.Get(s => s.id == id);
            return conver == null ? false : true;
        }

        public async Task<Conversation> PartialUpdate(string id, Conversation converEntity)
        {
            Conversation? existingConver = await _unitOfWork.conversationRepository.Get(
                p => p.id == id
            );

            if (existingConver != null)
            {
                existingConver.id = converEntity?.id ?? existingConver.id;
                existingConver.memberOneId =
                    converEntity?.memberOneId ?? existingConver.memberOneId;

                existingConver.memberTwoId =
                    converEntity?.memberTwoId ?? existingConver.memberTwoId;

                _unitOfWork.conversationRepository.Update(existingConver);
                await _unitOfWork.Save();

                return existingConver;
            }

            return null;
        }

        public async Task<Boolean> Remove(Conversation converEntity)
        {
            Boolean result = _unitOfWork.conversationRepository.Remove(converEntity);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }
    }
}
