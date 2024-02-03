using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.ServerService
{
    public class ServerService : IServerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string includeProp =
            "profile,channels,members,members.profile,channels.profile";

        public ServerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Server> Create(Server serverEntity)
        {
            var createdServer = _unitOfWork.serverRepository.Create(serverEntity);
            if (createdServer != null)
            {
                Member member = new Member
                {
                    role = MemberRole.ADMIN,
                    profileId = serverEntity.profileId,
                    serverId = createdServer.id,
                };
                _unitOfWork.memberRepository.Create(member);

                Channel channel = new Channel
                {
                    name = "general",
                    type = ChannelType.TEXT,
                    profileId = serverEntity.profileId,
                    serverId = createdServer.id,
                };
                _unitOfWork.channelRepository.Create(channel);
            }
            await _unitOfWork.Save();

            return createdServer;
        }

        public async Task<List<Server>> GetAll(string? includeProperties = null)
        {
            var servers = await _unitOfWork.serverRepository.GetAll(includeProperties);
            return servers.ToList();
        }

        public async Task<Server?> Get(
            Expression<Func<Server, bool>> filter,
            string? includeProperties = null
        )
        {
            Server? server = await _unitOfWork.serverRepository.Get(filter, includeProperties);
            return server;
        }

        public async Task<Server> Update(Server serverEntiry)
        {
            var updatedServer = _unitOfWork.serverRepository.Update(serverEntiry);
            await _unitOfWork.Save();
            return updatedServer;
        }

        public async Task<Boolean> isExist(string id)
        {
            Server? server = await _unitOfWork.serverRepository.Get(s => s.id == id);
            return server == null ? false : true;
        }

        public async Task<Server> PartialUpdate(string id, Server serverEntity)
        {
            Server? existingServer = await _unitOfWork.serverRepository.Get(s => s.id == id);

            if (existingServer != null)
            {
                existingServer.name = serverEntity?.name ?? existingServer.name;
                existingServer.inviteCode = serverEntity?.inviteCode ?? existingServer.inviteCode;
                existingServer.imageUrl = serverEntity?.imageUrl ?? existingServer.imageUrl;
                existingServer.profileId = serverEntity?.profileId ?? existingServer.profileId;

                _unitOfWork.serverRepository.Update(existingServer);
                await _unitOfWork.Save();

                return existingServer;
            }

            return null;
        }

        public async Task<Boolean> Remove(Server serverEntiry)
        {
            Boolean result = _unitOfWork.serverRepository.Remove(serverEntiry);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }

        public async Task<Server?> FindFirstServerByProfileId(String id)
        {
            Server? server = await _unitOfWork.serverRepository.Get(
                s => s.profileId == id,
                includeProp
            );
            return server;
        }

        public async Task<List<Server>> GetServersByMemberId(String id)
        {
            var servers = await _unitOfWork.serverRepository.GetAll(includeProp);
            List<Server> serversByMember = new List<Server>();

            foreach (Server server in servers)
            {
                if (server.members != null && server.members.Any(m => m.profileId == id))
                {
                    serversByMember.Add(server);
                }
            }

            return serversByMember.ToList();
        }

        public async Task<Server?> FindServerByInviteCode(String inviteCode)
        {
            Server? server = await _unitOfWork.serverRepository.Get(
                s => s.inviteCode == inviteCode,
                includeProp
            );
            return server;
        }

        public async Task<Server?> SaveMemberByInviteCode(string inviteCode, Member memberEntity)
        {
            Server? server = await _unitOfWork.serverRepository.Get(
                s => s.inviteCode == inviteCode
            );
            if (server != null)
            {
                memberEntity.serverId = server.id;
                Member savedMember = _unitOfWork.memberRepository.Create(memberEntity);
                if (savedMember != null)
                {
                    await _unitOfWork.Save();
                }
            }
            return server;
        }
    }
}
