using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.ProfileContract;
using TeamChat.Services.ProfileService;
using Profile = TeamChat.Models.Profile;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        public ProfileController(IProfileService profileService, IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProfileResponse>> Create(ProfileRequest profileRequest)
        {
            Profile profile = _mapper.Map<Profile>(profileRequest);
            Profile createdProfile = await _profileService.Create(profile);
            return CreatedAtAction(nameof(Create), _mapper.Map<ProfileResponse>(createdProfile));
        }

        [HttpGet]
        public async Task<ActionResult<List<ProfileResponse>>> GetAll()
        {
            List<Profile> profiles = await _profileService.GetAll(
                includeProperties: "servers,members,channels"
            );
            List<ProfileResponse> profileResponse = profiles
                .Select(profile => _mapper.Map<ProfileResponse>(profile))
                .ToList();
            return Ok(profileResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileResponse>> Get(string id)
        {
            Boolean isExist = await _profileService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            Profile? foundProfile = await _profileService.Get(
                s => s.id == id,
                "servers,members,channels"
            );
            return Ok(_mapper.Map<ProfileResponse>(foundProfile));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProfileResponse>> Update(
            string id,
            ProfileRequest profileRequest
        )
        {
            Boolean isExist = await _profileService.isExist(id);
            if (!isExist)
                return NotFound();

            profileRequest.id = id;

            Profile profile = _mapper.Map<Profile>(profileRequest);

            Profile updatedProfile = await _profileService.Update(profile);

            return Ok(_mapper.Map<ProfileResponse>(updatedProfile));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ProfileResponse>> PartialUpdate(
            string id,
            ProfileRequest profileRequest
        )
        {
            Boolean isExist = await _profileService.isExist(id);
            if (!isExist)
                return NotFound();

            Profile profile = _mapper.Map<Profile>(profileRequest);

            Profile updatedProfile = await _profileService.PartialUpdate(id, profile);

            return Ok(_mapper.Map<ProfileResponse>(updatedProfile));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Profile? existingProfile = await _profileService.Get(s => s.id == id);

            if (existingProfile == null)
                return NotFound();

            await _profileService.Remove(existingProfile);

            return NoContent();
        }
    }
}
