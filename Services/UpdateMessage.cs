using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.Services
{
    public class UpdateMessage : ValueChangedMessage<bool>
    {
        public UpdateMessage() : base(true) { }
    }
}
