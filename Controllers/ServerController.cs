using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChat.Contracts.MemberContract;
using TeamChat.Contracts.ServerContract;
using TeamChat.Models;
using TeamChat.Services.ServerService;

namespace TeamChat.Controller
{
    [ApiController]
    [Route("/api/servers")]
    public class ServerController : ControllerBase
    {
        private readonly IServerService _serverService;
        private readonly IMapper _mapper;
        private readonly string includeProp =
            "profile,channels,members,members.profile,channels.profile";

        public ServerController(IServerService serverService, IMapper mapper)
        {
            _serverService = serverService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ServerRespone>> Create(CreateServerRequest serverRequest)
        {
            Server server = _mapper.Map<Server>(serverRequest);
            Server createdServer = await _serverService.Create(server);
            return CreatedAtAction(nameof(Create), _mapper.Map<ServerRespone>(createdServer));
        }

        [HttpGet]
        public async Task<ActionResult<List<ServerRespone>>> GetAll()
        {
            List<Server> servers = await _serverService.GetAll(includeProperties: includeProp);
            List<ServerRespone> serverRespone = servers
                .Select(server => _mapper.Map<ServerRespone>(server))
                .ToList();
            return Ok(serverRespone);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerRespone>> Get(string id)
        {
            Boolean isExist = await _serverService.isExist(id);
            if (!isExist)
            {
                return NotFound();
            }
            Server? foundServer = await _serverService.Get(s => s.id == id, includeProp);
            return Ok(_mapper.Map<ServerRespone>(foundServer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServerRespone>> Update(
            string id,
            UpdateServerRequest serverRequest
        )
        {
            Boolean isExist = await _serverService.isExist(id);
            if (!isExist)
                return NotFound();

            serverRequest.id = id;

            Server server = _mapper.Map<Server>(serverRequest);

            Server updatedServer = await _serverService.Update(server);

            return Ok(_mapper.Map<ServerRespone>(updatedServer));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ServerRespone>> PartialUpdate(
            string id,
            UpdateServerRequest serverRequest
        )
        {
            Boolean isExist = await _serverService.isExist(id);
            if (!isExist)
                return NotFound();

            Server server = _mapper.Map<Server>(serverRequest);

            Server updatedServer = await _serverService.PartialUpdate(id, server);

            return Ok(_mapper.Map<ServerRespone>(updatedServer));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            Server? existingServer = await _serverService.Get(s => s.id == id);

            if (existingServer == null)
                return NotFound();

            await _serverService.Remove(existingServer);

            return NoContent();
        }

        // [HttpGet("profile")]
        // public async Task<ActionResult<ServerRespone>> FindFirstServerByProfileId(
        //     [FromQuery] string profileId
        // )
        // {
        //     if (string.IsNullOrEmpty(profileId))
        //         return BadRequest();

        //     Server? foundServer = await _serverService.FindFirstServerByProfileId(profileId);

        //     if (foundServer != null)
        //     {
        //         return Ok(_mapper.Map<ServerRespone>(foundServer));
        //     }

        //     return NotFound();
        // }

        [HttpGet("member")]
        public async Task<ActionResult<ServerRespone>> GetServersByMember(
            [FromQuery] string memberId
        )
        {
            if (string.IsNullOrEmpty(memberId))
                return BadRequest();

            var foundServers = await _serverService.GetServersByMemberId(memberId);

            List<ServerRespone> serverRespone = foundServers
                .Select(server => _mapper.Map<ServerRespone>(server))
                .ToList();

            var emailValue = HttpContext.Items["email"] as string;
            Console.WriteLine($"This is email {emailValue}");

            return Ok(serverRespone);
        }

        [HttpGet("invite")]
        public async Task<ActionResult<ServerRespone>> FindServerByInviteCode(
            [FromQuery] string inviteCode
        )
        {
            if (string.IsNullOrEmpty(inviteCode))
                return BadRequest();

            Server? foundServer = await _serverService.FindServerByInviteCode(inviteCode);

            if (foundServer != null)
            {
                return Ok(_mapper.Map<ServerRespone>(foundServer));
            }

            return NotFound();
        }

        [HttpPost("invite")]
        public async Task<ActionResult<ServerRespone>> SaveMemberByInviteCode(
            [FromQuery] string inviteCode,
            CreateMemberRequest memberRequest
        )
        {
            if (string.IsNullOrEmpty(inviteCode))
                return BadRequest();

            Member member = _mapper.Map<Member>(memberRequest);

            Server? foundServer = await _serverService.SaveMemberByInviteCode(inviteCode, member);

            if (foundServer != null)
            {
                return Ok(_mapper.Map<ServerRespone>(foundServer));
            }

            return NotFound();
        }
    }
}
