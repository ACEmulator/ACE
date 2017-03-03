//using ACE.Entity.Enum;
//using ACE.Network;
//using ACE.Network.Enum;
//using ACE.Network.GameMessages;
//using ACE.Network.GameMessages.Messages;
//using ACE.Network.Managers;

///// <summary>
///// All World Objects are to be created from this base set
///// Things like Model/Physics/GameData Segments can be added to this
///// The idea behind this is to make the packet generation of world objects much simplier to control
///// without needing to worry about each spec packet btye.
///// </summary>
//namespace ACE.Entity.WorldPackages
//{
//    public class WorldObjectPackage
//    {
//        public ObjectType Type { get; }
//        public ObjectGuid Guid { get; }
//        public virtual float ListeningRadius { get; protected set; } = 0f;

//        //not sure what to do about this yet...
//        // public string Name { get; protected set; }
//        // public Position Position { get; protected set; }
//        // public ObjectDescriptionFlag DescriptionFlags { get; protected set; }
//        //  public PhysicsState PhysicsState { get; protected set; }

//        //this is seq / part of the physics segment.. not sure it belong here or not yet
//        //  public uint MovementIndex { get; set; }
//        //  public uint TeleportIndex { get; set; }

//        //Segments
//        public ModelData WorldObjectSegmentModelData { get; }
//        public PhysicsData WorldObjectSegmentPhysicsData { get; }
//        public GameData WorldObjectSegmentGameData { get; }

//        public WorldObjectPackage(ObjectType type, ObjectGuid guid, ModelData modeldata, PhysicsData physicsdata, GameData gamedata)
//        {
//            Type = type;
//            Guid = guid;

//            //Intialize World Object Segments for use.
//            WorldObjectSegmentModelData = new ModelData();
//            WorldObjectSegmentModelData = modeldata;
//            WorldObjectSegmentPhysicsData = new PhysicsData();
//            WorldObjectSegmentPhysicsData = physicsdata;
//            WorldObjectSegmentGameData = new GameData();
//            WorldObjectSegmentGameData = gamedata;
//        }

//        //todo: // make this work..
//        public void Render(System.IO.BinaryWriter writer)
//        {
//            //writer.WriteGuid(Guid);
//            //WorldObjectSegmentModelData.Render(writer);
//            //WorldObjectSegmentPhysicsData.Render(writer);
//            //WorldObjectSegmentGameData.Render(writer);
//        }

//    }
//}