using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Events;
using ACE.Managers;
using ACE.Network.GameAction;
using System.Threading;
using ACE.Database;
using ACE.Factories;
using ACE.Network;

namespace ACE.InGameManager
{
    public class GameWorld
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object objectCacheLocker = new object();
        private Dictionary<ObjectGuid, WorldObject> worldobjects = new Dictionary<ObjectGuid, WorldObject>();
        private Dictionary<uint, LandblockId> landblocks = new Dictionary<uint, LandblockId>();

        private const float maxobjectRange = 20000;
        private const float maxobjectGhostRange = 40000;
        private GameConcreteMediator meditor;

        public GameWorld(GameConcreteMediator meditor)
        {
            this.meditor = meditor;
        }

        public void PlayerEnterWorld(Session session)
        {
            Register(session.Player);
        }

        public void PlayerExitWorld(Session session)
        {
        }

        internal void Tick()
        {
        }

        public WorldObject ReadOnlyClone(ObjectGuid objectguid)
        {
            lock (objectCacheLocker)
            {
                if (worldobjects.ContainsKey(objectguid))
                {
                    // todo : mark as read only..
                    return worldobjects[objectguid];
                }
                else
                    return null;
            }
        }

        public void Register(WorldObject wo)
        {
            lock (objectCacheLocker)
            {
                if (!worldobjects.ContainsKey(wo.Guid))
                {
                    worldobjects.Add(wo.Guid, wo);
                }

                if (wo.Guid.IsPlayer())
                    LoadLandBlocksById(wo.Location.LandblockId);
            }

            var args = BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, wo);
            Broadcast(args);
        }

        private void AddPlayerTracking(List<WorldObject> wolist, Player player)
        {
            Parallel.ForEach(wolist, (o) =>
            {
                if (o.Guid.IsCreature())
                {
                    if ((o as Creature).IsAlive)
                        player.TrackObject(o);
                }
                else
                {
                    player.TrackObject(o);
                }
            });
        }

        public void LoadLandBlocksById(LandblockId landblockid)
        {
            // load world objects on this landblock and neghbor landblocks
            if (!landblocks.ContainsKey(landblockid.Raw))
            {
                // land block not already loaded.
                landblocks.Add(landblockid.Raw, landblockid);
                var objects = DatabaseManager.World.GetObjectsByLandblock(landblockid.Landblock);
                var factoryObjects = GenericObjectFactory.CreateWorldObjects(objects);
                factoryObjects.ForEach(fo => this.Register(fo));
                // todo adjancy loading..
            }
        }

        public void UnRegister(WorldObject wo)
        {
            lock (objectCacheLocker)
            {
                if (worldobjects.ContainsKey(wo.Guid))
                {
                    worldobjects.Remove(wo.Guid);
                }
            }
        }

        public void Update(WorldObject wo)
        {
            lock (objectCacheLocker)
            {
                if (worldobjects.ContainsKey(wo.Guid))
                {
                    worldobjects[wo.Guid] = wo;
                }
            }
        }

        public List<WorldObject> GetWorldObjectsInRange(WorldObject wo, float maxrange)
        {
            List<WorldObject> allworldobj = new List<WorldObject>();
            lock (objectCacheLocker)
            {
                allworldobj = worldobjects.Values.ToList();
            }
            allworldobj = allworldobj.Where(o => o.Location.SquaredDistanceTo(wo.Location) < maxrange).ToList();
            return allworldobj;
        }

        // stodo fix this.. it needs some work.  needs abstracted more.. to tied
        public void Broadcast(BroadcastEventArgs args)
        {
            WorldObject wo = args.Sender;
            List<Player> players = null;

            lock (objectCacheLocker)
            {
                players = worldobjects.Values.OfType<Player>().ToList();
            }
    
            switch (args.ActionType)
            {
                case BroadcastAction.Delete:
                    {
                        // players = players.Where(p => p.Location?.IsInQuadrant(quadrant) ?? false).ToList();

                        // If I am putting this in my inventory - you don't need to tell me about it.
                        players.RemoveAll(p => p.Guid.Full == wo.GameData.ContainerId);
                        Parallel.ForEach(players, p => p.StopTrackingObject(wo, true));
                        break;
                    }
                case BroadcastAction.AddOrUpdate:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, 2000f).OfType<Player>().ToList();

                        // players never need an update of themselves
                        players = players.Where(p => p.Guid != args.Sender.Guid).ToList();
                        Parallel.ForEach(players, p => p.TrackObject(wo));
                        break;
                    }
                case BroadcastAction.LocalChat:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, 2000f).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.ReceiveChat(wo, args.ChatMessage));
                        break;
                    }
                case BroadcastAction.PlaySound:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, 2000f).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.PlaySound(args.Sound, args.Sender.Guid));
                        break;
                    }
                case BroadcastAction.PlayParticleEffect:
                    {
                        // supresss updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, 2000f).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.PlayParticleEffect(args.Effect, args.Sender.Guid));
                        break;
                    }
                case BroadcastAction.MovementEvent:
                    {
                        // suppress updating if player is out of range of world object.= being updated or created
                        players = GetWorldObjectsInRange(wo, 2000f).OfType<Player>().ToList();
                        Parallel.ForEach(players, p => p.SendMovementEvent(args.Motion, args.Sender));
                        break;
                    }
                case BroadcastAction.BroadcastDeath:
                    {
                        // players never need an update of themselves
                        // TODO: Filter to players in range and include adjacencies
                        players = players.Where(p => p.Guid != args.Sender.Guid).ToList();
                        Parallel.ForEach(players, p => p.BroadcastPlayerDeath(args.DeathMessage.Message, args.DeathMessage.Victim, args.DeathMessage.Killer));
                        break;
                    }
            }
        }
        public void UseTime()
        {
            bool running = false;
            while (running)
            {
                List<WorldObject> allworldobj = null;
                List<Player> allplayers = null;
                List<Creature> allcreatures = null;
                List<WorldObject> movedObjects = null;
                List<WorldObject> despawnObjects = null;
                List<Creature> deadCreatures = null;

                lock (objectCacheLocker)
                {
                    // all players on this land block
                    allworldobj = worldobjects.OfType<WorldObject>().ToList();
                }

                allplayers = allworldobj.OfType<Player>().ToList();
                allcreatures = allworldobj.OfType<Creature>().ToList();

                despawnObjects = allworldobj.ToList();
                despawnObjects = despawnObjects.Where(x => x.DespawnTime > -1).ToList();

                deadCreatures = allworldobj.OfType<Creature>().ToList();
                deadCreatures = deadCreatures.Where(x => x.IsAlive == false).ToList();

                // flag them as updated now in order to reduce chance of missing an update
                // this is only for moving objects across landblocks.
                movedObjects = allworldobj.ToList();
                movedObjects = movedObjects.Where(p => p.LastUpdatedTicks >= p.LastMovementBroadcastTicks).ToList();
                movedObjects.ForEach(m => m.LastMovementBroadcastTicks = WorldManager.PortalYearTicks);

                // for all players on landblock.
                Parallel.ForEach(allplayers, player =>
                {
                    // Process Loading.
                    if (player.IsLoading)
                    {
                        List<WorldObject> wolist = null;
                        wolist = GetWorldObjectsInRange(player, maxobjectRange);
                        AddPlayerTracking(wolist, player);

                        // player loaded.
                        player.IsLoading = false;
                    }

                // Process Action Queue for player.
                QueuedGameAction action = player.ActionQueuePop();
                    if (action != null)
                        HandleGameAction(action, player);

                // Process Examination Queue for player
                QueuedGameAction examination = player.ExaminationQueuePop();
                    if (examination != null)
                        HandleGameAction(examination, player);
                });
                // UpdateStatus(allplayers.Count);

                double tickTime = WorldManager.PortalYearTicks;
                // per-creature update on landblock.
                Parallel.ForEach(allworldobj, wo =>
                {
                // Process the creatures
                wo.Tick(tickTime);
                });

                // broadcast moving objects to the world..
                // players and creatures can move.
                Parallel.ForEach(movedObjects, mo =>
                {
                // detect all world objects in ghost range
                List<WorldObject> woproxghost = new List<WorldObject>();
                    woproxghost.AddRange(GetWorldObjectsInRange(mo, 2000f));

                // for all objects in rang of this moving object or in ghost range of moving object update them.
                Parallel.ForEach(woproxghost, wo =>
                    {
                        if (mo.Guid.IsPlayer())
                        {
                        // if world object is in active zone then.
                        if (wo.Location.SquaredDistanceTo(mo.Location) <= 2000f)
                            {
                            // if world object is in active zone.
                            if (!(mo as Player).GetTrackedObjectGuids().Contains(wo.Guid))
                                    (mo as Player).TrackObject(wo);
                            }
                        // if world object is in ghost zone and outside of active zone
                        else
                                if ((mo as Player).GetTrackedObjectGuids().Contains(wo.Guid))
                                (mo as Player).StopTrackingObject(wo, true);
                        }
                    });

                    Broadcast(BroadcastEventArgs.CreateAction(BroadcastAction.AddOrUpdate, mo));
                });
                // despawn objects
                despawnObjects.ForEach(deo =>
                {
                    if (deo.DespawnTime < WorldManager.PortalYearTicks)
                    {
                        UnRegister(deo);
                    }
                });

                // respawn creatures
                deadCreatures.ForEach(dc =>
                {
                    if (dc.RespawnTime < WorldManager.PortalYearTicks)
                    {
                        dc.IsAlive = true;
                    // HandleParticleEffectEvent(dc, PlayScript.Create);
                    Register(dc);
                    }
                });

                Thread.Sleep(1);
            }
        }

        private void HandleGameAction(QueuedGameAction action, Player player)
        {
            lock (objectCacheLocker)
            {
                if (worldobjects.ContainsKey(new ObjectGuid(action.ObjectId)))
                    action.Handler(player);
                else
                    return;
            }
        }      
    }
}