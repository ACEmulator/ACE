using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

/// <summary>
/// All World Objects are to be created from this base set
/// Things like Model/Physics/GameData Segments can be added to this
/// The idea behind this is to make the packet generation of world objects much simplier to control
/// without needing to worry about each spec packet btye.
/// </summary>
namespace ACE.Entity.WorldPackages
{
    public class WolrdObjectPackage
    {

        public ObjectGuid Guid { get; }
        public ObjectType Type { get; }

        //not sure what to do about this yet...
        public string Name { get; protected set; }
        public Position Position { get; protected set; }
        public ObjectDescriptionFlag DescriptionFlags { get; protected set; }
        public PhysicsState PhysicsState { get; protected set; }

        //this is seq / part of the physics segment.. not sure it belong here or not yet
        public uint MovementIndex { get; set; }
        public uint TeleportIndex { get; set; }

        public virtual float ListeningRadius { get; protected set; } = 0f;

        //Segments
        public WorldObjectSegmentModelData WorldObjectSegmentModelData { get; }
        public WorldObjectSegmentPhysicsData WorldObjectSegmentPhysicsData { get; }
        public WolrdObjectSegmentGameData WolrdObjectSegmentGameData { get; }

        protected WolrdObjectPackage(ObjectType type, ObjectGuid guid)
        {
            Type = type;
            Guid = guid;
        }

        //todo: // make this work..
        protected void Render()
        {
            var objectCreate = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var objectCreateFragment = new ServerPacketFragment(0x0A, GameMessageOpcode.ObjectCreate);
            objectCreateFragment.Payload.WriteGuid(Guid);

            WorldObjectSegmentModelData.Render();
            WorldObjectSegmentPhysicsData.Render();
            WolrdObjectSegmentGameData.Render();
        }

    }

    }
