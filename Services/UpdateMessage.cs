using CommunityToolkit.Mvvm.Messaging.Messages;

///////////////////////////////////////////////////////////////////////////
//                              ШАБЛОННЫЙ КOД                            //
///////////////////////////////////////////////////////////////////////////

namespace FireTestingApp_net8.Services
{
    public class UpdateMessage : ValueChangedMessage<bool>
    {
        public UpdateMessage() : base(true) { }
    }
}
