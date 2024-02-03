using AutoMapper;
using TeamChat.Contracts.ChannelContact;
using TeamChat.Contracts.ConversationContract;
using TeamChat.Contracts.DirectMessageContract;
using TeamChat.Contracts.MemberContract;
using TeamChat.Contracts.MessageContract;
using TeamChat.Contracts.ProfileContract;
using TeamChat.Contracts.ServerContract;
using Channel = TeamChat.Models.Channel;
using Conversation = TeamChat.Models.Conversation;
using DirectMessage = TeamChat.Models.DirectMessage;
using Member = TeamChat.Models.Member;
using Message = TeamChat.Models.Message;
using Prof = TeamChat.Models.Profile;
using Server = TeamChat.Models.Server;

namespace TeamChat.Config.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Server
            CreateMap<CreateServerRequest, Server>();
            CreateMap<UpdateServerRequest, Server>();
            CreateMap<Server, ServerRespone>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (des.profile != null)
                            des.profile.servers = null;

                        if (src.channels != null)
                            foreach (Channel channel in src.channels)
                            {
                                channel.server = null;
                            }
                        if (src.members != null)
                            foreach (Member member in src.members)
                            {
                                member.server = null;
                            }
                    }
                );

            #endregion

            #region Profile
            CreateMap<ProfileRequest, Prof>();
            CreateMap<Prof, ProfileResponse>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (src.servers != null)
                            foreach (Server server in src.servers)
                            {
                                server.profile = null;
                            }
                        if (src.channels != null)
                            foreach (Channel channel in src.channels)
                            {
                                channel.profile = null;
                            }
                        if (src.members != null)
                            foreach (Member member in src.members)
                            {
                                member.profile = null;
                            }
                    }
                );

            #endregion

            #region Member
            CreateMap<CreateMemberRequest, Member>();
            CreateMap<UpdateMemberRequest, Member>();
            CreateMap<Member, MemberResponse>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (des.profile != null)
                            des.profile.members = null;
                        if (des.server != null)
                            des.server.members = null;
                    }
                );

            #endregion

            #region Channel
            CreateMap<CreateChannelRequest, Channel>();
            CreateMap<UpdateChannelRequest, Channel>();
            CreateMap<Channel, ChannelResponse>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (des.profile != null)
                            des.profile.channels = null;
                        if (des.server != null)
                            des.server.channels = null;
                    }
                );

            #endregion

            #region Message
            CreateMap<CreateMessageRequest, Message>();
            CreateMap<UpdateMessageRequest, Message>();
            CreateMap<Message, MessageResponse>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (des.member != null)
                            des.member.messages = null;
                        if (des.channel != null)
                            des.channel.messages = null;
                    }
                );

            #endregion

            #region DirectMessage
            CreateMap<CreateDirectMessageRequest, DirectMessage>();
            CreateMap<UpdateDirectMessageRequest, DirectMessage>();
            CreateMap<DirectMessage, DirectMessageResponse>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (des.member != null)
                            des.member.directMessages = null;
                        if (des.conversation != null)
                            des.conversation.directMessages = null;
                    }
                );

            #endregion

            #region Conversation
            CreateMap<CreateConversationRequest, Conversation>();
            CreateMap<UpdateConversationRequest, Conversation>();
            CreateMap<Conversation, ConversationResponse>()
                .AfterMap(
                    (src, des) =>
                    {
                        if (des.memberOne != null)
                        {
                            des.memberOne.conversationsInitialted = null;
                            des.memberOne.conversationsReceived = null;
                        }
                        if (des.memberTwo != null)
                        {
                            des.memberTwo.conversationsReceived = null;
                            des.memberTwo.conversationsInitialted = null;
                        }
                    }
                );

            #endregion
        }
    }
}
