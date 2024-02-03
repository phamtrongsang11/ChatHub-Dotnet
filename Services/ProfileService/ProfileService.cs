using System.Linq.Expressions;
using TeamChat.Models;
using TeamChat.Repositories.UnitOfWork;

namespace TeamChat.Services.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Profile> Create(Profile profileEntity)
        {
            var createdProfile = _unitOfWork.profileRepository.Create(profileEntity);
            await _unitOfWork.Save();
            return createdProfile;
        }

        public async Task<List<Profile>> GetAll(string? includeProperties = null)
        {
            var profiles = await _unitOfWork.profileRepository.GetAll(includeProperties);
            return profiles.ToList();
        }

        public async Task<Profile?> Get(
            Expression<Func<Profile, bool>> filter,
            string? includeProperties = null
        )
        {
            Profile? profile = await _unitOfWork.profileRepository.Get(filter, includeProperties);
            return profile;
        }

        public async Task<Profile> Update(Profile profileEntiry)
        {
            var updatedProfile = _unitOfWork.profileRepository.Update(profileEntiry);
            await _unitOfWork.Save();
            return updatedProfile;
        }

        public async Task<Boolean> isExist(string id)
        {
            Profile? profile = await _unitOfWork.profileRepository.Get(s => s.id == id);
            return profile == null ? false : true;
        }

        public async Task<Profile> PartialUpdate(string id, Profile profileEntity)
        {
            Profile? existingProfile = await _unitOfWork.profileRepository.Get(p => p.id == id);

            if (existingProfile != null)
            {
                existingProfile.name = profileEntity?.name ?? existingProfile.name;
                existingProfile.email = profileEntity?.email ?? existingProfile.email;
                existingProfile.imageUrl = profileEntity?.imageUrl ?? existingProfile.imageUrl;

                _unitOfWork.profileRepository.Update(existingProfile);
                await _unitOfWork.Save();

                return existingProfile;
            }

            return null;
        }

        public async Task<Boolean> Remove(Profile profileEntity)
        {
            Boolean result = _unitOfWork.profileRepository.Remove(profileEntity);
            if (result)
            {
                await _unitOfWork.Save();
            }

            return result;
        }
    }
}
