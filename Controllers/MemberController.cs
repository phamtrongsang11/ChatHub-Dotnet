using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.MemberContract;
using TeamChat.Models;
using TeamChat.Services.MemberService;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/members")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;
        private readonly string includeProp = "server,profile";

        public MemberController(IMemberService memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MemberResponse>> Create(CreateMemberRequest memberRequest)
        {
            Member member = _mapper.Map<Member>(memberRequest);
            Member createdMember = await _memberService.Create(member);
            return CreatedAtAction(nameof(Create), _mapper.Map<MemberResponse>(createdMember));
        }

        [HttpGet]
        public async Task<ActionResult<List<MemberResponse>>> GetAll()
        {
            List<Member> profiles = await _memberService.GetAll(includeProperties: includeProp);
            List<MemberResponse> profileResponse = profiles
                .Select(member => _mapper.Map<MemberResponse>(member))
                .ToList();
            return Ok(profileResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponse>> Get(string id)
        {
            Boolean isExist = await _memberService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            Member? foundMember = await _memberService.Get(s => s.id == id, includeProp);
            return Ok(_mapper.Map<MemberResponse>(foundMember));
        }

        [HttpGet("find")]
        public async Task<ActionResult<MemberResponse>> findByServerAndProfile(
            [FromQuery] string serverId,
            [FromQuery] string profileId
        )
        {
            Boolean isExist = await _memberService.checkIsExistByReference(serverId, profileId);
            if (!isExist)
            {
                return NotFound();
            }
            Member? foundMember = await _memberService.Get(
                s => (s.profileId == profileId && s.serverId == serverId),
                includeProp
            );
            return Ok(_mapper.Map<MemberResponse>(foundMember));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MemberResponse>> Update(
            string id,
            UpdateMemberRequest memberRequest
        )
        {
            Boolean isExist = await _memberService.isExist(id);
            if (!isExist)
                return NotFound();

            memberRequest.id = id;

            Member member = _mapper.Map<Member>(memberRequest);

            Member updatedMember = await _memberService.Update(member);

            return Ok(_mapper.Map<MemberResponse>(updatedMember));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<MemberResponse>> PartialUpdate(
            string id,
            UpdateMemberRequest memberRequest
        )
        {
            Boolean isExist = await _memberService.isExist(id);
            if (!isExist)
                return NotFound();

            Member member = _mapper.Map<Member>(memberRequest);

            Member updatedMember = await _memberService.PartialUpdate(id, member);

            return Ok(_mapper.Map<MemberResponse>(updatedMember));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Member? existingMember = await _memberService.Get(s => s.id == id);

            if (existingMember == null)
                return NotFound();

            await _memberService.Remove(existingMember);

            return NoContent();
        }

        [HttpDelete("profile")]
        public async Task<ActionResult> DeleteByProfileId([FromQuery] string profileId)
        {
            Member? existingMember = await _memberService.Get(s => s.profileId == profileId);

            if (existingMember == null)
                return NotFound();

            await _memberService.Remove(existingMember);

            return NoContent();
        }
    }
}
