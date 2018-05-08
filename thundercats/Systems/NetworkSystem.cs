using Game_Engine.Systems;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thundercats.Systems
{
    /// <summary>
    /// This system handles incoming and outgoing network messages
    /// </summary>
    public class NetworkSystem : IUpdateableSystem
    {
        private NetPeer peer;

        public NetworkSystem(NetPeer peer)
        {
            this.peer = peer;
        }

        public void Update(GameTime gameTime)
        {
            double nextSendUpdates = NetTime.Now;
            NetIncomingMessage message;
            while ((message = peer.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        peer.SendDiscoveryResponse(null, message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        int xinput = message.ReadInt32();
                        int yinput = message.ReadInt32();

                        int[] pos = message.SenderConnection.Tag as int[];

                        // fancy movement logic goes here; we just append input to position
                        pos[0] += xinput;
                        pos[1] += yinput;
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();
                        if (status == NetConnectionStatus.Connected)
                        {
                            //
                            // A new player just connected!
                            //
                            Console.WriteLine(NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier) + " connected!");

                            // randomize his position and store in connection tag
                            message.SenderConnection.Tag = new int[] {
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100)
                                };
                        }

                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
                double now = NetTime.Now;
                if (now > nextSendUpdates)
                {
                    // Yes, it's time to send position updates

                    // for each player...
                    foreach (NetConnection player in server.Connections)
                    {
                        // ... send information about every other player (actually including self)
                        foreach (NetConnection otherPlayer in server.Connections)
                        {
                            // send position update about 'otherPlayer' to 'player'
                            NetOutgoingMessage om = server.CreateMessage();

                            // write who this position is for
                            om.Write(otherPlayer.RemoteUniqueIdentifier);

                            if (otherPlayer.Tag == null)
                                otherPlayer.Tag = new int[2];

                            int[] pos = otherPlayer.Tag as int[];
                            om.Write(pos[0]);
                            om.Write(pos[1]);

                            // send message
                            server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                        }
                    }

                    // schedule next update
                    nextSendUpdates += (1.0 / 30.0);
                }
            }
        }
    }
}
