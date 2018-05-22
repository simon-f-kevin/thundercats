using Game_Engine.Components;
using Game_Engine.Components.Preformance;
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
        private Entity remotePlayerEntity;
        private Entity localPlayerEntity;
        private bool runDiagnostics;
        private double currentTime;
        private double nextSendUpdates;

        //diagnstics
        private NetworkDiagnosticComponent networkDiagnostic;
    

        public NetworkHandlingSystem(NetPeer peer, bool runDiagnostics = false)
        {
            this.peer = peer;
            this.runDiagnostics = runDiagnostics;
        }

        public void InitPlayers()
        {
            remotePlayerEntity = EntityHelper.GetPlayer(GameEntityFactory.REMOTE_PLAYER);
            localPlayerEntity = EntityHelper.GetPlayer(GameEntityFactory.LOCAL_PLAYER);
        }

        public void Update(GameTime gameTime)
        {
            nextSendUpdates = NetTime.Now;
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
                        response.Write(NetworkHelper.GetCurrentIPAddress());
                        // Send the response to the sender of the request
                        peer.SendDiscoveryResponse(response, message.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + message.SenderEndPoint + " name: " + message.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("Incoming game-data");
                        //Recieves data and puts it in the networkInputComponent for the remote player entity
                        if(remotePlayerEntity != null)
                        {
                            var transformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(remotePlayerEntity);
                            var velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(remotePlayerEntity);
                            //var xpos = message.ReadFloat();
                            //var ypos = message.ReadFloat();
                            //var zpos = message.ReadFloat();
                            var velx = message.ReadFloat();
                            var vely = message.ReadFloat();
                            var velz = message.ReadFloat();
                            //transformComponent.Position = new Vector3(xpos, ypos, zpos);
                            velocityComponent.Velocity = new Vector3(velx, vely, velz);

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
            currentTime = NetTime.Now;
            if (currentTime > nextSendUpdates)
            {
                foreach (NetConnection player in peer.Connections)
                {
                    foreach (NetConnection otherPlayer in peer.Connections)
                    {
                        
                        NetOutgoingMessage om = peer.CreateMessage();

                        //sends data over the network to the host/client

                        var transformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(localPlayerEntity);
                        var velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(localPlayerEntity);
                        om.Write(velocityComponent.Velocity.X);
                        om.Write(velocityComponent.Velocity.Y);
                        om.Write(velocityComponent.Velocity.Z);

                        peer.SendMessage(om, player, NetDeliveryMethod.UnreliableSequenced);
                        if (runDiagnostics)
                        {
                            Diagnostics(om);
                        }
                    }
                }

                //send data 30 times per second
                nextSendUpdates += (1.0 / 30.0);
            }
            if(nMessages > 0) Console.WriteLine(nMessages + " incoming messages!");

        }

        private void Diagnostics(NetOutgoingMessage om)
        {
            //do preformance measurement here
            networkDiagnostic = ComponentManager.Instance.GetConcurrentDictionary<NetworkDiagnosticComponent>().Values.First() as NetworkDiagnosticComponent;
            
            var sentData = om.LengthBytes;

            networkDiagnostic.DataSentThisSecond += sentData;
            networkDiagnostic.TotalDataSent += sentData;

            if(currentTime > nextSendUpdates)
            {
                networkDiagnostic.DataSentThisSecond = 0;
            }

            Console.WriteLine(om.Data[0].ToString());
        }

        
    }
}
