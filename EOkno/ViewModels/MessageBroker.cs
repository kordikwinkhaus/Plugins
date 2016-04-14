using System;

namespace EOkno.ViewModels
{
    internal class MessageBroker
    {
        internal PositionViewModel Position { get; set; }

        internal void DocumentUpdated(DocumentViewModel document)
        {
            if (Position != null)
            {
                Position.DocumentUpdated(document);
            }
        }
    }
}
