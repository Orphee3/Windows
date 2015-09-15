﻿using Microsoft.Practices.Unity;
using MidiDotNet.ExportModule;
using MidiDotNet.ExportModule.Interfaces;
using MidiDotNet.ImportModule;
using MidiDotNet.ImportModule.Interfaces;
using MidiDotNet.Shared;
using MidiDotNet.Shared.Interfaces;
using Orphee.CreationShared;
using Orphee.CreationShared.Interfaces;
using Orphee.FileManagement;
using Orphee.FileManagement.Interfaces;
using Orphee.RestApiManagement.Getters;
using Orphee.RestApiManagement.Getters.Interfaces;
using Orphee.RestApiManagement.Interfaces;
using Orphee.RestApiManagement.Posters;
using Orphee.RestApiManagement.Posters.Interfaces;
using Orphee.RestApiManagement.Senders;
using Orphee.RestApiManagement.Senders.Interfaces;
using Orphee.ViewModels;
using Orphee.ViewModels.Interfaces;

namespace Orphee.UnityModule
{
    public class UnityIocInitializator
    {
        public UnityIocInitializator(IUnityContainer container)
        {
            container.RegisterType<IPopularCreationGetter, PopularCreationGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<INotificationSender, NotificationSender>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserNewsGetter, UserNewsGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageListGetter, MessageListGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IConversationGetter, ConversationGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFriendAccepter, FriendAccepter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserFriendListGetter, UserFriendListGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserFluxGetter, UserFluxGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILikeSender, LikeSender>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICommentSender, CommentSender>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICreationCommentGetter, CreationCommentGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserCreationGetter, UserCreationGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IInvitationPageViewModel, InvitationPageViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOrpheeTrack, OrpheeTrack>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOrpheeFile, OrpheeFile>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMidiLibRepository, MidiLibRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISoundPlayer, SoundPlayer>(new ContainerControlledLifetimeManager());
            container.RegisterType<IInstrumentManager, InstrumentManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISwapManager, SwapManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITimeSignatureMessageWriter, TimeSignatureMessageWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITempoMessageWriter, TempoMessageWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProgramChangeMessageWriter, ProgramChangeMessageWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEndOfTrackMessageWriter, EndOfTrackMessageWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<INoteMessageWriter, NoteMessageWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOrpheeFileExporter, OrpheeFileExporter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFileHeaderWriter, FileHeaderWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITrackHeaderWriter, TrackHeaderWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDeltaTimeWriter, DeltaTimeWriter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOrpheeFileImporter, OrpheeFileImporter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFileHeaderReader, FileHeaderReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITrackHeaderReader, TrackHeaderReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITimeSignatureMessageReader, TimeSignatureMessageReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITempoMessageReader, TempoMessageReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProgramChangeMessageReader, ProgramChangeMessageReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDeltaTimeReader, DeltaTimeReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<INoteMessageReader, NoteMessageReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEndOfTrackMessageReader, EndOfTrackMessageReader>(new ContainerControlledLifetimeManager());
            container.RegisterType<IConnectionManager, ConnectionManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRegistrationManager, RegistrationManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IOrpheeFilesGetter, OrpheeFilesGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserListGetter, UserListGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFriendListGetter, FriendListGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFileUploader, FileUploader>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFriendshipAsker, FriendshipAsker>(new ContainerControlledLifetimeManager());
        }
    }
}
