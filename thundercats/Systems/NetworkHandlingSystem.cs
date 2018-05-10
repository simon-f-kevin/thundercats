using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
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
    public class NetworkHandlingSystem : IUpdateableSystem
    {
        private NetPeer peer;
        private Queue<NetIncomingMessage> messageQueue;

        public NetworkHandlingSystem(NetPeer peer)
        {
            this.peer = peer;
            messageQueue = new Queue<NetIncomingMessage>();
        }

        public void Update(GameTime gameTime)
        {
            Entity remotePlayerEntity = GetPlayer(GameEntityFactory.REMOTE_PLAYER);
            Entity localPlayerEntity = GetPlayer(GameEntityFactory.LOCAL_PLAYER);

            double nextSendUpdates = NetTime.Now;
            var listOfIncomingMessages = new List<NetIncomingMessage>();
            var nMessages = peer.ReadMessages(listOfIncomingMessages);
            
            foreach (var message in listOfIncomingMessages)
            {
                //RECIEVE DATA
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        // Create a response and write some example data to it
                        NetOutgoingMessage response = peer.CreateMessage();
                        response.Write(NetworkHelper.GetCurrentHostname());

                        // Send the response to the sender of the request
                        peer.SendDiscoveryResponse(response, message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + message.SenderEndPoint + " name: " + message.ReadString());
                        break;
                    case NetIncomingMessageType.Data:

                        //Recieves data and puts it in the networkInputComponent for the remote player entity
                        if(remotePlayerEntity != null)
                        {
                            NetworkInputComponent networkInputComponent = new NetworkInputComponent(remotePlayerEntity);
                            var data = message.Data;
                        }
                        
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();
                        if (status == NetConnectionStatus.RespondedConnect)
                        {
                            Console.WriteLine(NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier) + " responded to connection!");
                        }
                        if (status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine(NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier) + " connected, yay!");
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
            }
            /********************************************************************************************************/
            /*********************************************SEND DATA**************************************************/
            /********************************************************************************************************/
            double now = NetTime.Now;
            if (now > nextSendUpdates)
            {
                foreach (NetConnection player in peer.Connections)
                {
                    foreach (NetConnection otherPlayer in peer.Connections)
                    {
                        NetOutgoingMessage om = peer.CreateMessage();

                        //sends networkInputComponents' data over the network to the host/client
                        var networkInputComponent = ComponentManager.Instance.GetComponentOfEntity<NetworkInputComponent>(localPlayerEntity);
                        om.Data = new byte[] {Convert.ToByte(networkInputComponent.MoveForward), Convert.ToByte(networkInputComponent.MoveLeft),
                                Convert.ToByte(networkInputComponent.MoveRight), Convert.ToByte(networkInputComponent.Jump) };
                        peer.SendMessage(om, player, NetDeliveryMethod.ReliableOrdered);
                    }
                }

                nextSendUpdates += (1.0 / 30.0);
            }
            Console.WriteLine(nMessages + " incoming messages!");
            //Console.WriteLine(message.MessageType.ToString());
        }

        private Entity GetPlayer(string playertype)
        {
            Entity result = null;
            var playerEntities = ComponentManager.Instance.GetDictionary<PlayerComponent>().Keys;
            foreach (var player in playerEntities)
            {
                if (player.EntityTypeName.Equals(playertype)) result = player;
            }
            return result;
        }
    }
}
