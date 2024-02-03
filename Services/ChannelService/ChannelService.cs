using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.ChannelService
{
    public class ChannelService : IChannelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChannelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Channel> Create(Channel channelEntity)
        {
            var createdChannel = _unitOfWork.channelRepository.Create(channelEntity);
            await _unitOfWork.Save();
            return createdChannel;
        }

        public async Task<List<Channel>> GetAll(string? includeProperties = null)
        {
            var channels = await _unitOfWork.channelRepository.GetAll(includeProperties);
            return channels.ToList();
        }

        public async Task<Channel?> Get(
            Expression<Func<Channel, bool>> filter,
            string? includeProperties = null
        )
        {
            Channel? channel = await _unitOfWork.channelRepository.Get(filter, includeProperties);
            return channel;
        }

        public async Task<Channel> Update(Channel channelEntity)
        {
            var updatedChannel = _unitOfWork.channelRepository.Update(channelEntity);
            await _unitOfWork.Save();
            return updatedChannel;
        }

        public async Task<Boolean> isExist(string id)
        {
            Channel? channel = await _unitOfWork.channelRepository.Get(s => s.id == id);
            return channel == null ? false : true;
        }

        public async Task<Channel> PartialUpdate(string id, Channel channelEntity)
        {
            Channel? existingChannel = await _unitOfWork.channelRepository.Get(p => p.id == id);

            if (existingChannel != null)
            {
                existingChannel.id = channelEntity?.id ?? existingChannel.id;
                existingChannel.name = channelEntity?.name ?? existingChannel.name;
                existingChannel.type = channelEntity?.type ?? existingChannel.type;

                _unitOfWork.channelRepository.Update(existingChannel);
                await _unitOfWork.Save();

                return existingChannel;
            }

            return null;
        }

        public async Task<Boolean> Remove(Channel channelEntity)
        {
            Boolean result = _unitOfWork.channelRepository.Remove(channelEntity);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }
    }
}
