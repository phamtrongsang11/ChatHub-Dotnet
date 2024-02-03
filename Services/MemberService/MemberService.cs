using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.MemberService
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Member> Create(Member memberEntity)
        {
            var createdMember = _unitOfWork.memberRepository.Create(memberEntity);
            await _unitOfWork.Save();
            return createdMember;
        }

        public async Task<List<Member>> GetAll(string? includeProperties = null)
        {
            var members = await _unitOfWork.memberRepository.GetAll(includeProperties);
            return members.ToList();
        }

        public async Task<Member?> Get(
            Expression<Func<Member, bool>> filter,
            string? includeProperties = null
        )
        {
            Member? member = await _unitOfWork.memberRepository.Get(filter, includeProperties);
            return member;
        }

        public async Task<Member> Update(Member memberEntity)
        {
            var updatedMember = _unitOfWork.memberRepository.Update(memberEntity);
            await _unitOfWork.Save();
            return updatedMember;
        }

        public async Task<Boolean> isExist(string id)
        {
            Member? member = await _unitOfWork.memberRepository.Get(s => s.id == id);
            return member == null ? false : true;
        }

        public async Task<Boolean> checkIsExistByReference(string serverId, string profileId)
        {
            Member? member = await _unitOfWork.memberRepository.Get(
                s => (s.serverId == serverId && s.profileId == profileId)
            );
            return member == null ? false : true;
        }

        public async Task<Member> PartialUpdate(string id, Member memberEntity)
        {
            Member? existingMember = await _unitOfWork.memberRepository.Get(p => p.id == id);

            if (existingMember != null)
            {
                existingMember.id = memberEntity?.id ?? existingMember.id;
                existingMember.role = memberEntity?.role ?? existingMember.role;

                existingMember.profileId = memberEntity?.profileId ?? existingMember.profileId;

                existingMember.serverId = memberEntity?.serverId ?? existingMember.serverId;

                _unitOfWork.memberRepository.Update(existingMember);
                await _unitOfWork.Save();

                return existingMember;
            }

            return null;
        }

        public async Task<Boolean> Remove(Member memberEntity)
        {
            Boolean result = _unitOfWork.memberRepository.Remove(memberEntity);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }
    }
}
